using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MVCBudget.Data;
using MVCBudget.Models;

namespace MVCBudget.Controllers
{
    public class ImportController : Controller
    {
        ApplicationDbContext _context;
        public ImportController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        string Collection;
        MongoClient dbClient;
        IMongoDatabase db;

        public IActionResult Index()
        {
            ImportAccounts("accounts");
            importAccountsAsync();
            ImportAccounts("sales");
            importSalesAsync();
            ImportAccounts("trans");
            importTransAsync();

            return RedirectToAction("Index", "Home");

        void ImportAccounts(string Collection)
        {
            this.Collection = Collection;
            this.dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            db = dbClient.GetDatabase("Budget");

        }
        async void importAccountsAsync()
        {
            var accounts = _context.Account;
            foreach (var account in accounts)
                _context.Account.Remove(account);
            _context.SaveChanges();

            var collection = db.GetCollection<BsonDocument>(Collection);
                using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument()))
                {
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
                                Budget = budget
                            };
                            _context.Account.Add(account);
                        }
                    }
                }
            _context.SaveChanges();
        }
        async void importSalesAsync()
        {
            var salesx = _context.Sale;
            foreach (var sale in salesx)
                _context.Sale.Remove(sale);
            _context.SaveChanges();

            var collection = db.GetCollection<BsonDocument>(Collection);
            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument()))
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
                            Sales = sales
                        };
                        _context.Sale.Add(sale);
                    }
                }
            }
            _context.SaveChanges();
        }
        async void importTransAsync()
            {
                var trans = _context.Tran;
                foreach (var tran in trans)
                    _context.Tran.Remove(tran);
                _context.SaveChanges();

                var collection = db.GetCollection<BsonDocument>(Collection);
                using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument()))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        IEnumerable<BsonDocument> batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {
                            string name = (string)document["name"];
                            string period = (string)document["period"];
                            DateTime trans_date = DateTime.Now;
                            decimal amount = Convert.ToDecimal(document["amount"]);
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
                                Note = note
                            };
                            _context.Tran.Add(tran);
                        }
                    }
                }
                _context.SaveChanges();
            }
        }
    }
}