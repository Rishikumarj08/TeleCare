using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeleCare.Controllers;
using TeleCare.DTO;
using TeleCare.Service.Interface;

namespace TeleCare.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService>? _mockAuthService;
        private AuthController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region Register

        [Test]
        public async Task Register_ValidModel_ReturnsOkMessage()
        {
            var dto = new AuthRegisterDto
            {
                Name = "John",
                Email = "john@test.com",
                Phone = "123",
                Password = "pass",
                RoleName = "Patient"
            };

            var result = await _controller!.Register(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual("Registration completed successfully.", ok.Value);

            _mockAuthService!
                .Verify(s => s.RegisterAsync(dto), Times.Once);
        }

        [Test]
        public async Task Register_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.Register(new AuthRegisterDto
            {
                Name = "",
                Email = "",
                Phone = "",
                Password = "",
                RoleName = ""
            });

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task Register_ServiceCalledOnce_Verified()
        {
            var dto = new AuthRegisterDto
            {
                Name = "Jane",
                Email = "jane@test.com",
                Phone = "456",
                Password = "pass",
                RoleName = "Clinician"
            };

            await _controller!.Register(dto);

            _mockAuthService!
                .Verify(s => s.RegisterAsync(dto), Times.Once);
        }

        #endregion

        #region Login

        [Test]
        public async Task Login_ValidModel_ReturnsOkWithResponse()
        {
            var dto = new AuthLoginDto
            {
                Email = "test@test.com",
                Password = "pass"
            };

            var response = new AuthResponseDto
            {
                Success = true,
                Token = "jwt-token"
            };

            _mockAuthService!
                .Setup(s => s.LoginAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller!.Login(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task Login_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.Login(new AuthLoginDto
            {
                Email = "",
                Password = ""
            });

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task Login_ServiceReturnsNull_ReturnsOkNull()
        {
            var dto = new AuthLoginDto
            {
                Email = "test@test.com",
                Password = "pass"
            };

            _mockAuthService!
                .Setup(s => s.LoginAsync(dto))
                .ReturnsAsync((AuthResponseDto?)null);

            var result = await _controller!.Login(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.IsNull(ok!.Value);
        }

        #endregion

        #region VerifyPin

        [Test]
        public async Task VerifyPin_ValidModel_ReturnsOk()
        {
            var dto = new AuthPinVerificationDto
            {
                Email = "test@test.com",
                Pin = "1234"
            };

            var response = new AuthResponseDto
            {
                Success = true
            };

            _mockAuthService!
                .Setup(s => s.VerifyClinicianPinAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller!.VerifyPin(dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task VerifyPin_InvalidModel_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.VerifyPin(new AuthPinVerificationDto
            {
                Email = "",
                Pin = ""
            });

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task VerifyPin_ServiceCalledOnce()
        {
            var dto = new AuthPinVerificationDto
            {
                Email = "test@test.com",
                Pin = "1234"
            };

            await _controller!.VerifyPin(dto);

            _mockAuthService!
                .Verify(s => s.VerifyClinicianPinAsync(dto), Times.Once);
        }

        #endregion

        #region ForgotPassword

        [Test]
        public async Task ForgotPassword_ValidModel_ReturnsOkMessage()
        {
            var dto = new ForgotPasswordRequestDto
            {
                Email = "test@test.com"
            };

            var result = await _controller!.ForgotPassword(dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual("Password reset instructions have been generated for the patient account.", ok.Value);
        }

        [Test]
        public async Task ForgotPassword_InvalidModel_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.ForgotPassword(new ForgotPasswordRequestDto
            {
                Email = ""
            });

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task ForgotPassword_ServiceCalledOnce()
        {
            var dto = new ForgotPasswordRequestDto
            {
                Email = "test@test.com"
            };

            await _controller!.ForgotPassword(dto);

            _mockAuthService!
                .Verify(s => s.ForgotPasswordAsync(dto), Times.Once);
        }

        #endregion

        #region ResetPassword

        [Test]
        public async Task ResetPassword_ValidModel_ReturnsOkMessage()
        {
            var dto = new ResetPasswordDto
            {
                Email = "test@test.com",
                Token = "token",
                NewPassword = "newpass"
            };

            var result = await _controller!.ResetPassword(dto);

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual("Password has been reset successfully.", ok.Value);
        }

        [Test]
        public async Task ResetPassword_InvalidModel_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.ResetPassword(new ResetPasswordDto
            {
                Email = "",
                Token = "",
                NewPassword = ""
            });

            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task ResetPassword_ServiceCalledOnce()
        {
            var dto = new ResetPasswordDto
            {
                Email = "test@test.com",
                Token = "token",
                NewPassword = "newpass"
            };

            await _controller!.ResetPassword(dto);

            _mockAuthService!
                .Verify(s => s.ResetPasswordAsync(dto), Times.Once);
        }

        #endregion
    }
}
