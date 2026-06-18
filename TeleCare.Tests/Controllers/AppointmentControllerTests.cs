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
    public class AppointmentControllerTests
    {
        private Mock<IAppointmentService>? _mockService;
        private AppointmentController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IAppointmentService>();
            _controller = new AppointmentController(_mockService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region POST createAppointment

        [Test]
        public async Task CreateAppointment_ValidDto_ReturnsOk()
        {
            var dto = new AppointmentCreateDto
            {
                PatientID = 1,
                ClinicianID = 2,
                ScheduledAt = System.DateTime.Now,
                DurationMinutes = 30,
                Mode = "Video",
                Status = "Scheduled"
            };

            var response = new AppointmentResponseDto
            {
                AppID = 1,
                Status = "Scheduled"
            };

            _mockService!
                .Setup(s => s.createAppointmentAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller!.createAppointment(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task CreateAppointment_NullDto_ReturnsBadRequest()
        {
            var result = await _controller!.createAppointment(null);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(ApplicationMessages.RequestBodyNull, bad.Value);
        }

        [Test]
        public async Task CreateAppointment_ServiceReturnsNull_ReturnsOkNull()
        {
            var dto = new AppointmentCreateDto
            {
                PatientID = 1,
                ClinicianID = 2,
                ScheduledAt = System.DateTime.Now,
                DurationMinutes = 30,
                Mode = "Video",
                Status = "Scheduled"
            };

            _mockService!
                .Setup(s => s.createAppointmentAsync(dto))
                .ReturnsAsync((AppointmentResponseDto?)null);

            var result = await _controller!.createAppointment(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.IsNull(ok!.Value);
        }

        #endregion

        #region GET getAllAppointments

        [Test]
        public async Task GetAllAppointments_WhenDataExists_ReturnsOk()
        {
            var data = new List<AppointmentResponseDto>
            {
                new AppointmentResponseDto { AppID = 1, Status = "Scheduled" }
            };

            _mockService!
                .Setup(s => s.getAllAppointmentsAsync())
                .ReturnsAsync(data);

            var result = await _controller!.getAllAppointments();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllAppointments_WhenEmpty_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.getAllAppointmentsAsync())
                .ReturnsAsync(new List<AppointmentResponseDto>());

            var result = await _controller!.getAllAppointments();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllAppointments_ServiceCalledOnce()
        {
            await _controller!.getAllAppointments();

            _mockService!
                .Verify(s => s.getAllAppointmentsAsync(), Times.Once);
        }

        #endregion

        #region GET getAppointmentById

        [Test]
        public async Task GetAppointmentById_ValidId_ReturnsOk()
        {
            int id = 1;

            var response = new AppointmentResponseDto { AppID = id };

            _mockService!
                .Setup(s => s.getAppointmentByIdAsync(id))
                .ReturnsAsync(response);

            var result = await _controller!.getAppointmentById(id);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task GetAppointmentById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller!.getAppointmentById(0);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(ApplicationMessages.InvalidAppointmentId, bad.Value);
        }

        [Test]
        public async Task GetAppointmentById_NotFound_ReturnsNotFound()
        {
            int id = 5;

            _mockService!
                .Setup(s => s.getAppointmentByIdAsync(id))
                .ReturnsAsync((AppointmentResponseDto?)null);

            var result = await _controller!.getAppointmentById(id);

            var notFound = result as NotFoundObjectResult;
            Assert.NotNull(notFound);
            Assert.AreEqual(404, notFound!.StatusCode);
            Assert.AreEqual(ApplicationMessages.AppointmentNotFound, notFound.Value);
        }

        #endregion

        #region PUT updateAppointment

        [Test]
        public async Task UpdateAppointment_ValidInput_ReturnsOk()
        {
            int id = 1;

            var dto = new AppointmentCreateDto
            {
                PatientID = 1,
                ClinicianID = 2,
                ScheduledAt = System.DateTime.Now,
                DurationMinutes = 30,
                Mode = "Phone",
                Status = "Updated"
            };

            var response = new AppointmentResponseDto { AppID = id };

            _mockService!
                .Setup(s => s.updateAppointmentAsync(id, dto))
                .ReturnsAsync(response);

            var result = await _controller!.updateAppointment(id, dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task UpdateAppointment_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller!.updateAppointment(0, new AppointmentCreateDto());

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(ApplicationMessages.InvalidAppointmentId, bad.Value);
        }

        [Test]
        public async Task UpdateAppointment_NotFound_ReturnsNotFound()
        {
            int id = 10;

            var dto = new AppointmentCreateDto();

            _mockService!
                .Setup(s => s.updateAppointmentAsync(id, dto))
                .ReturnsAsync((AppointmentResponseDto?)null);

            var result = await _controller!.updateAppointment(id, dto);

            var notFound = result as NotFoundObjectResult;
            Assert.NotNull(notFound);
            Assert.AreEqual(404, notFound!.StatusCode);
            Assert.AreEqual(ApplicationMessages.AppointmentNotFound, notFound.Value);
        }

        [Test]
        public async Task UpdateAppointment_NullDto_ReturnsBadRequest()
        {
            var result = await _controller!.updateAppointment(1, null);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(ApplicationMessages.RequestBodyNull, bad.Value);
        }

        #endregion

        #region GET getFilteredAppointments

        [Test]
        public async Task GetFilteredAppointments_ValidQuery_ReturnsOk()
        {
            var query = new AppointmentQueryDto();

            var data = new List<AppointmentResponseDto>();

            _mockService!
                .Setup(s => s.getFilteredAppointmentsAsync(query))
                .ReturnsAsync(data);

            var result = await _controller!.getFilteredAppointments(query);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetFilteredAppointments_NullQuery_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.getFilteredAppointmentsAsync(null))
                .ReturnsAsync(new List<AppointmentResponseDto>());

            var result = await _controller!.getFilteredAppointments(null);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetFilteredAppointments_ServiceCalledOnce()
        {
            var query = new AppointmentQueryDto();

            await _controller!.getFilteredAppointments(query);

            _mockService!
                .Verify(s => s.getFilteredAppointmentsAsync(query), Times.Once);
        }

        #endregion
    }
}
