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
    public class ProgramControllerTests
    {
        private Mock<IProgramService> _mockService;
        private ProgramController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IProgramService>();
            _controller = new ProgramController(_mockService.Object);
            _controller.ModelState.Clear();
        }

        #region ✅ Helper

        private ProgramCreateDTO GetValidCreateDTO()
        {
            return new ProgramCreateDTO
            {
                ProgramName = "Program1",
                Description = "Test Program",
                Status = ProgramStatus.Active
            };
        }

        private ProgramUpdateDTO GetValidUpdateDTO(int id)
        {
            return new ProgramUpdateDTO
            {
                ProgramID = id,
                ProgramName = "Updated",
                Description = "Updated Desc",
                Status = ProgramStatus.Active
            };
        }

        #endregion

        #region POST CreateProgram

        [Test]
        public async Task CreateProgram_Valid_ReturnsOk()
        {
            var dto = GetValidCreateDTO();

            _mockService
                .Setup(s => s.CreateProgramAsync(It.IsAny<ProgramCreateDTO>()))
                .ReturnsAsync(new ProgramResponseDTO { ProgramID = 1 });

            var result = await _controller.CreateProgram(dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateProgram_InvalidModel_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("x", "error");

            var result = await _controller.CreateProgram(GetValidCreateDTO());

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task CreateProgram_ServiceReturnsNull_ReturnsBadRequest()
        {
            _mockService
                .Setup(s => s.CreateProgramAsync(It.IsAny<ProgramCreateDTO>()))
                .ReturnsAsync((ProgramResponseDTO?)null);

            var result = await _controller.CreateProgram(GetValidCreateDTO());

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        #endregion

        #region GET GetAllPrograms

        [Test]
        public async Task GetAllPrograms_Valid_ReturnsOk()
        {
            _mockService
                .Setup(s => s.GetAllProgramsAsync(It.IsAny<ProgramSearchDTO>()))
                .ReturnsAsync(new List<ProgramResponseDTO>());

            var result = await _controller.GetAllPrograms(new ProgramSearchDTO());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllPrograms_InvalidModel_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("x", "error");

            var result = await _controller.GetAllPrograms(new ProgramSearchDTO());

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetAllPrograms_ServiceCalledOnce()
        {
            var query = new ProgramSearchDTO();

            await _controller.GetAllPrograms(query);

            _mockService.Verify(
                s => s.GetAllProgramsAsync(It.IsAny<ProgramSearchDTO>()),
                Times.Once);
        }

        #endregion

        #region GET GetProgramById

        [Test]
        public async Task GetProgramById_Valid_ReturnsOk()
        {
            _mockService
                .Setup(s => s.GetProgramByIdAsync(1))
                .ReturnsAsync(new ProgramResponseDTO());

            var result = await _controller.GetProgramById(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetProgramById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.GetProgramById(0);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetProgramById_NotFound_ReturnsNotFound()
        {
            _mockService
                .Setup(s => s.GetProgramByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((ProgramResponseDTO?)null);

            var result = await _controller.GetProgramById(5);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion

        #region PUT UpdateProgram

        [Test]
        public async Task UpdateProgram_Valid_ReturnsOk()
        {
            var dto = GetValidUpdateDTO(1);

            _mockService
                .Setup(s => s.UpdateProgramAsync(It.IsAny<ProgramUpdateDTO>()))
                .ReturnsAsync(new ProgramResponseDTO());

            var result = await _controller.UpdateProgram(1, dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateProgram_IdMismatch_ReturnsBadRequest()
        {
            var dto = GetValidUpdateDTO(2);

            var result = await _controller.UpdateProgram(1, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateProgram_NotFound_ReturnsNotFound()
        {
            var dto = GetValidUpdateDTO(1);

            _mockService
                .Setup(s => s.UpdateProgramAsync(It.IsAny<ProgramUpdateDTO>()))
                .ReturnsAsync((ProgramResponseDTO?)null);

            var result = await _controller.UpdateProgram(1, dto);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion

        #region DELETE DeleteProgram

        [Test]
        public async Task DeleteProgram_Valid_ReturnsOk()
        {
            _mockService
                .Setup(s => s.GetProgramByIdAsync(1))
                .ReturnsAsync(new ProgramResponseDTO());

            var result = await _controller.DeleteProgram(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteProgram_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.DeleteProgram(0);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task DeleteProgram_NotFound_ReturnsNotFound()
        {
            _mockService
                .Setup(s => s.GetProgramByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((ProgramResponseDTO?)null);

            var result = await _controller.DeleteProgram(5);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion
    }
}