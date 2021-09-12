using DrugsManager.Controllers;
using DrugsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace DrugsManager.Tests
{
    public class DrugsControllerTests : TestsBase
    {
        [Fact]
        public async Task GetDrug_ReturnsCorrectListOfDrugs()
        {
            var mockRepository = Substitute.For<IRepository>();
            mockRepository.GetAllDrugs().Returns(DefaultDrugsList);
            var controller = new DrugsController(mockRepository);

            var getResult = await controller.GetDrug();

            Assert.Equal(DefaultDrugsList, getResult.Value);
        }

        [Fact]
        public async Task PutDrug_DrugWithSameNdcUpdatedSuccessfully()
        {
            var updatedDrug = new Drug { Id = DefaultDrugsList[0].Id, Ndc = "11111111", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var mockRepository = Substitute.For<IRepository>();
            mockRepository.UpdateDrug(updatedDrug).Returns(updatedDrug);
            mockRepository.IsDrugExists(DefaultDrugsList[0].Id).Returns(true);
            mockRepository.IsDrugExists("11111111").Returns(true);
            var controller = new DrugsController(mockRepository);

            var putResult = await controller.PutDrug(updatedDrug, "11111111");

            Assert.IsType<OkResult>(putResult);
        }

        [Fact]
        public async Task PutDrug_DrugWithNewUniqueNdcUpdatedSuccessfully()
        {
            var updatedDrug = new Drug { Id = DefaultDrugsList[0].Id, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var mockRepository = Substitute.For<IRepository>();
            mockRepository.UpdateDrug(updatedDrug).Returns(updatedDrug);
            mockRepository.IsDrugExists(DefaultDrugsList[0].Id).Returns(true);
            mockRepository.IsDrugExists("1234asdf").Returns(false);
            var controller = new DrugsController(mockRepository);

            var putResult = await controller.PutDrug(updatedDrug, "11111111");

            Assert.IsType<OkResult>(putResult);
        }

        [Fact]
        public async Task PutDrug_DrugWithNewNotUniqueNdcNotUpdated()
        {
            var updatedDrug = new Drug { Id = DefaultDrugsList[0].Id, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var mockRepository = Substitute.For<IRepository>();
            mockRepository.IsDrugExists(DefaultDrugsList[0].Id).Returns(true);
            mockRepository.IsDrugExists("1234asdf").Returns(true);
            var controller = new DrugsController(mockRepository);

            var putResult = await controller.PutDrug(updatedDrug, "11111111");
            
            var badResult = Assert.IsType<BadRequestObjectResult>(putResult);
            Assert.Equal("Drug with this NDC already exists", badResult.Value);
        }

        [Fact]
        public async Task PutDrug_DrugNotExists()
        {
            var updatedDrug = new Drug { Id = DefaultDrugsList[0].Id, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var mockRepository = Substitute.For<IRepository>();
            mockRepository.IsDrugExists("1234asdf").Returns(false);
            var controller = new DrugsController(mockRepository);

            var putResult = await controller.PutDrug(updatedDrug, "11111111");

            Assert.IsType<NotFoundResult>(putResult);
        }

        [Fact]
        public async Task PutDrug_DbUpdateConcurrencyExceptionDrugNotFound()
        {
            var updatedDrug = new Drug { Id = DefaultDrugsList[0].Id, Ndc = "11111111", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var mockRepository = Substitute.For<IRepository>();
            mockRepository.UpdateDrug(updatedDrug).Returns(Task.FromException<Drug>(new DbUpdateConcurrencyException()));
            mockRepository.IsDrugExists(DefaultDrugsList[0].Id).Returns(true, false);
            mockRepository.IsDrugExists("11111111").Returns(true);
            var controller = new DrugsController(mockRepository);

            var putResult = await controller.PutDrug(updatedDrug, "11111111");

            Assert.IsType<NotFoundResult>(putResult);
        }

        [Fact]
        public async Task PostDrug_DrugCreatedSuccessfully()
        {
            var newDrug = new Drug { Id = 123, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var mockRepository = Substitute.For<IRepository>();
            mockRepository.CreateDrug(newDrug).Returns(newDrug.Id);
            mockRepository.IsDrugExists(newDrug.Ndc).Returns(false);
            mockRepository.IsDrugExists(newDrug.Id).Returns(false);
            var controller = new DrugsController(mockRepository);

            var postResult = await controller.PostDrug(newDrug);

            var createdResult = Assert.IsType<CreatedAtActionResult>(postResult.Result);
            Assert.Equal(newDrug.Id, createdResult.Value);
        }

        [Fact]
        public async Task PostDrug_DrugWithNdcAlreadyExists()
        {
            var newDrug = new Drug { Id = 123, Ndc = "11111111", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var mockRepository = Substitute.For<IRepository>();
            mockRepository.IsDrugExists(newDrug.Ndc).Returns(true);
            mockRepository.IsDrugExists(newDrug.Id).Returns(false);
            var controller = new DrugsController(mockRepository);

            var postResult = await controller.PostDrug(newDrug);

            var badResult = Assert.IsType<BadRequestObjectResult>(postResult.Result);
            Assert.Equal("Drug with this NDC already exists", badResult.Value);
        }

        [Fact]
        public async Task DeleteDrug_DrugDeletedSuccessfully()
        {
            var mockRepository = Substitute.For<IRepository>();
            mockRepository.DeleteDrug(DefaultDrugsList[0].Id).Returns(DefaultDrugsList[0]);
            mockRepository.IsDrugExists(DefaultDrugsList[0].Id).Returns(true, false);
            var controller = new DrugsController(mockRepository);

            var deleteResult = await controller.DeleteDrug(DefaultDrugsList[0].Id);

            Assert.IsType<OkResult>(deleteResult.Result);
        }

        [Fact]
        public async Task DeleteDrug_DrugNotFound()
        {
            var mockRepository = Substitute.For<IRepository>();
            mockRepository.IsDrugExists(DefaultDrugsList[0].Id).Returns(false);
            var controller = new DrugsController(mockRepository);

            var deleteResult = await controller.DeleteDrug(DefaultDrugsList[0].Id);

            Assert.IsType<NotFoundResult>(deleteResult.Result);
        }

        [Fact]
        public async Task DeleteDrug_DrugNotDeleted()
        {
            var mockRepository = Substitute.For<IRepository>();
            mockRepository.DeleteDrug(DefaultDrugsList[0].Id).Returns(DefaultDrugsList[0]);
            mockRepository.IsDrugExists(DefaultDrugsList[0].Id).Returns(true, true);
            var controller = new DrugsController(mockRepository);

            var deleteResult = await controller.DeleteDrug(DefaultDrugsList[0].Id);

            var badResult = Assert.IsType<BadRequestObjectResult>(deleteResult.Result);
            Assert.Equal($"Drug with Id [ {DefaultDrugsList[0].Id}] is not deleted", badResult.Value);
        }
    }
}
