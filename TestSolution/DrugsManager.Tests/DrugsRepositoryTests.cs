using DrugsManager.Data;
using DrugsManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DrugsManager.Tests
{
    public class DrugsRepositoryTests : IDisposable
    {
        private readonly DrugsManagerContext _testContext;
        private readonly DrugRepository _testRepository;
        private readonly List<Drug> DefaultDrugsList = new List<Drug>
        {
            new Drug { Id=111, Ndc="11111111", Name="First Drug", PackSize=1, Unit=Unit.SmallPack, Price=1.11m},
            new Drug { Id=222, Ndc="22222222", Name="Second Drug", PackSize=2, Unit=Unit.MediumPack, Price=2.22m},
            new Drug { Id=333, Ndc="33333333", Name="Third Drug", PackSize=3, Unit=Unit.LargePack, Price=3.33m},
        };

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
            Assert.Equal("An item with the same key has already been added. Key: 111", exception.Message);
        }

        [Fact]
        public async Task UpdateDrug_DrugUpdatedSuccessfully()
        {
            var updatedDrug = new Drug { Id = DefaultDrugsList[0].Id, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };
            _testContext.Entry(DefaultDrugsList[0]).State = EntityState.Detached;

            await _testRepository.UpdateDrug(updatedDrug);

            Assert.DoesNotContain(DefaultDrugsList[0], _testContext.Drug);
            Assert.Contains(updatedDrug, _testContext.Drug);
        }

        [Fact]
        public async Task UpdateDrug_DrugNotExists()
        {
            var updatedDrug = new Drug { Id = 123, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _testRepository.UpdateDrug(updatedDrug));
            Assert.Equal("Attempted to update or delete an entity that does not exist in the store.", exception.Message);
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
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _testRepository.DeleteDrug(123));
            Assert.Contains("Value cannot be null.", exception.Message);
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
