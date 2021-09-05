using DrugsManager.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugsManager.Models
{
    public class DrugRepository : IRepository
    {
        private readonly DrugsManagerContext _context;

        public DrugRepository(DrugsManagerContext context)
        {
            _context = context;
        }

        public async Task<List<Drug>> GetAllDrugs()
        {
            return await _context.Drug.ToListAsync();
        }

        public async Task<int> CreateDrug(Drug drug)
        {
            _context.Drug.Add(drug);
            await _context.SaveChangesAsync();

            return drug.Id;
        }

        public async Task<Drug> UpdateDrug(Drug drug)
        {
            _context.Entry(drug).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return drug;
        }

        public async Task<Drug> DeleteDrug(int id)
        {
            var drug = await _context.Drug.FindAsync(id);

            _context.Drug.Remove(drug);
            await _context.SaveChangesAsync();

            return drug;
        }

        public bool IsDrugExists(int id)
        {
            return _context.Drug.Any(e => e.Id == id);
        }

        public bool IsDrugExists(string ndc)
        {
            return _context.Drug.Any(e => e.Ndc == ndc);
        }
    }
}
