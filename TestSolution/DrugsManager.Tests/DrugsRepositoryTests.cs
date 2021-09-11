using DrugsManager.Data;
using DrugsManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DrugsManager.Tests
{
    public class DrugsRepositoryTests : TestsBase, IDisposable
    {
        private readonly DrugsManagerContext _testContext;
        private readonly DrugRepository _testRepository;

        public DrugsRepositoryTests()
        {
            _testContext = GetTestDatabase(Guid.NewGuid().ToString());
            _testRepository = new DrugRepository(_testContext);
        }

        public void Dispose()
        {
            _testContext.Dispose();
        }

        [Fact]
        public async Task GetAllDrugs_ReturnsCorrectListOfDrugs()
        {
            var getResult = await _testRepository.GetAllDrugs();

            Assert.Equal(_testContext.Drug, getResult);
        }

        [Fact]
        public async Task CreateDrug_DrugCreatedSuccessfully()
        {
            var testDrug = new Drug { Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            await _testRepository.CreateDrug(testDrug);

            Assert.Contains(testDrug, _testContext.Drug);
        }

        [Fact]
        public async Task CreateDrug_IdAlreadyExists()
        {
            _testContext.Entry(DefaultDrugsList[0]).State = EntityState.Detached;
            var testDrug = new Drug { Id = 111, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _testRepository.CreateDrug(testDrug));
            Assert.Equal($"Drug with Id [{testDrug.Id}] already exists.", exception.Message);
        }

        [Fact]
        public async Task UpdateDrug_DrugUpdatedSuccessfully()
        {
            var updatedDrug = new Drug { Id = DefaultDrugsList[0].Id, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            await _testRepository.UpdateDrug(updatedDrug);

            Assert.DoesNotContain(DefaultDrugsList[0], _testContext.Drug);
            Assert.Contains(updatedDrug, _testContext.Drug);
        }

        [Fact]
        public async Task UpdateDrug_DrugNotExists()
        {
            var updatedDrug = new Drug { Id = 123, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _testRepository.UpdateDrug(updatedDrug));
            Assert.Equal("Can't update drug: drug not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteDrug_DrugDeletedSuccessfully()
        {
            await _testRepository.DeleteDrug(DefaultDrugsList[1].Id);

            Assert.DoesNotContain(DefaultDrugsList[1], _testContext.Drug);
        }

        [Fact]
        public async Task DeleteDrug_DrugNotExists()
        {
            var wrongId = 123;
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _testRepository.DeleteDrug(wrongId));
            Assert.Contains($"No drug with Id [{wrongId}].", exception.Message);
        }

        [Fact]
        public void IsDrugExists_DrugExists()
        {
            Assert.True(_testRepository.IsDrugExists(DefaultDrugsList[2].Id));
            Assert.True(_testRepository.IsDrugExists(DefaultDrugsList[2].Ndc));
        }

        [Fact]
        public void IsDrugExists_DrugNotExists()
        {
            Assert.False(_testRepository.IsDrugExists(123));
            Assert.False(_testRepository.IsDrugExists("asdf1234"));
        }

        private static DrugsManagerContext GetTestDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DrugsManagerContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new DrugsManagerContext(options);
        }

        private DrugsManagerContext GetTestDatabase(string dbName)
        {
            var testContext = GetTestDbContext(dbName);
            testContext.AddRange(DefaultDrugsList);
            testContext.SaveChanges();
            return testContext;
        }
    }
}
