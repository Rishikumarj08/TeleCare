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
using TeleCare.Model; // ✅ IMPORTANT

namespace TeleCare.Tests.Controllers
{
    [TestFixture]
    public class AdminpaymentControllerTests
    {
        private Mock<IPaymentService>? _mockPaymentService;
        private Mock<IPayerRepository>? _mockPayerRepo;
        private AdminPaymentController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _mockPayerRepo = new Mock<IPayerRepository>();

            _controller = new AdminPaymentController(
                _mockPaymentService.Object,
                _mockPayerRepo.Object
            );

            _controller.ModelState.Clear();
        }

        #endregion

        #region GetAllPayments

        [Test]
        public async Task GetAllPayments_WhenDataExists_ReturnsOkWithData()
        {
            var data = new List<PaymentResponseDto>
            {
                new PaymentResponseDto 
                { 
                    PaymentID = 1, 
                    Method = "Card", 
                    Status = "Paid" 
                }
            };

            _mockPaymentService!
                .Setup(s => s.GetAllPaymentsAsync())
                .ReturnsAsync(data);

            var result = await _controller!.GetAllPayments();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllPayments_WhenEmpty_ReturnsOkEmpty()
        {
            _mockPaymentService!
                .Setup(s => s.GetAllPaymentsAsync())
                .ReturnsAsync(new List<PaymentResponseDto>());

            var result = await _controller!.GetAllPayments();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllPayments_ServiceCalledOnce()
        {
            await _controller!.GetAllPayments();

            _mockPaymentService!
                .Verify(s => s.GetAllPaymentsAsync(), Times.Once);
        }

        #endregion

        #region SearchPayments

        [Test]
        public async Task SearchPayments_ValidModel_ReturnsOk()
        {
            var dto = new SearchPaymentDto();

            _mockPaymentService!
                .Setup(s => s.SearchPaymentsAsync(dto))
                .ReturnsAsync(new List<PaymentResponseDto>());

            var result = await _controller!.SearchPayments(dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task SearchPayments_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.SearchPayments(new SearchPaymentDto());

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task SearchPayments_EmptyResult_ReturnsOk()
        {
            var dto = new SearchPaymentDto();

            _mockPaymentService!
                .Setup(s => s.SearchPaymentsAsync(dto))
                .ReturnsAsync(new List<PaymentResponseDto>());

            var result = await _controller!.SearchPayments(dto);

            Assert.NotNull(result);
        }

        #endregion

        #region CreatePayment

        [Test]
        public async Task CreatePayment_ValidInput_ReturnsOk()
        {
            var dto = new PaymentCreateDto
            {
                Method = "Card",
                Status = "Paid"
            };

            var result = await _controller!.CreatePayment(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordCreated, ok.Value);

            _mockPaymentService!
                .Verify(s => s.CreatePaymentAsync(dto), Times.Once);
        }

        [Test]
        public async Task CreatePayment_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.CreatePayment(new PaymentCreateDto
            {
                Method = "Card",
                Status = "Paid"
            });

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task CreatePayment_ServiceCalledOnce()
        {
            var dto = new PaymentCreateDto
            {
                Method = "UPI",
                Status = "Pending"
            };

            await _controller!.CreatePayment(dto);

            _mockPaymentService!
                .Verify(s => s.CreatePaymentAsync(dto), Times.Once);
        }

        #endregion

        #region UpdatePayment

        [Test]
        public async Task UpdatePayment_ValidInput_ReturnsOk()
        {
            var dto = new PaymentCreateDto
            {
                Method = "Cash",
                Status = "Paid"
            };

            var result = await _controller!.UpdatePayment(1, dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordUpdated, ok.Value);

            _mockPaymentService!
                .Verify(s => s.UpdatePaymentAsync(1, dto), Times.Once);
        }

        [Test]
        public async Task UpdatePayment_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.UpdatePayment(1, new PaymentCreateDto
            {
                Method = "Card",
                Status = "Paid"
            });

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task UpdatePayment_ServiceCalledOnce()
        {
            var dto = new PaymentCreateDto
            {
                Method = "Card",
                Status = "Pending"
            };

            await _controller!.UpdatePayment(5, dto);

            _mockPaymentService!
                .Verify(s => s.UpdatePaymentAsync(5, dto), Times.Once);
        }

        #endregion

        #region DeletePayment

        [Test]
        public async Task DeletePayment_ValidId_ReturnsOk()
        {
            var result = await _controller!.DeletePayment(1);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordDeleted, ok.Value);
        }

        [Test]
        public async Task DeletePayment_ServiceCalledOnce()
        {
            await _controller!.DeletePayment(10);

            _mockPaymentService!
                .Verify(s => s.DeletePaymentAsync(10), Times.Once);
        }

        [Test]
        public async Task DeletePayment_ZeroId_ReturnsOk()
        {
            var result = await _controller!.DeletePayment(0);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        #endregion

        #region GetPayers ✅ (CRITICAL FIX)

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
