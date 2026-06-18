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
    public class PatientControllerTests
    {
        private Mock<IPatientService> _mockService;
        private PatientController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IPatientService>();
            _controller = new PatientController(_mockService.Object);
        }

        #region ✅ Helper (VERY IMPORTANT)

        private PatientCreateDto GetValidCreateDto()
        {
            return new PatientCreateDto
            {
                UserID = 1,
                MRN = "MRN123",
                Name = "John",
                DOB = System.DateTime.Now,
                Gender = "Male",
                Address = "Address",
                ContactInfoJSON = "{}",
                EmergencyContactJSON = "{}",
                PrimaryLanguage = "English",
                ConsentStatus = true,
                EnrolledProgramsJSON = "{}"
            };
        }

        private PatientUpdateDto GetValidUpdateDto()
        {
            return new PatientUpdateDto
            {
                PatientID = 1,
                Name = "Updated",
                Address = "New Address",
                ContactInfoJSON = "{}",
                EmergencyContactJSON = "{}"
            };
        }

        #endregion

        #region POST createPatientRecord

        [Test]
        public async Task CreatePatient_Valid_ReturnsOk()
        {
            var dto = GetValidCreateDto();

            _mockService
                .Setup(x => x.createPatientRecordAsync(It.IsAny<PatientCreateDto>()))
                .ReturnsAsync(new PatientResponseDto
                {
                    PatientID = 1,
                    MRN = "MRN123",
                    Name = "John",
                    Gender = "Male",
                    Address = "Addr",
                    ContactInfoJSON = "{}",
                    EmergencyContactJSON = "{}",
                    PrimaryLanguage = "EN",
                    EnrolledProgramsJSON = "{}"
                });

            var result = await _controller.createPatientRecord(dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreatePatient_Null_ReturnsBadRequest()
        {
            var result = await _controller.createPatientRecord(null);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task CreatePatient_ServiceReturnsNull_ReturnsOkNull()
        {
            _mockService
                .Setup(x => x.createPatientRecordAsync(It.IsAny<PatientCreateDto>()))
                .ReturnsAsync((PatientResponseDto?)null);

            var result = await _controller.createPatientRecord(GetValidCreateDto());

            var ok = result as OkObjectResult;
            Assert.IsNull(ok!.Value);
        }

        #endregion

        #region GET getPatientDetailsByPatientId

        [Test]
        public async Task GetPatient_ValidId_ReturnsOk()
        {
            _mockService
                .Setup(x => x.getPatientDetailsByPatientIdAsync(1))
                .ReturnsAsync(new PatientResponseDto
                {
                    PatientID = 1,
                    MRN = "MRN",
                    Name = "Name",
                    Gender = "G",
                    Address = "A",
                    ContactInfoJSON = "{}",
                    EmergencyContactJSON = "{}",
                    PrimaryLanguage = "EN",
                    EnrolledProgramsJSON = "{}"
                });

            var result = await _controller.getPatientDetailsByPatientId(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetPatient_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.getPatientDetailsByPatientId(0);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetPatient_NotFound_ReturnsNotFound()
        {
            _mockService
                .Setup(x => x.getPatientDetailsByPatientIdAsync(It.IsAny<int>()))
                .ReturnsAsync((PatientResponseDto?)null);

            var result = await _controller.getPatientDetailsByPatientId(5);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion

        #region PUT updatePatientDetailsByPatientId

        [Test]
        public async Task UpdatePatient_Valid_ReturnsOk()
        {
            _mockService
                .Setup(x => x.updatePatientDetailsByPatientIdAsync(
                    It.IsAny<int>(), It.IsAny<PatientUpdateDto>()))
                .ReturnsAsync(new PatientResponseDto
                {
                    PatientID = 1,
                    MRN = "MRN",
                    Name = "Updated",
                    Gender = "G",
                    Address = "A",
                    ContactInfoJSON = "{}",
                    EmergencyContactJSON = "{}",
                    PrimaryLanguage = "EN",
                    EnrolledProgramsJSON = "{}"
                });

            var result = await _controller.updatePatientDetailsByPatientId(
                1, GetValidUpdateDto());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdatePatient_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller.updatePatientDetailsByPatientId(
                0, GetValidUpdateDto());

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdatePatient_NotFound_ReturnsNotFound()
        {
            _mockService
                .Setup(x => x.updatePatientDetailsByPatientIdAsync(
                    It.IsAny<int>(), It.IsAny<PatientUpdateDto>()))
                .ReturnsAsync((PatientResponseDto?)null);

            var result = await _controller.updatePatientDetailsByPatientId(
                5, GetValidUpdateDto());

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion

        #region GET filter

        [Test]
        public async Task FilterPatients_ReturnsOk()
        {
            _mockService
                .Setup(x => x.getFilteredPatientRecordsAsync(It.IsAny<PatientQueryDto>()))
                .ReturnsAsync(new List<PatientResponseDto>());

            var result = await _controller.getFilteredPatientRecords(new PatientQueryDto());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task FilterPatients_NullQuery_ReturnsOk()
        {
            _mockService
                .Setup(x => x.getFilteredPatientRecordsAsync(It.IsAny<PatientQueryDto>()))
                .ReturnsAsync(new List<PatientResponseDto>());

            var result = await _controller.getFilteredPatientRecords(null);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task FilterPatients_ServiceCalledOnce()
        {
            var query = new PatientQueryDto();

            await _controller.getFilteredPatientRecords(query);

            _mockService.Verify(
                x => x.getFilteredPatientRecordsAsync(query),
                Times.Once);
        }

        #endregion
    }
}