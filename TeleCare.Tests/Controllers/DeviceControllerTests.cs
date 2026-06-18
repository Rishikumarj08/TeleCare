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
    public class DeviceControllerTests
    {
        private Mock<IDeviceService>? _mockService;
        private DeviceController? _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IDeviceService>();
            _controller = new DeviceController(_mockService.Object);
        }

        #region CreateDeviceRecord

        [Test]
        public async Task CreateDeviceRecord_ValidDto_ReturnsOk()
        {
            var dto = new DeviceCreateDto { SerialNumber = "123", Model = "ModelX" };
            var response = new DeviceResponseDto { DeviceID = 1 };

            _mockService!
                .Setup(s => s.createDeviceRecordAsync(It.IsAny<DeviceCreateDto>()))
                .ReturnsAsync(response);

            var result = await _controller!.createDeviceRecord(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task CreateDeviceRecord_NullDto_ReturnsBadRequest()
        {
            var result = await _controller!.createDeviceRecord(null);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(DeviceConstants.RequestBodyNull, bad!.Value);
        }

        [Test]
        public async Task CreateDeviceRecord_ServiceReturnsNull_ReturnsOkNull()
        {
            _mockService!
                .Setup(s => s.createDeviceRecordAsync(It.IsAny<DeviceCreateDto>()))
                .ReturnsAsync((DeviceResponseDto?)null);

            var result = await _controller!.createDeviceRecord(new DeviceCreateDto());

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.IsNull(ok!.Value);
        }

        #endregion

        #region GetDeviceDetailsByDeviceId

        [Test]
        public async Task GetDevice_ValidId_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.getDeviceDetailsByDeviceIdAsync(1))
                .ReturnsAsync(new DeviceResponseDto { DeviceID = 1 });

            var result = await _controller!.getDeviceDetailsByDeviceId(1);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetDevice_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller!.getDeviceDetailsByDeviceId(0);

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(DeviceConstants.InvalidDeviceId, bad!.Value);
        }

        [Test]
        public async Task GetDevice_NotFound_ReturnsNotFound()
        {
            _mockService!
                .Setup(s => s.getDeviceDetailsByDeviceIdAsync(5))
                .ReturnsAsync((DeviceResponseDto?)null);

            var result = await _controller!.getDeviceDetailsByDeviceId(5);

            var notFound = result as NotFoundObjectResult;
            Assert.AreEqual(DeviceConstants.DeviceNotFound, notFound!.Value);
        }

        #endregion

        #region UpdateDevice

        [Test]
        public async Task UpdateDevice_Valid_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.updateDeviceDetailsByDeviceIdAsync(
                    It.IsAny<int>(), It.IsAny<DeviceUpdateDto>()))
                .ReturnsAsync(new DeviceResponseDto { DeviceID = 1 });

            var result = await _controller!.updateDeviceDetailsByDeviceId(
                1, new DeviceUpdateDto());

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task UpdateDevice_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller!.updateDeviceDetailsByDeviceId(
                0, new DeviceUpdateDto());

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(DeviceConstants.InvalidDeviceId, bad!.Value);
        }

        [Test]
        public async Task UpdateDevice_NotFound_ReturnsNotFound()
        {
            _mockService!
                .Setup(s => s.updateDeviceDetailsByDeviceIdAsync(
                    It.IsAny<int>(), It.IsAny<DeviceUpdateDto>()))
                .ReturnsAsync((DeviceResponseDto?)null);

            var result = await _controller!.updateDeviceDetailsByDeviceId(
                5, new DeviceUpdateDto());

            var notFound = result as NotFoundObjectResult;
            Assert.AreEqual(DeviceConstants.DeviceNotFound, notFound!.Value);
        }

        #endregion

        #region DeleteDevice

        [Test]
        public async Task DeleteDevice_Valid_ReturnsOk()
        {
            var result = await _controller!.deleteDeviceRecord(1);

            var ok = result as OkObjectResult;
            Assert.AreEqual(DeviceConstants.DeviceDeleted, ok!.Value);

            _mockService!.Verify(s => s.deleteDeviceRecordAsync(1), Times.Once);
        }

        [Test]
        public async Task DeleteDevice_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller!.deleteDeviceRecord(0);

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(DeviceConstants.InvalidDeviceId, bad!.Value);
        }

        [Test]
        public async Task DeleteDevice_ServiceCalledOnce()
        {
            await _controller!.deleteDeviceRecord(10);

            _mockService!
                .Verify(s => s.deleteDeviceRecordAsync(10), Times.Once);
        }

        #endregion

        #region GetFilteredDevices

        [Test]
        public async Task GetFilteredDevices_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.getFilteredDeviceRecordsAsync(It.IsAny<DeviceQueryDto>()))
                .ReturnsAsync(new List<DeviceResponseDto>());

            var result = await _controller!.getFilteredDeviceRecords(new DeviceQueryDto());

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
        }

        [Test]
        public async Task GetFilteredDevices_NullQuery_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.getFilteredDeviceRecordsAsync(It.IsAny<DeviceQueryDto>()))
                .ReturnsAsync(new List<DeviceResponseDto>());

            var result = await _controller!.getFilteredDeviceRecords(null);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
        }

        [Test]
        public async Task GetFilteredDevices_ServiceCalledOnce()
        {
            var query = new DeviceQueryDto();

            await _controller!.getFilteredDeviceRecords(query);

            _mockService!
                .Verify(s => s.getFilteredDeviceRecordsAsync(query), Times.Once);
        }

        #endregion
    }
}