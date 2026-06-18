using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeleCare.Controllers;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;

namespace TeleCare.Tests.Controllers
{
    [TestFixture]
    public class NotificationControllerTests
    {
        private Mock<INotificationService>? _mockService;
        private AdminNotificationController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<INotificationService>();
            _controller = new AdminNotificationController(_mockService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region GetAllNotifications

        [Test]
        public async Task GetAllNotifications_WhenDataExists_ReturnsOkWithData()
        {
            var data = new List<NotificationResponseDto>
            {
                new NotificationResponseDto
                {
                    NotificationID = 1,
                    Message = "Test",
                    Category = "Alert",
                    Status = "Sent"
                }
            };

            _mockService!
                .Setup(s => s.GetAllNotificationsAsync())
                .ReturnsAsync(data);

            var result = await _controller!.GetAllNotifications();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllNotifications_WhenEmpty_ReturnsOkEmptyList()
        {
            _mockService!
                .Setup(s => s.GetAllNotificationsAsync())
                .ReturnsAsync(new List<NotificationResponseDto>());

            var result = await _controller!.GetAllNotifications();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllNotifications_ServiceCalledOnce()
        {
            await _controller!.GetAllNotifications();

            _mockService!
                .Verify(s => s.GetAllNotificationsAsync(), Times.Once);
        }

        #endregion

        #region GetNotificationsForUser

        [Test]
        public async Task GetNotificationsForUser_ValidUser_ReturnsOk()
        {
            int userId = 1;

            var data = new List<NotificationResponseDto>
            {
                new NotificationResponseDto
                {
                    UserID = userId,
                    Message = "Msg",
                    Category = "Info",
                    Status = "Sent"
                }
            };

            _mockService!
                .Setup(s => s.GetNotificationsForUserAsync(userId))
                .ReturnsAsync(data);

            var result = await _controller!.GetNotificationsForUser(userId);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetNotificationsForUser_EmptyResult_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.GetNotificationsForUserAsync(1))
                .ReturnsAsync(new List<NotificationResponseDto>());

            var result = await _controller!.GetNotificationsForUser(1);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetNotificationsForUser_ServiceCalledOnce()
        {
            await _controller!.GetNotificationsForUser(5);

            _mockService!
                .Verify(s => s.GetNotificationsForUserAsync(5), Times.Once);
        }

        #endregion

        #region SendNotification

        [Test]
        public async Task SendNotification_ValidDto_ReturnsOk()
        {
            var dto = new NotificationSendDto
            {
                Message = "Hello",
                Category = "Alert"
            };

            var result = await _controller!.SendNotification(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordCreated, ok.Value);

            _mockService!
                .Verify(s => s.SendNotificationAsync(dto), Times.Once);
        }

        [Test]
        public async Task SendNotification_InvalidModel_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.SendNotification(new NotificationSendDto
            {
                Message = "Test",
                Category = "Alert"
            });

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task SendNotification_ServiceCalledOnce()
        {
            var dto = new NotificationSendDto
            {
                Message = "Test",
                Category = "Alert"
            };

            await _controller!.SendNotification(dto);

            _mockService!
                .Verify(s => s.SendNotificationAsync(dto), Times.Once);
        }

        #endregion

        #region DeleteNotification

        [Test]
        public async Task DeleteNotification_ValidId_ReturnsOk()
        {
            var result = await _controller!.DeleteNotification(1);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordDeleted, ok.Value);
        }

        [Test]
        public async Task DeleteNotification_ServiceCalledOnce()
        {
            await _controller!.DeleteNotification(10);

            _mockService!
                .Verify(s => s.DeleteNotificationAsync(10), Times.Once);
        }

        [Test]
        public async Task DeleteNotification_ZeroId_ReturnsOk()
        {
            var result = await _controller!.DeleteNotification(0);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        #endregion
    }
}
