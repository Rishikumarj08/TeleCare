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
    public class CarePlanControllerTests
    {
        private Mock<ICarePlanService>? _mockService;
        private CarePlanController? _controller;

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ICarePlanService>();
            _controller = new CarePlanController(_mockService.Object);

            _controller.ModelState.Clear();
        }

        #endregion

        #region GET GetAllCarePlans

        [Test]
        public async Task GetAllCarePlans_ValidQuery_ReturnsOkWithData()
        {
            var query = new CarePlanSearchDTO();

            var data = new List<CarePlanResponseDTO>
            {
                new CarePlanResponseDTO { PlanName = "Plan1", Status = CarePlanStatus.Active }
            };

            _mockService!
                .Setup(s => s.GetAllCarePlansAsync(query))
                .ReturnsAsync(data);

            var result = await _controller!.GetAllCarePlans(query);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(data, ok.Value);
        }

        [Test]
        public async Task GetAllCarePlans_EmptyList_ReturnsOk()
        {
            _mockService!
                .Setup(s => s.GetAllCarePlansAsync(It.IsAny<CarePlanSearchDTO>()))
                .ReturnsAsync(new List<CarePlanResponseDTO>());

            var result = await _controller!.GetAllCarePlans(new CarePlanSearchDTO());

            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
        }

        [Test]
        public async Task GetAllCarePlans_ServiceCalledOnce()
        {
            var query = new CarePlanSearchDTO();

            await _controller!.GetAllCarePlans(query);

            _mockService!
                .Verify(s => s.GetAllCarePlansAsync(query), Times.Once);
        }

        #endregion

        #region GET GetById

        [Test]
        public async Task GetById_ValidId_ReturnsOk()
        {
            int id = 1;

            var response = new CarePlanResponseDTO
            {
                PlanName = "Plan1",
                Status = CarePlanStatus.Active
            };

            _mockService!
                .Setup(s => s.GetCarePlanByIdAsync(id))
                .ReturnsAsync(response);

            var result = await _controller!.GetById(id);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task GetById_InvalidId_ReturnsBadRequest()
        {
            var result = await _controller!.GetById(0);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(CarePlanConstants.InvalidRequest, bad.Value);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            _mockService!
                .Setup(s => s.GetCarePlanByIdAsync(5))
                .ReturnsAsync((CarePlanResponseDTO?)null);

            var result = await _controller!.GetById(5);

            var notFound = result as NotFoundObjectResult;
            Assert.NotNull(notFound);
            Assert.AreEqual(404, notFound!.StatusCode);
            Assert.AreEqual(CarePlanConstants.CarePlanNotFound, notFound.Value);
        }

        #endregion

        #region POST Create

        [Test]
        public async Task Create_ValidDto_ReturnsOkWithResult()
        {
            var dto = new CarePlanCreateDTO
            {
                PatientID = 1,
                ProgramID = 1,
                PlanName = "Plan",
                Description = "Desc",
                StartDate = System.DateTime.Now
            };

            var response = new CarePlanResponseDTO
            {
                PlanName = "Plan",
                Status = CarePlanStatus.Active
            };

            _mockService!
                .Setup(s => s.CreateCarePlanAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller!.Create(dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task Create_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.Create(new CarePlanCreateDTO());

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task Create_NullDto_ReturnsBadRequest()
        {
            var result = await _controller!.Create(null);

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        #endregion

        #region PUT Update

        [Test]
        public async Task Update_ValidInput_ReturnsOk()
        {
            int id = 1;

            var dto = new CarePlanUpdateDTO
            {
                CarePlanID = id,
                Status = CarePlanStatus.Active
            };

            var response = new CarePlanResponseDTO
            {
                PlanName = "Updated",
                Status = CarePlanStatus.Active
            };

            _mockService!
                .Setup(s => s.UpdateCarePlanAsync(id, dto))
                .ReturnsAsync(response);

            var result = await _controller!.Update(id, dto);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(response, ok.Value);
        }

        [Test]
        public async Task Update_InvalidModelState_ReturnsBadRequest()
        {
            _controller!.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.Update(1, new CarePlanUpdateDTO());

            var bad = result as BadRequestObjectResult;
            Assert.NotNull(bad);
            Assert.AreEqual(400, bad!.StatusCode);
        }

        [Test]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            int id = 5;

            var dto = new CarePlanUpdateDTO
            {
                CarePlanID = id,
                Status = CarePlanStatus.Active
            };

            _mockService!
                .Setup(s => s.UpdateCarePlanAsync(id, dto))
                .ReturnsAsync((CarePlanResponseDTO?)null);

            var result = await _controller!.Update(id, dto);

            var notFound = result as NotFoundObjectResult;
            Assert.NotNull(notFound);
            Assert.AreEqual(404, notFound!.StatusCode);
            Assert.AreEqual(CarePlanConstants.CarePlanNotFound, notFound.Value);
        }

        #endregion
    }
}