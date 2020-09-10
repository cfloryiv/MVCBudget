using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBudget.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBudget.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IBudgetLibraryRepository _budgetLibraryRepository;

        public SalesController(IBudgetLibraryRepository budgetLibraryRepository)
        {
            _budgetLibraryRepository = budgetLibraryRepository;
        }
        [HttpGet()]
        public IActionResult GetSales()
        {
            var salesFromRepos = _budgetLibraryRepository.GetSales();
            return Ok(salesFromRepos);
        }

        [HttpGet("{period}")]
        public IActionResult GetSales(string period)
        {
            var salesFromRepos = _budgetLibraryRepository.GetSales(period);
            if (salesFromRepos == null)
            {
                return NotFound();
            }

            return Ok(salesFromRepos);
        }
    }
}
