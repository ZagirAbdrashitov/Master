using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrugsManager.Data;
using DrugsManager.Models;

namespace DrugsManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugsController : ControllerBase
    {
        private readonly DrugsManagerContext _context;

        public DrugsController(DrugsManagerContext context)
        {
            _context = context;
        }

        // GET: api/Drugs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drug>>> GetDrug()
        {
            return await _context.Drug.ToListAsync();
        }

        // PUT: api/Drugs
        [HttpPut("{previousNdc}")]
        public async Task<IActionResult> PutDrug(Drug drug, string previousNdc)
        {
            _context.Entry(drug).State = EntityState.Modified;
            if (!ModelState.IsValid)
            {
                return BadRequest(drug.Price);
            }
            if (DrugExists(drug.Id))
            {
                if (!previousNdc.Equals(drug.Ndc) && DrugExists(drug.Ndc))
                {
                    return BadRequest("Drug with this NDC already exists");
                }
            }
            else
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrugExists(drug.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // POST: api/Drugs
        [HttpPost]
        public async Task<ActionResult<Drug>> PostDrug(Drug drug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (DrugExists(drug.Ndc))
            {
                return BadRequest("Drug with this NDC already exists");
            }

            _context.Drug.Add(drug);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction("GetDrug", drug.Id);
        }

        // DELETE: api/Drugs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Drug>> DeleteDrug(int id)
        {
            var drug = await _context.Drug.FindAsync(id);
            if (drug == null)
            {
                return NotFound();
            }

            _context.Drug.Remove(drug);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            return drug;
        }

        private bool DrugExists(int id)
        {
            return _context.Drug.Any(e => e.Id == id);
        }

        private bool DrugExists(string ndc)
        {
            return _context.Drug.Any(e => e.Ndc == ndc);
        }
    }
}
