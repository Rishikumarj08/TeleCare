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
    public class AuditorAuditLogControllerTests
    {
        private Mock<IAuditLogService>? _mockService;
        private AuditorAuditLogController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IAuditLogService>();
            _controller = new AuditorAuditLogController(_mockService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region GET GetAllAuditLogs

        [Test]
        public async Task GetAllAuditLogs_WhenDataExists_ReturnsOkWithData()
        {
            // Arrange
            var logs = new List<AuditLogResponseDto>
            {
                new AuditLogResponseDto
                {
                    Action = "CREATE",
                    ResourceType = "User",
                    PerformedBy = "Admin"
                }
            };

            _mockService!
                .Setup(s => s.GetAllAuditLogsAsync())
                .ReturnsAsync(logs);

            // Act
            var result = await _controller!.GetAllAuditLogs();

            // Assert
            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(logs, ok.Value);
        }

        [Test]
        public async Task GetAllAuditLogs_WhenEmpty_ReturnsOkEmptyList()
        {
            // Arrange
            _mockService!
                .Setup(s => s.GetAllAuditLogsAsync())
                .ReturnsAsync(new List<AuditLogResponseDto>());

            // Act
            var result = await _controller!.GetAllAuditLogs();

            // Assert
            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllAuditLogs_ServiceCalledOnce_Verified()
        {
            // Act
            await _controller!.GetAllAuditLogs();

            // Assert
            _mockService!
                .Verify(s => s.GetAllAuditLogsAsync(), Times.Once);
        }

        #endregion

        #region POST SearchAuditLogs

        [Test]
        public async Task SearchAuditLogs_ValidModel_ReturnsOkWithResults()
        {
            // Arrange
            var dto = new SearchAuditLogDto();

            var logs = new List<AuditLogResponseDto>();

            _mockService!
                .Setup(s => s.SearchAuditLogsAsync(dto))
                .ReturnsAsync(logs);

            // Act
            var result = await _controller!.SearchAuditLogs(dto);

            // Assert
            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(logs, ok.Value);
        }

        [Test]
        public async Task SearchAuditLogs_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller!.ModelState.AddModelError("Error", "Invalid");

            // Act
            var result = await _controller.SearchAuditLogs(new SearchAuditLogDto());

            // Assert
            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task SearchAuditLogs_EmptyResult_ReturnsOkEmpty()
        {
            // Arrange
            var dto = new SearchAuditLogDto();

            _mockService!
                .Setup(s => s.SearchAuditLogsAsync(dto))
                .ReturnsAsync(new List<AuditLogResponseDto>());

            // Act
            var result = await _controller!.SearchAuditLogs(dto);

            // Assert
            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
        }

        #endregion
    }
}
