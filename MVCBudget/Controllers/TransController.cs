using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MVCBudget.Data;
using MVCBudget.Models;

namespace MVCBudget.Controllers
{
    public class TransController : Controller
    {
        private readonly ApplicationDbContext _context;
        string Collection;
        MongoClient dbClient;
        IMongoDatabase db;
        public TransController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trans
       
        [Route("Index/{Name}/{Period}")]
        public async Task<IActionResult> Index(String Name, String Period)
        {
            var Email = User.Identity.Name;
            return View(await _context.Tran.Where(t => t.Name==Name && t.Period == Period).ToListAsync());
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tran.ToListAsync());
        }
        public async Task<IActionResult> Import()
        {
            var Email = User.Identity.Name;
            this.Collection = "trans";
            this.dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            db = dbClient.GetDatabase("Budget");
            var trans = _context.Tran;
            foreach (var tran in trans)
                _context.Tran.Remove(tran);
            await _context.SaveChangesAsync();

            var collection = db.GetCollection<BsonDocument>(Collection);
            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument());
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        string name = (string)document["name"];
                        string period = (string)document["period"];
                        DateTime trans_date = DateTime.Now;
                        decimal amount;
                        try
                        {
                            amount = Convert.ToDecimal(document["amount"]);
                        } catch
                        {
                            amount = 0;
                        }
                        string note;
                        try
                        {
                            note = (string)document["note"];
                        }
                        catch
                        {
                            note = "";
                        }


                        var tran = new Tran
                        {
                            Name = name,
                            Period = period,
                            Trans_Date = trans_date,
                            Amount = amount,
                            Note = note,
                            Email=""
                        };
                        _context.Tran.Add(tran);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Configs");
        }
        // GET: Trans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tran = await _context.Tran
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tran == null)
            {
                return NotFound();
            }

            return View(tran);
        }

        // GET: Trans/Create
        public IActionResult Create(String name, String Period)
        {
            var tran = new Tran()
            {
                Name = name,
                Period = Period,
                Trans_Date = DateTime.Now,
                Amount = 0,
                Note = ""
            };
            return View(tran);
        }

        // POST: Trans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Period,Trans_Date,Amount,Note,ID")] Tran tran)
        {
            if (ModelState.IsValid)
            {
                var Email = User.Identity.Name;
                tran.Email = Email;
                _context.Add(tran);
                await _context.SaveChangesAsync();
                var sale = _context.Sale.Where(s => s.Name == tran.Name && s.Period == tran.Period).FirstOrDefault();
                if (sale == null)
                {
                    var account = _context.Account.Where(a => a.Name == tran.Name).First();
                    
                    sale = new Sale()
                    {
                        Name = tran.Name,
                        Period = tran.Period,
                        Sales = tran.Amount,
                        Budget = account.Budget,
                        Email=Email
                    };
                    _context.Sale.Add(sale);
                }
                else
                {
                    sale.Sales += tran.Amount;
                }
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Sales", new { Period = tran.Period });
            }
            return View(tran);
        }

        // GET: Trans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tran = await _context.Tran.FindAsync(id);
            if (tran == null)
            {
                return NotFound();
            }
            return View(tran);
        }

        // POST: Trans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Period,Trans_Date,Amount,Note,ID")] Tran tran)
        {
            if (id != tran.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tran);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TranExists(tran.ID))
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
            return View(tran);
        }

        // GET: Trans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tran = await _context.Tran
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tran == null)
            {
                return NotFound();
            }

            return View(tran);
        }

        // POST: Trans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tran = await _context.Tran.FindAsync(id);
            var sale = _context.Sale.Where(s => s.Name == tran.Name && s.Period == tran.Period).FirstOrDefault();
            if (sale != null)
            {
                sale.Sales -= tran.Amount;
                await _context.SaveChangesAsync();
            }
            _context.Tran.Remove(tran);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TranExists(int id)
        {
            return _context.Tran.Any(e => e.ID == id);
        }
    }
}
