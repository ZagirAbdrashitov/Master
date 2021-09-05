using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrugsManager.Models
{
    public interface IRepository
    {
        Task<List<Drug>> GetAllDrugs();

        Task<int> CreateDrug(Drug drug);

        Task<Drug> UpdateDrug(Drug drug);

        Task<Drug> DeleteDrug(int id);

        bool IsDrugExists(int id);

        bool IsDrugExists(string ndc);
    }
}
