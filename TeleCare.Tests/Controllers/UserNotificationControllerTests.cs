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
    public class UserNotificationControllerTests
    {
        private Mock<INotificationService> _mockService;
        private UserNotificationController _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<INotificationService>();
            _controller = new UserNotificationController(_mockService.Object);
        }

        #endregion

        #region GET GetMyNotifications

        [Test]
        public async Task GetMyNotifications_ValidUser_ReturnsOkWithData()
        {
            var data = new List<NotificationResponseDto>
            {
                new NotificationResponseDto
                {
                    NotificationID = 1,
                    UserID = 1,
                    Message = "Test",
                    Category = "Alert",
                    Status = "Unread"
                }
            };

            _mockService
                .Setup(s => s.GetNotificationsForUserAsync(It.IsAny<int>()))
                .ReturnsAsync(data);

            var result = await _controller.GetMyNotifications(1);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetMyNotifications_EmptyList_ReturnsOk()
        {
            _mockService
                .Setup(s => s.GetNotificationsForUserAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<NotificationResponseDto>());

            var result = await _controller.GetMyNotifications(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetMyNotifications_ServiceCalledOnce()
        {
            await _controller.GetMyNotifications(5);

            _mockService.Verify(
                s => s.GetNotificationsForUserAsync(5),
                Times.Once);
        }

        #endregion

        #region PATCH MarkAsRead

        [Test]
        public async Task MarkAsRead_ValidId_ReturnsOkMessage()
        {
            var result = await _controller.MarkAsRead(1);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordUpdated, ok.Value);
        }

        [Test]
        public async Task MarkAsRead_ServiceCalledOnce()
        {
            await _controller.MarkAsRead(10);

            _mockService.Verify(
                s => s.MarkAsReadAsync(10),
                Times.Once);
        }

        [Test]
        public async Task MarkAsRead_ZeroId_ReturnsOk()
        {
            // Controller does NOT validate ID → still returns OK
            var result = await _controller.MarkAsRead(0);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        #endregion
    }
}