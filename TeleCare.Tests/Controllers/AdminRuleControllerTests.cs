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
    public class AdminRuleControllerTests
    {
        private Mock<IRuleService>? _mockRuleService;
        private AdminRuleController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockRuleService = new Mock<IRuleService>();
            _controller = new AdminRuleController(_mockRuleService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region GetAllRules

        [Test]
        public async Task GetAllRules_WhenDataExists_ReturnsOkWithData()
        {
            var data = new List<RuleResponseDto>
            {
                new RuleResponseDto { RuleID = 1, Name = "Rule1", Status = "Active" }
            };

            _mockRuleService!
                .Setup(s => s.GetAllRulesAsync())
                .ReturnsAsync(data);

            var result = await _controller!.GetAllRules();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllRules_WhenEmptyList_ReturnsOkEmpty()
        {
            _mockRuleService!
                .Setup(s => s.GetAllRulesAsync())
                .ReturnsAsync(new List<RuleResponseDto>());

            var result = await _controller!.GetAllRules();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllRules_ServiceCalledOnce_Verified()
        {
            await _controller!.GetAllRules();

            _mockRuleService!
                .Verify(s => s.GetAllRulesAsync(), Times.Once);
        }

        #endregion

        #region SearchRules

        [Test]
        public async Task SearchRules_ValidModel_ReturnsOkWithResults()
        {
            var dto = new SearchRuleDto();

            _mockRuleService!
                .Setup(s => s.SearchRulesAsync(dto))
                .ReturnsAsync(new List<RuleResponseDto>());

            var result = await _controller!.SearchRules(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task SearchRules_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.SearchRules(new SearchRuleDto());

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task SearchRules_EmptyResult_ReturnsOkEmpty()
        {
            var dto = new SearchRuleDto();

            _mockRuleService!
                .Setup(s => s.SearchRulesAsync(dto))
                .ReturnsAsync(new List<RuleResponseDto>());

            var result = await _controller!.SearchRules(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
        }

        #endregion

        #region CreateRule

        [Test]
        public async Task CreateRule_ValidInput_ReturnsOkWithCreatedMessage()
        {
            var dto = new RuleCreateDto
            {
                Name = "Rule1",
                Status = "Active"
            };

            var result = await _controller!.CreateRule(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordCreated, ok.Value);

            _mockRuleService!
                .Verify(s => s.CreateRuleAsync(dto), Times.Once);
        }

        [Test]
        public async Task CreateRule_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.CreateRule(new RuleCreateDto
            {
                Name = "Rule1",
                Status = "Active"
            });

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task CreateRule_ServiceInvocation_Verified()
        {
            var dto = new RuleCreateDto
            {
                Name = "Rule2",
                Status = "Inactive"
            };

            await _controller!.CreateRule(dto);

            _mockRuleService!
                .Verify(s => s.CreateRuleAsync(dto), Times.Once);
        }

        #endregion

        #region UpdateRule

        [Test]
        public async Task UpdateRule_ValidInput_ReturnsOkWithUpdatedMessage()
        {
            var dto = new RuleCreateDto
            {
                Name = "Rule1",
                Status = "Active"
            };

            var result = await _controller!.UpdateRule(1, dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordUpdated, ok.Value);

            _mockRuleService!
                .Verify(s => s.UpdateRuleAsync(1, dto), Times.Once);
        }

        [Test]
        public async Task UpdateRule_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.UpdateRule(1, new RuleCreateDto
            {
                Name = "Rule1",
                Status = "Active"
            });

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task UpdateRule_ServiceInvocation_Verified()
        {
            var dto = new RuleCreateDto
            {
                Name = "Rule2",
                Status = "Inactive"
            };

            await _controller!.UpdateRule(5, dto);

            _mockRuleService!
                .Verify(s => s.UpdateRuleAsync(5, dto), Times.Once);
        }

        #endregion

        #region DeleteRule

        [Test]
        public async Task DeleteRule_ValidId_ReturnsOkWithDeletedMessage()
        {
            var result = await _controller!.DeleteRule(1);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordDeleted, ok.Value);
        }

        [Test]
        public async Task DeleteRule_ServiceCalledOnce()
        {
            await _controller!.DeleteRule(10);

            _mockRuleService!
                .Verify(s => s.DeleteRuleAsync(10), Times.Once);
        }

        [Test]
        public async Task DeleteRule_ZeroId_ReturnsOk()
        {
            var result = await _controller!.DeleteRule(0);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        #endregion
    }
}