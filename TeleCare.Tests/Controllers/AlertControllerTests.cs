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
    public class AlertControllerTests
    {
        private Mock<IAlertService>? _mockService;
        private AlertController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IAlertService>();
            _controller = new AlertController(_mockService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region POST createAlert

        [Test]
        public async Task CreateAlert_ValidDto_ReturnsOkWithResult()
        {
            var dto = new AlertCreateDto();

            var response = new AlertResponseDto
            {
                AlertID = 1,
                Status = "Active"
            };

            _mockService!
                .Setup(s => s.createAlertAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller!.createAlert(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task CreateAlert_NullDto_ReturnsBadRequest()
        {
            var result = await _controller!.createAlert(null);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(ApplicationMessages.RequestBodyNull, bad.Value);
        }

        [Test]
        public async Task CreateAlert_ServiceReturnsNull_ReturnsOkNull()
        {
            var dto = new AlertCreateDto();

            _mockService!
                .Setup(s => s.createAlertAsync(dto))
                .ReturnsAsync((AlertResponseDto?)null);

            var result = await _controller!.createAlert(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.IsNull(ok.Value);
        }

        #endregion

        #region GET getAllAlerts

        [Test]
        public async Task GetAllAlerts_WhenDataExists_ReturnsOk()
        {
            var data = new List<AlertResponseDto>
            {
                new AlertResponseDto { AlertID = 1, Status = "Active" }
            };

            _mockService!
                .Setup(s => s.getAllAlertsAsync())
                .ReturnsAsync(data);

            var result = await _controller!.getAllAlerts();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllAlerts_WhenEmpty_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.getAllAlertsAsync())
                .ReturnsAsync(new List<AlertResponseDto>());

            var result = await _controller!.getAllAlerts();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllAlerts_ServiceCalledOnce()
        {
            await _controller!.getAllAlerts();

            _mockService!
                .Verify(s => s.getAllAlertsAsync(), Times.Once);
        }

        #endregion

        #region GET getAlertById

        [Test]
        public async Task GetAlertById_ValidId_ReturnsOk()
        {
            int id = 1;

            var response = new AlertResponseDto { AlertID = id };

            _mockService!
                .Setup(s => s.getAlertByIdAsync(id))
                .ReturnsAsync(response);

            var result = await _controller!.getAlertById(id);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task GetAlertById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller!.getAlertById(0);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(ApplicationMessages.InvalidAlertId, bad.Value);
        }

        [Test]
        public async Task GetAlertById_NotFound_ReturnsNotFound()
        {
            int id = 5;

            _mockService!
                .Setup(s => s.getAlertByIdAsync(id))
                .ReturnsAsync((AlertResponseDto?)null);

            var result = await _controller!.getAlertById(id);

            var notFound = result as NotFoundObjectResult;
            Assert.NotNull(notFound);
            Assert.AreEqual(404, notFound!.StatusCode);
            Assert.AreEqual(ApplicationMessages.AlertNotFound, notFound.Value);
        }

        #endregion

        #region PUT updateAlert

        [Test]
        public async Task UpdateAlert_ValidInput_ReturnsOk()
        {
            int id = 1;

            var dto = new AlertCreateDto();

            var response = new AlertResponseDto { AlertID = id };

            _mockService!
                .Setup(s => s.updateAlertAsync(id, dto))
                .ReturnsAsync(response);

            var result = await _controller!.updateAlert(id, dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task UpdateAlert_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller!.updateAlert(0, new AlertCreateDto());

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(ApplicationMessages.InvalidAlertId, bad.Value);
        }

        [Test]
        public async Task UpdateAlert_NotFound_ReturnsNotFound()
        {
            int id = 10;

            var dto = new AlertCreateDto();

            _mockService!
                .Setup(s => s.updateAlertAsync(id, dto))
                .ReturnsAsync((AlertResponseDto?)null);

            var result = await _controller!.updateAlert(id, dto);

            var notFound = result as NotFoundObjectResult;
            Assert.NotNull(notFound);
            Assert.AreEqual(404, notFound!.StatusCode);
            Assert.AreEqual(ApplicationMessages.AlertNotFound, notFound.Value);
        }

        [Test]
        public async Task UpdateAlert_NullDto_ReturnsBadRequest()
        {
            var result = await _controller!.updateAlert(1, null);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(ApplicationMessages.RequestBodyNull, bad.Value);
        }

        #endregion
    }
}
