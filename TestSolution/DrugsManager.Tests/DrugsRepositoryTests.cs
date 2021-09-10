using DrugsManager.Data;
using DrugsManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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

        public DrugsRepositoryTests(ITestOutputHelper output)
        {
            var type = output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(output);

            _testContext = GetTestDatabase(test.DisplayName);
            _testRepository = new DrugRepository(_testContext);
        }

        public void Dispose()
        {
            _testContext.Dispose();
        }

        [Fact]
        public async Task RepositoryReturnsListOfDrugs()
        {
            // Act
            var getResult = await _testRepository.GetAllDrugs();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Drug>>(getResult);
            Assert.Equal(_testContext.Drug.Count(), getResult.Count());

            foreach (var drug in getResult)
            {
                Assert.Contains(drug, _testContext.Drug);
            }
        }

        [Fact]
        public async Task RepositoryCreatesDrug()
        {
            // Arrange
            var testDrug = new Drug { Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };

            //act
            var newId = await _testRepository.CreateDrug(testDrug);

            //assert
            Assert.Contains(testDrug, _testContext.Drug);
            Assert.True(_testRepository.IsDrugExists(newId));
            Assert.True(_testRepository.IsDrugExists(testDrug.Ndc));
        }

        [Fact]
        public async Task RepositoryUpdatesDrug()
        {
            // Arrange
            var updatedDrug = new Drug { Id = DefaultDrugsList[0].Id, Ndc = "1234asdf", Name = "Test Drug", PackSize = 2, Unit = Unit.MediumPack, Price = 1.23m };
            _testContext.Entry(DefaultDrugsList[0]).State = EntityState.Detached;

            //act
            await _testRepository.UpdateDrug(updatedDrug);

            //assert
            Assert.DoesNotContain(DefaultDrugsList[0], _testContext.Drug);
            Assert.Contains(updatedDrug, _testContext.Drug);
        }

        [Fact]
        public async Task RepositoryDeletesDrug()
        {
            //act
            await _testRepository.DeleteDrug(DefaultDrugsList[1].Id);

            //assert
            Assert.DoesNotContain(DefaultDrugsList[1], _testContext.Drug);
            Assert.False(_testRepository.IsDrugExists(DefaultDrugsList[1].Id));
            Assert.False(_testRepository.IsDrugExists(DefaultDrugsList[1].Ndc));
        }

        private static DrugsManagerContext GetTestDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DrugsManagerContext>()
                .EnableSensitiveDataLogging()
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
