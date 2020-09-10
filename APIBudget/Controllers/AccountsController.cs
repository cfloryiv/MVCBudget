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
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IBudgetLibraryRepository _budgetLibraryRepository;

        public AccountsController(IBudgetLibraryRepository budgetLibraryRepository)
        {
            _budgetLibraryRepository = budgetLibraryRepository;
        }
        [HttpGet()]
        public IActionResult GetAccounts()
        {
            var accountsFromRepos = _budgetLibraryRepository.GetAccounts();
            return Ok(accountsFromRepos);
        }

        [HttpGet("{ID}")]
        public IActionResult GetAccount(int ID)
        {
            var accountFromRepos = _budgetLibraryRepository.GetAccount(ID);
            if (accountFromRepos == null)
            {
                return NotFound();
            }

            return Ok(accountFromRepos);
        }
        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            if (account == null)
                return BadRequest();

            
            _budgetLibraryRepository.AddAccount(account);

            return Created("Account", account);
        }

        [HttpPut]
        public IActionResult UpdateAccount([FromBody] Account account)
        {
            if (account == null)
                return BadRequest();


            var AccountToUpdate = _budgetLibraryRepository.GetAccount(account.ID);

            if (AccountToUpdate == null)
                return NotFound();

            _budgetLibraryRepository.UpdateAccount(account);

            return NoContent(); //success
        }

        [HttpDelete("{ID}")]
        public IActionResult DeleteAccount(int ID)
        {
            if (ID == 0)
                return BadRequest();

            var AccountToDelete = _budgetLibraryRepository.GetAccount(ID);
            if (AccountToDelete == null)
                return NotFound();

            _budgetLibraryRepository.DeleteAccount(AccountToDelete);

            return NoContent();//success
        }
    }
}
