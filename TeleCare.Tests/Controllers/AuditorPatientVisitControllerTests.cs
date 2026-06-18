using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeleCare.Controllers;
using TeleCare.DTO;
using TeleCare.Service.Interface;

namespace TeleCare.Tests.Controllers
{
    [TestFixture]
    public class AuditorPatientVisitControllerTests
    {
        private Mock<IAuditorVisitNoteService>? _mockService;
        private AuditorPatientVisitController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IAuditorVisitNoteService>();
            _controller = new AuditorPatientVisitController(_mockService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region GetAllPatientVisits

        [Test]
        public async Task GetAllPatientVisits_WhenDataExists_ReturnsOkWithData()
        {
            // Arrange
            var data = new List<AuditorVisitNoteResponseDto>
            {
                new AuditorVisitNoteResponseDto
                {
                    PatientName = "John",
                    Notes = "Test Notes",
                    Diagnosis = "Flu",
                    Orders = "Rest",
                    AttachmentName = "file.pdf",
                    VisitNoteStatus = "Completed"
                }
            };

            _mockService!
                .Setup(s => s.GetAllVisitNotesAsync())
                .ReturnsAsync(data);

            // Act
            var result = await _controller!.GetAllPatientVisits();

            // Assert
            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllPatientVisits_WhenEmptyList_ReturnsOk()
        {
            // Arrange
            _mockService!
                .Setup(s => s.GetAllVisitNotesAsync())
                .ReturnsAsync(new List<AuditorVisitNoteResponseDto>());

            // Act
            var result = await _controller!.GetAllPatientVisits();

            // Assert
            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllPatientVisits_ServiceCalledOnce_Verified()
        {
            // Act
            await _controller!.GetAllPatientVisits();

            // Assert
            _mockService!
                .Verify(s => s.GetAllVisitNotesAsync(), Times.Once);
        }

        #endregion

        #region SearchPatientVisits

        [Test]
        public async Task SearchPatientVisits_ValidModel_ReturnsOkWithResults()
        {
            // Arrange
            var dto = new SearchVisitNoteDto();

            var data = new List<AuditorVisitNoteResponseDto>();

            _mockService!
                .Setup(s => s.SearchVisitNotesAsync(dto))
                .ReturnsAsync(data);

            // Act
            var result = await _controller!.SearchPatientVisits(dto);

            // Assert
            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task SearchPatientVisits_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller!.ModelState.AddModelError("Error", "Invalid");

            // Act
            var result = await _controller.SearchPatientVisits(new SearchVisitNoteDto());

            // Assert
            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task SearchPatientVisits_EmptyResult_ReturnsOk()
        {
            // Arrange
            var dto = new SearchVisitNoteDto();

            _mockService!
                .Setup(s => s.SearchVisitNotesAsync(dto))
                .ReturnsAsync(new List<AuditorVisitNoteResponseDto>());

            // Act
            var result = await _controller!.SearchPatientVisits(dto);

            // Assert
            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
        }

        #endregion
    }
}