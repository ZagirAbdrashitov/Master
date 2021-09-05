﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrugsManager.Models;

namespace DrugsManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugsController : ControllerBase
    {
        private readonly IRepository _repository;

        public DrugsController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Drugs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drug>>> GetDrug()
        {
            return await _repository.GetAllDrugs();
        }

        // PUT: api/Drugs
        [HttpPut("{previousNdc}")]
        public async Task<IActionResult> PutDrug(Drug drug, string previousNdc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_repository.IsDrugExists(drug.Id))
            {
                if (!previousNdc.Equals(drug.Ndc) && _repository.IsDrugExists(drug.Ndc))
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
                await _repository.UpdateDrug(drug);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.IsDrugExists(drug.Id))
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

            return Ok();
        }

        // POST: api/Drugs
        [HttpPost]
        public async Task<ActionResult<Drug>> PostDrug(Drug drug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_repository.IsDrugExists(drug.Ndc))
            {
                return BadRequest("Drug with this NDC already exists");
            }

            try
            {
                var result = await _repository.CreateDrug(drug);

                return CreatedAtAction("GetDrug", result);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Drugs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Drug>> DeleteDrug(int id)
        {
            if (!_repository.IsDrugExists(id))
            {
                return NotFound();
            }

            try
            {
                await _repository.DeleteDrug(id);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            if (!_repository.IsDrugExists(id))
            {
                return Ok();
            }

            return Problem(title: "Drug is not deleted", detail: $"Drug ID: {id}");
        }
    }
}
