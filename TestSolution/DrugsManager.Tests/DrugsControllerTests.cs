using DrugsManager.Controllers;
using DrugsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DrugsManager.Tests
{
    public class DrugsControllerTests
    {
        [Fact]
        public void ControllerReturnsListOfDrugs()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetAllDrugs()).Returns(GetTestDrugs());
            var controller = new DrugsController(mock.Object);

            // Act
            var getResult = controller.GetDrug();

            // Assert
            Assert.IsType<Task<ActionResult<IEnumerable<Drug>>>>(getResult);
            Assert.IsAssignableFrom<IEnumerable<Drug>>(getResult.Result.Value);
            Assert.Equal(GetTestDrugs().Result.Count, getResult.Result.Value.Count());
        }

        private Task<List<Drug>> GetTestDrugs()
        {
            var drugs = new List<Drug>
            {
                new Drug { Id=1, Ndc="11111111", Name="First Drug", PackSize=1, Unit=Unit.SmallPack, Price=1.11m},
                new Drug { Id=1, Ndc="22222222", Name="Second Drug", PackSize=2, Unit=Unit.MediumPack, Price=2.22m},
                new Drug { Id=1, Ndc="33333333", Name="Third Drug", PackSize=3, Unit=Unit.LargePack, Price=3.33m},
            };
            return Task.FromResult(drugs);
        }
    }
}
