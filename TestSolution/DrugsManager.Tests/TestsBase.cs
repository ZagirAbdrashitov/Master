using DrugsManager.Models;
using System.Collections.Generic;

namespace DrugsManager.Tests
{
    public class TestsBase
    {
        protected readonly List<Drug> DefaultDrugsList = new List<Drug>
        {
            new Drug { Id=111, Ndc="11111111", Name="First Drug", PackSize=1, Unit=Unit.SmallPack, Price=1.11m},
            new Drug { Id=222, Ndc="22222222", Name="Second Drug", PackSize=2, Unit=Unit.MediumPack, Price=2.22m},
            new Drug { Id=333, Ndc="33333333", Name="Third Drug", PackSize=3, Unit=Unit.LargePack, Price=3.33m},
        };
    }
}
