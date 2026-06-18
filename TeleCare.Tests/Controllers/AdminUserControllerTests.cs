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
    public class AdminUserControllerTests
    {
        private Mock<IUserService>? _mockUserService;
        private AdminUserController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new AdminUserController(_mockUserService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region GetAllUsers

        [Test]
        public async Task GetAllUsers_WhenDataExists_ReturnsOkWithUserList()
        {
            var users = new List<UserResponseDto>
            {
                new UserResponseDto
                {
                    UserID = 1,
                    Name = "John",
                    Email = "john@test.com",
                    Phone = "123",
                    RoleName = "Admin"
                }
            };

            _mockUserService!
                .Setup(s => s.GetAllUsersAsync())
                .ReturnsAsync(users);

            var result = await _controller!.GetAllUsers();

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(users, ok.Value);
        }

        [Test]
        public async Task GetAllUsers_WhenEmpty_ReturnsOkEmptyList()
        {
            _mockUserService!
                .Setup(s => s.GetAllUsersAsync())
                .ReturnsAsync(new List<UserResponseDto>());

            var result = await _controller!.GetAllUsers();

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllUsers_ServiceCalledOnce()
        {
            await _controller!.GetAllUsers();

            _mockUserService!
                .Verify(s => s.GetAllUsersAsync(), Times.Once);
        }

        #endregion

        #region GetUsers (Search)

        [Test]
        public async Task GetUsers_ValidModel_ReturnsOkWithResults()
        {
            var dto = new SearchUserDto();

            var users = new List<UserResponseDto>();

            _mockUserService!
                .Setup(s => s.GetUsersAsync(dto))
                .ReturnsAsync(users);

            var result = await _controller!.GetUsers(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(users, ok.Value);
        }

        [Test]
        public async Task GetUsers_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.GetUsers(new SearchUserDto());

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task GetUsers_EmptyResult_ReturnsOk()
        {
            var dto = new SearchUserDto();

            _mockUserService!
                .Setup(s => s.GetUsersAsync(dto))
                .ReturnsAsync(new List<UserResponseDto>());

            var result = await _controller!.GetUsers(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
        }

        #endregion

        #region CreateUser

        [Test]
        public async Task CreateUser_ValidInput_ReturnsOkWithCreatedMessage()
        {
            var dto = new UserCreateDto
            {
                Name = "John",
                Email = "john@test.com",
                Phone = "123",
                Password = "pass",
                RoleName = "Admin"
            };

            var result = await _controller!.CreateUser(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordCreated, ok.Value);

            _mockUserService!
                .Verify(s => s.CreateUserAsync(dto), Times.Once);
        }

        [Test]
        public async Task CreateUser_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.CreateUser(new UserCreateDto
            {
                Name = "John",
                Email = "john@test.com",
                Phone = "123",
                Password = "pass",
                RoleName = "Admin"
            });

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task CreateUser_ServiceCalledOnce()
        {
            var dto = new UserCreateDto
            {
                Name = "John",
                Email = "john@test.com",
                Phone = "123",
                Password = "pass",
                RoleName = "Admin"
            };

            await _controller!.CreateUser(dto);

            _mockUserService!
                .Verify(s => s.CreateUserAsync(dto), Times.Once);
        }

        #endregion

        #region UpdateUser

        [Test]
        public async Task UpdateUser_ValidInput_ReturnsOkWithUpdatedMessage()
        {
            var dto = new UserCreateDto
            {
                Name = "John",
                Email = "john@test.com",
                Phone = "123",
                Password = "pass",
                RoleName = "Admin"
            };

            var result = await _controller!.UpdateUser(1, dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(AppConstants.RecordUpdated, ok.Value);

            _mockUserService!
                .Verify(s => s.UpdateUserAsync(1, dto), Times.Once);
        }

        [Test]
        public async Task UpdateUser_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.UpdateUser(1, new UserCreateDto
            {
                Name = "John",
                Email = "john@test.com",
                Phone = "123",
                Password = "pass",
                RoleName = "Admin"
            });

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task UpdateUser_ServiceCalledOnce()
        {
            var dto = new UserCreateDto
            {
                Name = "John",
                Email = "john@test.com",
                Phone = "123",
                Password = "pass",
                RoleName = "Admin"
            };

            await _controller!.UpdateUser(10, dto);

            _mockUserService!
                .Verify(s => s.UpdateUserAsync(10, dto), Times.Once);
        }

        #endregion
    }
}