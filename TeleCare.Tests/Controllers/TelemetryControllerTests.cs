using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeleCare.Controllers;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;
using TeleCare.Enum;

namespace TeleCare.Tests.Controllers
{
    [TestFixture]
    public class TelemetryControllerTests
    {
        private Mock<ITelemetryService> _mockService;
        private TelemetryController _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ITelemetryService>();
            _controller = new TelemetryController(_mockService.Object);
        }

        #endregion

        #region Helper

        private TelemetryCreateDto GetValidDto()
        {
            return new TelemetryCreateDto
            {
                DeviceID = 1,
                PatientID = 1,
                MetricName = "HeartRate",
                Value = 75,
                Unit = "bpm",
                Timestamp = System.DateTime.Now,
                Source = TelemetrySource.Device
            };
        }

        #endregion

        #region POST createTelemetryRecord

        [Test]
        public async Task CreateTelemetry_Valid_ReturnsOk()
        {
            _mockService
                .Setup(s => s.createTelemetryRecordAsync(It.IsAny<TelemetryCreateDto>()))
                .ReturnsAsync(new TelemetryResponseDto { TelemetryID = 1 });

            var result = await _controller.createTelemetryRecord(GetValidDto());

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
        }

        [Test]
        public async Task CreateTelemetry_Null_ReturnsBadRequest()
        {
            var result = await _controller.createTelemetryRecord(null);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(TelemetryConstants.RequestBodyNull, bad.Value);
        }

        [Test]
        public async Task CreateTelemetry_ServiceReturnsNull_ReturnsOkNull()
        {
            _mockService
                .Setup(s => s.createTelemetryRecordAsync(It.IsAny<TelemetryCreateDto>()))
                .ReturnsAsync((TelemetryResponseDto?)null);

            var result = await _controller.createTelemetryRecord(GetValidDto());

            var ok = result as OkObjectResult;
            Assert.IsNull(ok!.Value);
        }

        #endregion

        #region GET getTelemetryDetailsByTelemetryId

        [Test]
        public async Task GetTelemetry_ValidId_ReturnsOk()
        {
            _mockService
                .Setup(s => s.getTelemetryDetailsByTelemetryIdAsync(1))
                .ReturnsAsync(new TelemetryResponseDto { TelemetryID = 1 });

            var result = await _controller.getTelemetryDetailsByTelemetryId(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetTelemetry_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.getTelemetryDetailsByTelemetryId(0);

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(TelemetryConstants.InvalidTelemetryId, bad!.Value);
        }

        [Test]
        public async Task GetTelemetry_NotFound_ReturnsNotFound()
        {
            _mockService
                .Setup(s => s.getTelemetryDetailsByTelemetryIdAsync(It.IsAny<int>()))
                .ReturnsAsync((TelemetryResponseDto?)null);

            var result = await _controller.getTelemetryDetailsByTelemetryId(5);

            var notFound = result as NotFoundObjectResult;
            Assert.AreEqual(TelemetryConstants.TelemetryNotFound, notFound!.Value);
        }

        #endregion

        #region GET getFilteredTelemetryRecords

        [Test]
        public async Task GetFilteredTelemetry_ReturnsOk()
        {
            _mockService
                .Setup(s => s.getFilteredTelemetryRecordsAsync(It.IsAny<TelemetryQueryDto>()))
                .ReturnsAsync(new List<TelemetryResponseDto>());

            var result = await _controller.getFilteredTelemetryRecords(new TelemetryQueryDto());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetFilteredTelemetry_NullQuery_ReturnsOk()
        {
            _mockService
                .Setup(s => s.getFilteredTelemetryRecordsAsync(It.IsAny<TelemetryQueryDto>()))
                .ReturnsAsync(new List<TelemetryResponseDto>());

            var result = await _controller.getFilteredTelemetryRecords(null);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetFilteredTelemetry_ServiceCalledOnce()
        {
            var query = new TelemetryQueryDto();

            await _controller.getFilteredTelemetryRecords(query);

            _mockService.Verify(
                s => s.getFilteredTelemetryRecordsAsync(query),
                Times.Once);
        }

        #endregion
    }
}