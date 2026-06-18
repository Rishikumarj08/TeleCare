using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeleCare.Controllers;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Repository.Interface;
using TeleCare.Constants;
using TeleCare.Model; // ✅ IMPORTANT (REAL Payer)

namespace TeleCare.Tests.Controllers
{
    [TestFixture]
    public class AdminClaimControllerTests
    {
        private Mock<IClaimService>? _mockClaimService;
        private Mock<IPayerRepository>? _mockPayerRepo;
        private AdminClaimController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockClaimService = new Mock<IClaimService>();
            _mockPayerRepo = new Mock<IPayerRepository>();

            _controller = new AdminClaimController(
                _mockClaimService.Object,
                _mockPayerRepo.Object
            );

            _controller.ModelState.Clear();
        }

        #endregion

        #region GetAllClaims

        [Test]
        public async Task GetAllClaims_WhenDataExists_ReturnsOk()
        {
            var data = new List<ClaimResponseDto>
            {
                new ClaimResponseDto { ClaimID = 1, Status = "Approved" }
            };

            _mockClaimService!
                .Setup(s => s.GetAllClaimsAsync())
                .ReturnsAsync(data);

            var result = await _controller!.GetAllClaims();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllClaims_Empty_ReturnsOk()
        {
            _mockClaimService!
                .Setup(s => s.GetAllClaimsAsync())
                .ReturnsAsync(new List<ClaimResponseDto>());

            var result = await _controller!.GetAllClaims();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllClaims_ServiceCalled()
        {
            await _controller!.GetAllClaims();

            _mockClaimService!
                .Verify(x => x.GetAllClaimsAsync(), Times.Once);
        }

        #endregion

        #region SearchClaims

        [Test]
        public async Task SearchClaims_Valid_ReturnsOk()
        {
            var dto = new SearchClaimDto();

            _mockClaimService!
                .Setup(s => s.SearchClaimsAsync(dto))
                .ReturnsAsync(new List<ClaimResponseDto>());

            var result = await _controller!.SearchClaims(dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task SearchClaims_InvalidModel_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("x", "error");

            var result = await _controller.SearchClaims(new SearchClaimDto());

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        #endregion

        #region CreateClaim

        [Test]
        public async Task CreateClaim_Valid_ReturnsOk()
        {
            var dto = new ClaimCreateDto { Status = "Pending" };

            var result = await _controller!.CreateClaim(dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordCreated, ok.Value);
        }

        [Test]
        public async Task CreateClaim_InvalidModel_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("x", "error");

            var result = await _controller.CreateClaim(
                new ClaimCreateDto { Status = "Pending" });

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        #endregion

        #region UpdateClaim

        [Test]
        public async Task UpdateClaim_Valid_ReturnsOk()
        {
            var dto = new ClaimCreateDto { Status = "Approved" };

            var result = await _controller!.UpdateClaim(1, dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordUpdated, ok.Value);
        }

        #endregion

        #region DeleteClaim

        [Test]
        public async Task DeleteClaim_Valid_ReturnsOk()
        {
            var result = await _controller!.DeleteClaim(1);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordDeleted, ok.Value);
        }

        #endregion

        #region GetPayers ✅ FIXED

        [Test]
        public async Task GetPayers_WhenDataExists_ReturnsOk()
        {
            var payers = new List<Payer>
            {
                new Payer { PayerID = 1, PayerName = "Aetna" }
            };

            _mockPayerRepo!
                .Setup(p => p.GetAllPayersAsync())
                .ReturnsAsync(payers);

            var result = await _controller!.GetPayers();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);

            var data = ok.Value as IEnumerable<object>;
            Assert.NotNull(data);
            Assert.IsNotEmpty(data!.ToList());
        }

        [Test]
        public async Task GetPayers_WhenEmpty_ReturnsOkEmpty()
        {
            _mockPayerRepo!
                .Setup(p => p.GetAllPayersAsync())
                .ReturnsAsync(new List<Payer>());

            var result = await _controller!.GetPayers();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetPayers_ServiceCalledOnce()
        {
            _mockPayerRepo!
                .Setup(p => p.GetAllPayersAsync())
                .ReturnsAsync(new List<Payer>());

            await _controller!.GetPayers();

            _mockPayerRepo.Verify(p => p.GetAllPayersAsync(), Times.Once);
        }

        #endregion
    }
}
