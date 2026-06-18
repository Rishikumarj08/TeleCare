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
    public class VisitNoteControllerTests
    {
        private Mock<IVisitNoteService> _mockService;
        private VisitNoteController _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IVisitNoteService>();
            _controller = new VisitNoteController(_mockService.Object);
        }

        #endregion

        #region Helper

        private VisitNoteCreateDto GetValidDto()
        {
            return new VisitNoteCreateDto
            {
                AppID = 1,
                PatientID = 1,
                ClinicianID = 1,
                NoteText = "Test Note"
            };
        }

        #endregion

        #region POST createVisitNote

        [Test]
        public async Task CreateVisitNote_Valid_ReturnsOk()
        {
            _mockService
                .Setup(s => s.createVisitNoteAsync(It.IsAny<VisitNoteCreateDto>()))
                .ReturnsAsync(new VisitNoteResponseDto { NoteID = 1 });

            var result = await _controller.createVisitNote(GetValidDto());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateVisitNote_NullDto_ReturnsBadRequest()
        {
            var result = await _controller.createVisitNote(null);

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(ApplicationMessages.RequestBodyNull, bad!.Value);
        }

        [Test]
        public async Task CreateVisitNote_ServiceReturnsNull_ReturnsOkNull()
        {
            _mockService
                .Setup(s => s.createVisitNoteAsync(It.IsAny<VisitNoteCreateDto>()))
                .ReturnsAsync((VisitNoteResponseDto?)null);

            var result = await _controller.createVisitNote(GetValidDto());

            var ok = result as OkObjectResult;
            Assert.IsNull(ok!.Value);
        }

        #endregion

        #region GET getAllVisitNotes

        [Test]
        public async Task GetAllVisitNotes_WithData_ReturnsOk()
        {
            _mockService
                .Setup(s => s.getAllVisitNotesAsync())
                .ReturnsAsync(new List<VisitNoteResponseDto>());

            var result = await _controller.getAllVisitNotes();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllVisitNotes_Empty_ReturnsOk()
        {
            _mockService
                .Setup(s => s.getAllVisitNotesAsync())
                .ReturnsAsync(new List<VisitNoteResponseDto>());

            var result = await _controller.getAllVisitNotes();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllVisitNotes_ServiceCalledOnce()
        {
            await _controller.getAllVisitNotes();

            _mockService.Verify(s => s.getAllVisitNotesAsync(), Times.Once);
        }

        #endregion

        #region GET getVisitNoteById

        [Test]
        public async Task GetVisitNoteById_Valid_ReturnsOk()
        {
            _mockService
                .Setup(s => s.getVisitNoteByIdAsync(1))
                .ReturnsAsync(new VisitNoteResponseDto());

            var result = await _controller.getVisitNoteById(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetVisitNoteById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.getVisitNoteById(0);

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(ApplicationMessages.InvalidVisitNoteId, bad!.Value);
        }

        [Test]
        public async Task GetVisitNoteById_NotFound_ReturnsNotFound()
        {
            _mockService
                .Setup(s => s.getVisitNoteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((VisitNoteResponseDto?)null);

            var result = await _controller.getVisitNoteById(5);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion

        #region PUT updateVisitNote

        [Test]
        public async Task UpdateVisitNote_Valid_ReturnsOk()
        {
            _mockService
                .Setup(s => s.updateVisitNoteAsync(It.IsAny<int>(), It.IsAny<VisitNoteCreateDto>()))
                .ReturnsAsync(new VisitNoteResponseDto());

            var result = await _controller.updateVisitNote(1, GetValidDto());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateVisitNote_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.updateVisitNote(0, GetValidDto());

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(ApplicationMessages.InvalidVisitNoteId, bad!.Value);
        }

        [Test]
        public async Task UpdateVisitNote_NullDto_ReturnsBadRequest()
        {
            var result = await _controller.updateVisitNote(1, null);

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(ApplicationMessages.RequestBodyNull, bad!.Value);
        }

        [Test]
        public async Task UpdateVisitNote_NotFound_ReturnsNotFound()
        {
            _mockService
                .Setup(s => s.updateVisitNoteAsync(It.IsAny<int>(), It.IsAny<VisitNoteCreateDto>()))
                .ReturnsAsync((VisitNoteResponseDto?)null);

            var result = await _controller.updateVisitNote(5, GetValidDto());

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion

        #region GET getFilteredVisitNotes

        [Test]
        public async Task GetFilteredVisitNotes_ValidQuery_ReturnsOk()
        {
            _mockService
                .Setup(s => s.getFilteredVisitNotesAsync(It.IsAny<VisitNoteQueryDto>()))
                .ReturnsAsync(new List<VisitNoteResponseDto>());

            var result = await _controller.getFilteredVisitNotes(new VisitNoteQueryDto());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetFilteredVisitNotes_NullQuery_ReturnsOk()
        {
            _mockService
                .Setup(s => s.getFilteredVisitNotesAsync(It.IsAny<VisitNoteQueryDto>()))
                .ReturnsAsync(new List<VisitNoteResponseDto>());

            var result = await _controller.getFilteredVisitNotes(null);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetFilteredVisitNotes_ServiceCalledOnce()
        {
            var query = new VisitNoteQueryDto();

            await _controller.getFilteredVisitNotes(query);

            _mockService.Verify(s => s.getFilteredVisitNotesAsync(query), Times.Once);
        }

        #endregion
    }
}