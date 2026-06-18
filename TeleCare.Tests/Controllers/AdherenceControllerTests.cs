using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using TeleCare.Controllers;
using TeleCare.Service.Interface;
using TeleCare.DTO;
using TeleCare.Constants;
using TeleCare.Enum;

namespace TeleCare.Tests.Controllers
{
    [TestFixture]
    public class AdherenceControllerTests
    {
        private Mock<IAdherenceService> _mockService;
        private AdherenceController _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IAdherenceService>();
            _controller = new AdherenceController(_mockService.Object);

            // Ensure ModelState is clean initially
            _controller.ModelState.Clear();
        }

        #endregion

        #region POST createAdherenceRecord

        [Test]
        public async Task CreateAdherenceRecord_ValidDto_ReturnsOkWithResult()
        {
            // Arrange
            var dto = new AdherenceCreateDto
            {
                MedID = 1,
                PatientID = 1,
                Status = AdherenceStatus.Taken
            };

            var response = new AdherenceResponseDto
            {
                AdhID = 10,
                MedID = 1,
                PatientID = 1,
                Status = AdherenceStatus.Taken
            };

            _mockService
                .Setup(s => s.createAdherenceRecordAsync(dto))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.createAdherenceRecord(dto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(response, okResult.Value);
        }

        [Test]
        public async Task CreateAdherenceRecord_NullDto_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.createAdherenceRecord(null);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual(AdherenceConstants.RequestBodyNull, badRequest.Value);
        }

        [Test]
        public async Task CreateAdherenceRecord_ServiceReturnsNull_ReturnsOkWithNull()
        {
            // Arrange
            var dto = new AdherenceCreateDto();

            _mockService
                .Setup(s => s.createAdherenceRecordAsync(dto))
                .ReturnsAsync((AdherenceResponseDto)null);

            // Act
            var result = await _controller.createAdherenceRecord(dto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNull(okResult.Value);
        }

        #endregion

        #region GET getAdherenceDetailsByAdhID

        [Test]
        public async Task GetAdherenceDetailsByAdhID_ValidId_ReturnsOk()
        {
            // Arrange
            int id = 1;
            var response = new AdherenceResponseDto { AdhID = id };

            _mockService
                .Setup(s => s.getAdherenceDetailsByAdhIDAsync(id))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.getAdherenceDetailsByAdhID(id);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(response, okResult.Value);
        }

        [Test]
        public async Task GetAdherenceDetailsByAdhID_InvalidId_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.getAdherenceDetailsByAdhID(0);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual(AdherenceConstants.InvalidAdherenceId, badRequest.Value);
        }

        [Test]
        public async Task GetAdherenceDetailsByAdhID_NotFound_ReturnsNotFound()
        {
            // Arrange
            int id = 5;

            _mockService
                .Setup(s => s.getAdherenceDetailsByAdhIDAsync(id))
                .ReturnsAsync((AdherenceResponseDto)null);

            // Act
            var result = await _controller.getAdherenceDetailsByAdhID(id);

            // Assert
            var notFound = result as NotFoundObjectResult;
            Assert.NotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.AreEqual(AdherenceConstants.RecordNotFound, notFound.Value);
        }

        #endregion

        #region PUT updateAdherenceDetailsByAdhID

        [Test]
        public async Task UpdateAdherenceDetails_ValidInput_ReturnsOk()
        {
            // Arrange
            int id = 1;
            var dto = new AdherenceUpdateDto();
            var response = new AdherenceResponseDto { AdhID = id };

            _mockService
                .Setup(s => s.updateAdherenceDetailsByAdhIDAsync(id, dto))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.updateAdherenceDetailsByAdhID(id, dto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(response, okResult.Value);
        }

        [Test]
        public async Task UpdateAdherenceDetails_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var dto = new AdherenceUpdateDto();

            // Act
            var result = await _controller.updateAdherenceDetailsByAdhID(0, dto);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual(AdherenceConstants.InvalidAdherenceId, badRequest.Value);
        }

        [Test]
        public async Task UpdateAdherenceDetails_NotFound_ReturnsNotFound()
        {
            // Arrange
            int id = 10;
            var dto = new AdherenceUpdateDto();

            _mockService
                .Setup(s => s.updateAdherenceDetailsByAdhIDAsync(id, dto))
                .ReturnsAsync((AdherenceResponseDto)null);

            // Act
            var result = await _controller.updateAdherenceDetailsByAdhID(id, dto);

            // Assert
            var notFound = result as NotFoundObjectResult;
            Assert.NotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.AreEqual(AdherenceConstants.RecordNotFound, notFound.Value);
        }

        #endregion

        #region GET getFilteredAdherenceRecords

        [Test]
        public async Task GetFilteredAdherenceRecords_ValidQuery_ReturnsOkList()
        {
            // Arrange
            var query = new AdherenceQueryDto();
            var list = new List<AdherenceResponseDto>
            {
                new AdherenceResponseDto { AdhID = 1 }
            };

            _mockService
                .Setup(s => s.getFilteredAdherenceRecordsAsync(query))
                .ReturnsAsync(list);

            // Act
            var result = await _controller.getFilteredAdherenceRecords(query);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(list, okResult.Value);
        }

        [Test]
        public async Task GetFilteredAdherenceRecords_NullQuery_ReturnsOkEmpty()
        {
            // Arrange
            _mockService
                .Setup(s => s.getFilteredAdherenceRecordsAsync(null))
                .ReturnsAsync(new List<AdherenceResponseDto>());

            // Act
            var result = await _controller.getFilteredAdherenceRecords(null);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
        }

        [Test]
        public async Task GetFilteredAdherenceRecords_ServiceReturnsEmpty_ReturnsOkEmptyList()
        {
            // Arrange
            var query = new AdherenceQueryDto();

            _mockService
                .Setup(s => s.getFilteredAdherenceRecordsAsync(query))
                .ReturnsAsync(new List<AdherenceResponseDto>());

            // Act
            var result = await _controller.getFilteredAdherenceRecords(query);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOf<List<AdherenceResponseDto>>(okResult.Value);
        }

        #endregion
    }
}
