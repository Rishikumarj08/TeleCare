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
    public class AuditorKpiControllerTests
    {
        private Mock<IKpiService>? _mockKpiService;
        private AuditorKpiController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockKpiService = new Mock<IKpiService>();
            _controller = new AuditorKpiController(_mockKpiService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region GetAllKpis

        [Test]
        public async Task GetAllKpis_WhenDataExists_ReturnsOkWithData()
        {
            var data = new List<KpiResponseDto>
            {
                new KpiResponseDto
                {
                    Name = "Efficiency",
                    ReportingPeriod = "Monthly",
                    PerformanceIndicator = "Good"
                }
            };

            _mockKpiService!
                .Setup(s => s.GetAllKpisAsync())
                .ReturnsAsync(data);

            var result = await _controller!.GetAllKpis();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllKpis_WhenEmpty_ReturnsOkEmptyList()
        {
            _mockKpiService!
                .Setup(s => s.GetAllKpisAsync())
                .ReturnsAsync(new List<KpiResponseDto>());

            var result = await _controller!.GetAllKpis();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllKpis_ServiceCalledOnce_Verified()
        {
            await _controller!.GetAllKpis();

            _mockKpiService!
                .Verify(s => s.GetAllKpisAsync(), Times.Once);
        }

        #endregion

        #region SearchKpis

        [Test]
        public async Task SearchKpis_ValidModel_ReturnsOkWithResults()
        {
            var dto = new SearchKpiDto();

            var data = new List<KpiResponseDto>();

            _mockKpiService!
                .Setup(s => s.SearchKpisAsync(dto))
                .ReturnsAsync(data);

            var result = await _controller!.SearchKpis(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task SearchKpis_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.SearchKpis(new SearchKpiDto());

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task SearchKpis_EmptyResult_ReturnsOk()
        {
            var dto = new SearchKpiDto();

            _mockKpiService!
                .Setup(s => s.SearchKpisAsync(dto))
                .ReturnsAsync(new List<KpiResponseDto>());

            var result = await _controller!.SearchKpis(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
        }

        #endregion

        #region CreateKpi

        [Test]
        public async Task CreateKpi_ValidInput_ReturnsOkWithCreatedMessage()
        {
            var dto = new KpiCreateDto
            {
                Name = "Efficiency",
                ReportingPeriod = "Monthly"
            };

            var result = await _controller!.CreateKpi(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordCreated, ok.Value);

            _mockKpiService!
                .Verify(s => s.CreateKpiAsync(dto), Times.Once);
        }

        [Test]
        public async Task CreateKpi_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.CreateKpi(new KpiCreateDto
            {
                Name = "Efficiency",
                ReportingPeriod = "Monthly"
            });

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task CreateKpi_ServiceCalledOnce_Verified()
        {
            var dto = new KpiCreateDto
            {
                Name = "Quality",
                ReportingPeriod = "Weekly"
            };

            await _controller!.CreateKpi(dto);

            _mockKpiService!
                .Verify(s => s.CreateKpiAsync(dto), Times.Once);
        }

        #endregion

        #region UpdateKpi

        [Test]
        public async Task UpdateKpi_ValidInput_ReturnsOkWithUpdatedMessage()
        {
            var dto = new KpiCreateDto
            {
                Name = "Efficiency",
                ReportingPeriod = "Monthly"
            };

            var result = await _controller!.UpdateKpi(1, dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordUpdated, ok.Value);

            _mockKpiService!
                .Verify(s => s.UpdateKpiAsync(1, dto), Times.Once);
        }

        [Test]
        public async Task UpdateKpi_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.UpdateKpi(1, new KpiCreateDto
            {
                Name = "Efficiency",
                ReportingPeriod = "Monthly"
            });

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task UpdateKpi_ServiceCalledOnce_Verified()
        {
            var dto = new KpiCreateDto
            {
                Name = "Quality",
                ReportingPeriod = "Weekly"
            };

            await _controller!.UpdateKpi(5, dto);

            _mockKpiService!
                .Verify(s => s.UpdateKpiAsync(5, dto), Times.Once);
        }

        #endregion

        #region DeleteKpi

        [Test]
        public async Task DeleteKpi_ValidId_ReturnsOkWithDeletedMessage()
        {
            var result = await _controller!.DeleteKpi(1);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordDeleted, ok.Value);
        }

        [Test]
        public async Task DeleteKpi_ServiceCalledOnce()
        {
            await _controller!.DeleteKpi(10);

            _mockKpiService!
                .Verify(s => s.DeleteKpiAsync(10), Times.Once);
        }

        [Test]
        public async Task DeleteKpi_ZeroId_ReturnsOk()
        {
            var result = await _controller!.DeleteKpi(0);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        #endregion
    }
}