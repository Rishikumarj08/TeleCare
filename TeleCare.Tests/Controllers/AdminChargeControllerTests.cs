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
    public class AdminChargeControllerTests
    {
        private Mock<IChargeService>? _mockService;
        private AdminChargeController? _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IChargeService>();
            _controller = new AdminChargeController(_mockService.Object);
            _controller.ModelState.Clear();
        }

        #region GetAllCharges

        [Test]
        public async Task GetAllCharges_Valid_ReturnsOk()
        {
            var data = new List<ChargeResponseDto>
            {
                new ChargeResponseDto { ChargeID = 1, Status = "Paid" }
            };

            _mockService!
                .Setup(s => s.GetAllChargesAsync())
                .ReturnsAsync(data);

            var result = await _controller!.GetAllCharges();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllCharges_Empty_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.GetAllChargesAsync())
                .ReturnsAsync(new List<ChargeResponseDto>());

            var result = await _controller!.GetAllCharges();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllCharges_ServiceReturnsNull_ReturnsOkNull()
        {
            _mockService!
                .Setup(s => s.GetAllChargesAsync())
                .ReturnsAsync(new List<ChargeResponseDto>());

            var result = await _controller!.GetAllCharges();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
        }

        #endregion

        #region SearchCharges

        [Test]
        public async Task SearchCharges_Valid_ReturnsOk()
        {
            var dto = new SearchChargeDto();

            _mockService!
                .Setup(s => s.SearchChargesAsync(dto))
                .ReturnsAsync(new List<ChargeResponseDto>());

            var result = await _controller!.SearchCharges(dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task SearchCharges_InvalidModel_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("x", "error");

            var result = await _controller.SearchCharges(new SearchChargeDto());

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task SearchCharges_EmptyResult_ReturnsOk()
        {
            var dto = new SearchChargeDto();

            _mockService!
                .Setup(s => s.SearchChargesAsync(dto))
                .ReturnsAsync(new List<ChargeResponseDto>());

            var result = await _controller!.SearchCharges(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
        }

        #endregion

        #region CreateCharge

        [Test]
        public async Task CreateCharge_Valid_ReturnsOk()
        {
            var dto = new ChargeCreateDto { Status = "Pending" };

            var result = await _controller!.CreateCharge(dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordCreated, ok.Value);

            _mockService!.Verify(s => s.CreateChargeAsync(dto), Times.Once);
        }

        [Test]
        public async Task CreateCharge_InvalidModel_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("x", "error");

            var result = await _controller.CreateCharge(new ChargeCreateDto { Status = "Pending" });

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task CreateCharge_VerifyServiceCall()
        {
            var dto = new ChargeCreateDto { Status = "Paid" };

            await _controller!.CreateCharge(dto);

            _mockService!.Verify(s => s.CreateChargeAsync(dto), Times.Once);
        }

        #endregion

        #region UpdateCharge

        [Test]
        public async Task UpdateCharge_Valid_ReturnsOk()
        {
            var dto = new ChargeCreateDto { Status = "Paid" };

            var result = await _controller!.UpdateCharge(1, dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordUpdated, ok.Value);
        }

        [Test]
        public async Task UpdateCharge_InvalidModel_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("x", "error");

            var result = await _controller.UpdateCharge(1, new ChargeCreateDto { Status = "Pending" });

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task UpdateCharge_VerifyServiceCall()
        {
            var dto = new ChargeCreateDto { Status = "Pending" };

            await _controller!.UpdateCharge(5, dto);

            _mockService!.Verify(s => s.UpdateChargeAsync(5, dto), Times.Once);
        }

        #endregion

        #region DeleteCharge

        [Test]
        public async Task DeleteCharge_Valid_ReturnsOk()
        {
            var result = await _controller!.DeleteCharge(1);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordDeleted, ok.Value);
        }

        [Test]
        public async Task DeleteCharge_VerifyServiceCall()
        {
            await _controller!.DeleteCharge(10);

            _mockService!.Verify(s => s.DeleteChargeAsync(10), Times.Once);
        }

        [Test]
        public async Task DeleteCharge_ZeroId_ReturnsOk()
        {
            var result = await _controller!.DeleteCharge(0);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        #endregion
    }
}