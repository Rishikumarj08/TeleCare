using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeleCare.Controllers;
using TeleCare.Dto;
using TeleCare.Service.Interface;
using TeleCare.Constants;
using TeleCare.Enums;

namespace TeleCare.Tests.Controllers
{
    [TestFixture]
    public class MedicationControllerTests
    {
        private Mock<IMedicationService> _mockService;
        private MedicationController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IMedicationService>();
            _controller = new MedicationController(_mockService.Object);
        }

        #region ✅ HELPER METHOD (IMPORTANT)

        private MedicationRequestDto GetValidDto()
        {
            return new MedicationRequestDto
            {
                Name = "Paracetamol",
                Dose = "500mg",
                Frequency = "Twice Daily",
                Route = "Oral",
                StartAt = System.DateTime.Now
            };
        }

        #endregion

        #region GET ALL

        [Test]
        public async Task GetAllMedications_Valid_ReturnsOk()
        {
            _mockService
                .Setup(x => x.GetAllMedicationsAsync(It.IsAny<MedicationSearchDto>()))
                .ReturnsAsync(new List<MedicationResponseDto>());

            var result = await _controller.GetAllMedications(new MedicationSearchDto());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllMedications_InvalidModel_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("x", "error");

            var result = await _controller.GetAllMedications(new MedicationSearchDto());

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        #endregion

        #region GET BY ID

        [Test]
        public async Task GetById_Valid_ReturnsOk()
        {
            _mockService
                .Setup(x => x.GetMedicationByIdAsync(1))
                .ReturnsAsync(new MedicationResponseDto());

            var result = await _controller.GetMedicationById(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetById_Invalid_ReturnsBadRequest()
        {
            var result = await _controller.GetMedicationById(0);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            _mockService
                .Setup(x => x.GetMedicationByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((MedicationResponseDto)null);

            var result = await _controller.GetMedicationById(10);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion

        #region CREATE

        [Test]
        public async Task Create_Valid_ReturnsOk()
        {
            var dto = GetValidDto();

            _mockService
                .Setup(x => x.CreateMedicationAsync(It.IsAny<int>(), It.IsAny<MedicationRequestDto>()))
                .ReturnsAsync(new MedicationResponseDto());

            var result = await _controller.CreateMedication(1, dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_InvalidPatientId_ReturnsBadRequest()
        {
            var result = await _controller.CreateMedication(0, GetValidDto());

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_NullDto_ReturnsBadRequest()
        {
            var result = await _controller.CreateMedication(1, null);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        #endregion

        #region UPDATE

        [Test]
        public async Task Update_Valid_ReturnsOk()
        {
            var dto = GetValidDto();

            _mockService
                .Setup(x => x.UpdateMedicationAsync(It.IsAny<int>(), It.IsAny<MedicationRequestDto>()))
                .ReturnsAsync(new MedicationResponseDto());

            var result = await _controller.UpdateMedication(1, dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Update_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.UpdateMedication(0, GetValidDto());

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            _mockService
                .Setup(x => x.UpdateMedicationAsync(It.IsAny<int>(), It.IsAny<MedicationRequestDto>()))
                .ReturnsAsync((MedicationResponseDto)null);

            var result = await _controller.UpdateMedication(10, GetValidDto());

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion
    }
}