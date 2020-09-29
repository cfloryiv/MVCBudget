using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBudget.Repositories;
using Microsoft.AspNetCore.Mvc;
using MVCBudget.Models;

namespace APIBudget.Controllers
{
    [ApiController]
    [Route("api/configs")]
    public class ConfigsController : ControllerBase
    {
        private readonly IBudgetLibraryRepository _budgetLibraryRepository;

        public ConfigsController(IBudgetLibraryRepository budgetLibraryRepository)
        {
            _budgetLibraryRepository = budgetLibraryRepository;
        }
        [HttpGet()]
        public IActionResult GetConfigs()
        {
            var configsFromReposConfigs = _budgetLibraryRepository.GetConfigs();
            return Ok(configsFromReposConfigs);
        }

       [HttpGet("{key}")]
        public IActionResult GetConfig(string key)
        {
            var configFromRepos = _budgetLibraryRepository.GetConfig(key);
            if (configFromRepos == null)
            {
                return NotFound();
            }

            return Ok(configFromRepos);
        }
        [HttpPut]
        public IActionResult UpdateConfigs([FromBody] Config config)
        {
            if (config == null)
                return BadRequest();

            string key = config.Key;
            var ConfigToUpdate = _budgetLibraryRepository.GetConfig(key);

            if (ConfigToUpdate == null)
                return NotFound();

            _budgetLibraryRepository.UpdateConfig(config);

            return NoContent(); //success
        }
        [HttpPut("newperiod")]
        public IActionResult NewPeriod()
        {
    

            _budgetLibraryRepository.StartNewPeriod();

            return NoContent(); //success
        }
    }
}