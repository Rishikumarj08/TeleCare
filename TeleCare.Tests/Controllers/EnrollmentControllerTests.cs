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
    public class EnrollmentControllerTests
    {
        private Mock<IEnrollmentService>? _mockService;
        private EnrollmentController? _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IEnrollmentService>();
            _controller = new EnrollmentController(_mockService.Object);
        }

        #region POST

        [Test]
        public async Task CreateEnrollment_Valid_ReturnsOk()
        {
            var dto = new EnrollmentCreateDto
            {
                PatientID = 1,
                ProgramID = 1,
                EnrolledBy = 1,
                ConsentDocumentURI = "file.pdf" // ✅ REQUIRED FIX
            };

            _mockService!
                .Setup(x => x.createEnrollmentRecordAsync(It.IsAny<EnrollmentCreateDto>()))
                .ReturnsAsync(new EnrollmentResponseDto
                {
                    EnrollID = 1,
                    ConsentDocumentURI = "file.pdf"
                });

            var result = await _controller!.createEnrollmentRecord(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
        }

        [Test]
        public async Task CreateEnrollment_Null_ReturnsBadRequest()
        {
            var result = await _controller!.createEnrollmentRecord(null);

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(EnrollmentConstants.RequestBodyNull, bad!.Value);
        }

        [Test]
        public async Task CreateEnrollment_ServiceNull_ReturnsOkNull()
        {
            _mockService!
                .Setup(x => x.createEnrollmentRecordAsync(It.IsAny<EnrollmentCreateDto>()))
                .ReturnsAsync((EnrollmentResponseDto?)null);

            var result = await _controller!.createEnrollmentRecord(new EnrollmentCreateDto
            {
                ConsentDocumentURI = "file.pdf" // ✅ REQUIRED FIX
            });

            var ok = result as OkObjectResult;
            Assert.IsNull(ok!.Value);
        }

        #endregion

        #region GET BY ID

        [Test]
        public async Task GetById_Valid_ReturnsOk()
        {
            _mockService!
                .Setup(x => x.getEnrollmentDetailsByEnrollIDAsync(1))
                .ReturnsAsync(new EnrollmentResponseDto
                {
                    EnrollID = 1,
                    ConsentDocumentURI = "file.pdf"
                });

            var result = await _controller!.getEnrollmentDetailsByEnrollID(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetById_Invalid_ReturnsBadRequest()
        {
            var result = await _controller!.getEnrollmentDetailsByEnrollID(0);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            _mockService!
                .Setup(x => x.getEnrollmentDetailsByEnrollIDAsync(5))
                .ReturnsAsync((EnrollmentResponseDto?)null);

            var result = await _controller!.getEnrollmentDetailsByEnrollID(5);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion

        #region PUT

        [Test]
        public async Task Update_Valid_ReturnsOk()
        {
            var dto = new EnrollmentUpdateDto
            {
                EnrollID = 1,
                ConsentDocumentURI = "file.pdf", // ✅ REQUIRED
                Status = EnrollmentStatus.Active
            };

            _mockService!
                .Setup(x => x.updateEnrollmentDetailsByEnrollIDAsync(It.IsAny<int>(), It.IsAny<EnrollmentUpdateDto>()))
                .ReturnsAsync(new EnrollmentResponseDto
                {
                    EnrollID = 1,
                    ConsentDocumentURI = "file.pdf"
                });

            var result = await _controller!.updateEnrollmentDetailsByEnrollID(1, dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Update_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller!.updateEnrollmentDetailsByEnrollID(0,
                new EnrollmentUpdateDto
                {
                    ConsentDocumentURI = "file.pdf" // ✅ REQUIRED
                });

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            _mockService!
                .Setup(x => x.updateEnrollmentDetailsByEnrollIDAsync(It.IsAny<int>(), It.IsAny<EnrollmentUpdateDto>()))
                .ReturnsAsync((EnrollmentResponseDto?)null);

            var result = await _controller!.updateEnrollmentDetailsByEnrollID(5,
                new EnrollmentUpdateDto
                {
                    ConsentDocumentURI = "file.pdf" // ✅ REQUIRED
                });

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        #endregion

        #region FILTER

        [Test]
        public async Task Filter_ReturnsOk()
        {
            _mockService!
                .Setup(x => x.getFilteredEnrollmentRecordsAsync(It.IsAny<EnrollmentQueryDto>()))
                .ReturnsAsync(new List<EnrollmentResponseDto>());

            var result = await _controller!.getFilteredEnrollmentRecords(new EnrollmentQueryDto());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Filter_NullQuery_ReturnsOk()
        {
            _mockService!
                .Setup(x => x.getFilteredEnrollmentRecordsAsync(It.IsAny<EnrollmentQueryDto>()))
                .ReturnsAsync(new List<EnrollmentResponseDto>());

            var result = await _controller!.getFilteredEnrollmentRecords(null);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Filter_ServiceCalled()
        {
            var query = new EnrollmentQueryDto();

            await _controller!.getFilteredEnrollmentRecords(query);

            _mockService!
                .Verify(x => x.getFilteredEnrollmentRecordsAsync(query), Times.Once);
        }

        #endregion
    }
}
