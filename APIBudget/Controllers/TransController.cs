using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBudget.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCBudget.Models;

namespace APIBudget.Controllers
{
    [Route("api/trans")]
    [ApiController]
    public class TransController : ControllerBase
    {
        private readonly IBudgetLibraryRepository _budgetLibraryRepository;

        public TransController(IBudgetLibraryRepository budgetLibraryRepository)
        {
            _budgetLibraryRepository = budgetLibraryRepository;
        }

        [HttpGet("{accountName}/{period}")]

        public IActionResult GetTrans(string accountName, string period)
        {
            var transFromRepos = _budgetLibraryRepository.GetTrans(accountName, period);
            return Ok(transFromRepos);
        }
        [HttpGet("{id}")]

        public IActionResult GetTrans(int id)
        {
            var transFromRepos = _budgetLibraryRepository.GetTrans(id);
            return Ok(transFromRepos);
        }
        [HttpPost]
        public IActionResult AddTrans([FromBody] Tran trans)
        {
            if (trans == null)
                return BadRequest();


            _budgetLibraryRepository.AddTrans(trans);
            _budgetLibraryRepository.UpdateSales(trans);

            return Created("Tran", trans);
        }
    }
}
