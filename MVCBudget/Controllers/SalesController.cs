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
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        string Collection;
        MongoClient dbClient;
        IMongoDatabase db;
        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sales
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var KeyValue = _context.Config.Where(c => c.Key == "PERIOD").First();

            string Period = KeyValue.Value;
            return View(await _context.Sale.Where(s => s.Period == Period).OrderBy(s => s.Name).ToListAsync());
        }
        [Route("Index/{Period}")]
        public async Task<IActionResult> Index(String Period)
        {
            return View(await _context.Sale.Where(s => s.Period == Period).OrderBy(s => s.Name).ToListAsync());
        }
        public async Task<IActionResult> IndexbyName(String Name)
        {
            return View(await _context.Sale.Where(s => s.Name == Name).OrderByDescending(s => s.Period).ToListAsync());
        }
        public IActionResult PeriodReport()
        {
            var Email = User.Identity.Name;
            return View(_context.Sale
            //    .Where(s => s.Email==Email)
                .GroupBy(s => s.Period)
                .Select(m => new PeriodReport { Period = m.Key, Budget = m.Sum(v => v.Budget), Sales = m.Sum(v => v.Sales), Net = m.Sum(v => v.Sales) - m.Sum(v => v.Budget) })
                .OrderByDescending(s => s.Period)
                .ToList());
        }
        public IActionResult AccountReport()
        {
            var Email = User.Identity.Name;
            return View(_context.Sale
             //   .Where(s => s.Email == Email)
                .GroupBy(s => s.Name)
                .Select(m => new AccountReport { Name = m.Key, Budget = m.Sum(v => v.Budget), Sales = m.Sum(v => v.Sales), Net = m.Sum(v => v.Sales) - m.Sum(v => v.Budget) })
                .OrderBy(s => s.Name)
                .ToList());
        }
        public async Task<IActionResult> Import()
        {
            var Email = User.Identity.Name;
            this.Collection = "sales";
            this.dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            db = dbClient.GetDatabase("Budget");
            var salesx = _context.Sale;
            foreach (var sale in salesx)
                _context.Sale.Remove(sale);
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
                        decimal budget = Convert.ToDecimal(document["budget"]);
                        string period = (string)document["period"];
                        decimal sales = Convert.ToDecimal(document["sales"]);



                        var sale = new Sale
                        {
                            Name = name,
                            Budget = budget,
                            Period = period,
                            Sales = sales,
                            Email=Email
                        };
                        _context.Sale.Add(sale);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Configs");
        }
        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sale
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Period,Budget,Sales,ID")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sale.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Period,Budget,Sales,ID")] Sale sale)
        {
            if (id != sale.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.ID))
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
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sale
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sale.FindAsync(id);
            _context.Sale.Remove(sale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sale.Any(e => e.ID == id);
        }
    }
}
