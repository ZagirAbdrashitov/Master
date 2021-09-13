using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrugsManager.Models;
using AutoMapper;
using ServiceStack.Text;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using System;
using System.Linq;

namespace DrugsManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        public DrugsController(IMapper mapper, IRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // GET: api/Drugs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drug>>> GetDrug()
        {
            return await _repository.GetAllDrugs();
        }

        // GET: api/Drugs/export
        [HttpGet("export")]
        public async Task<IActionResult> GetDrugsAndConvertToCsv()
        {
            var allDrugs = await _repository.GetAllDrugs();
            var csvContent = CsvSerializer.SerializeToCsv(allDrugs);
            return File(Encoding.ASCII.GetBytes(csvContent), "text/csv");
        }

        // PUT: api/Drugs
        [HttpPut("{previousNdc}")]
        public async Task<IActionResult> PutDrug(Drug drug, string previousNdc)
        {
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

            return Ok();
        }

        // POST: api/Drugs
        [HttpPost]
        public async Task<ActionResult<Drug>> PostDrug(DrugDto drug)
        {
            if (_repository.IsDrugExists(drug.Ndc))
            {
                return BadRequest("Drug with this NDC already exists");
            }

            var newDrug = _mapper.Map<Drug>(drug);
            var result = await _repository.CreateDrug(newDrug);

            return CreatedAtAction("GetDrug", result);
        }

        // POST: api/Drugs
        [HttpPost("import")]
        public async Task<IActionResult> ConvertFromCsvAndPostDrug()
        {
            var createdCount = 0;
            var updatedCount = 0;
            var invalidCount = 0;
            var errorMessage = "";

            var uploadedFile = Request.Form.Files[0];
            var uploadedDrugs = new List<Drug>();
            try
            {
                uploadedDrugs = CsvSerializer.DeserializeFromStream<List<Drug>>(uploadedFile.OpenReadStream());
            }
            catch
            {
                return BadRequest("Can't convert to drugs. Some fields has bad data");
            }

            foreach (var drug in uploadedDrugs)
            {
                var validationResult = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(drug, new ValidationContext(drug), validationResult, true);

                if (isValid)
                {
                    if (_repository.IsDrugExists(drug.Id))
                    {
                        var allDrugs = await _repository.GetAllDrugs();
                        var existingDrug = allDrugs.Find(d => d.Id == drug.Id);
                        if (existingDrug.Ndc == drug.Ndc || !_repository.IsDrugExists(drug.Ndc))
                        {
                            await _repository.UpdateDrug(drug);
                            updatedCount++;
                        }
                        else
                        {
                            errorMessage += $"{Environment.NewLine}Drug with NDC [{drug.Ndc}] already exists{Environment.NewLine}";
                            invalidCount++;
                        }
                    }
                    else if (!_repository.IsDrugExists(drug.Ndc))
                    {
                        drug.Id = 0;
                        await _repository.CreateDrug(drug);
                        createdCount++;
                    }
                    else
                    {
                        errorMessage += $"{Environment.NewLine}Drug with NDC [{drug.Ndc}] already exists{Environment.NewLine}";
                        invalidCount++;
                    }
                }
                else
                {
                    errorMessage += $"{Environment.NewLine}Drug with NDC [{drug.Ndc}] has validation problems:{Environment.NewLine}";
                    errorMessage += $"{string.Join(Environment.NewLine, validationResult.Select(m => m.ErrorMessage))}{Environment.NewLine}";
                    invalidCount++;
                }
            }

            errorMessage += $"{Environment.NewLine}{invalidCount} drugs has validation problems;{Environment.NewLine}";
            return Content($"{errorMessage}{createdCount} drugs were created;{Environment.NewLine}{updatedCount} drugs were updated;");
        }

        // DELETE: api/Drugs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Drug>> DeleteDrug(int id)
        {
            if (!_repository.IsDrugExists(id))
            {
                return NotFound();
            }

            await _repository.DeleteDrug(id);

            if (!_repository.IsDrugExists(id))
            {
                return Ok();
            }

            return BadRequest($"Drug with Id [ {id}] is not deleted");
        }
    }
}
