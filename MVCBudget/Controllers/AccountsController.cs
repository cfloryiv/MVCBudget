using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MVCBudget.Data;
using MVCBudget.Models;

namespace MVCBudget.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        string Collection;
        MongoClient dbClient;
        IMongoDatabase db;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var Email = User.Identity.Name;
            //return View(await _context.Account.OrderBy(a => a.Name).Where(a => a.Email == Email).ToListAsync());
            return View(await _context.Account.OrderBy(a=>a.Name).ToListAsync());
        }
        public async Task<IActionResult> Import()
        {
            var Email = User.Identity.Name;

            this.Collection = "accounts";
            this.dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            db = dbClient.GetDatabase("Budget");
   
                var accounts = _context.Account.ToList();
                foreach (var account in accounts)
                    _context.Account.Remove(account);
                await _context.SaveChangesAsync();

                var collection = db.GetCollection<BsonDocument>(Collection);
                IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument());
                    while (await cursor.MoveNextAsync())
                    {
                        IEnumerable<BsonDocument> batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {
                            string name = (string)document["name"];
                            decimal budget = Convert.ToDecimal(document["budget"]);


                            var account = new Account
                            {
                                Name = name,
                                Budget = budget,
                                Email=Email
                            };
                            _context.Add(account);
                        }
                    }
                await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Configs");
        }
 

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.ID == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Budget")] Account account)
        {
            if (ModelState.IsValid)
            {
                var Email = User.Identity.Name;
                account.Email = Email;
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Budget")] Account account)
        {
            if (id != account.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.ID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Account.FindAsync(id);
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.ID == id);
        }
        
    }
}
