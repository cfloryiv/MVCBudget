using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBudget.Tools;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MVCBudget.Data;
using MVCBudget.Models;

namespace APIBudget.Repositories
{
    public class BudgetLibraryRepository : IBudgetLibraryRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public BudgetLibraryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Tran> GetTrans(string accountName, string period)
        {
            return _context.Tran.Where(t => t.Name == accountName && t.Period == period).ToList();
        }

        public Tran GetTrans(int id)
        {
            return _context.Tran.FirstOrDefault(t => t.ID == id);
        }
        public IEnumerable<Config> GetConfigs()
        {
            return _context.Config.ToList();
        }

        public Config GetConfig(string key)
        {
            return _context.Config.FirstOrDefault(k => k.Key == key);
        }
        public void UpdateConfig(Config config)
        {

            if (config == null)
            {
                throw new ArgumentNullException(nameof(Config));
            }

            Config conf = _context.Config.FirstOrDefault(a => a.ID == config.ID);
            if (conf == null)
            {
                conf = config;

            }
            else
            {
                conf.Value = config.Value;
            }

            _context.Config.Update(conf);
            Save();
        }
        public IEnumerable<Sale> GetSales()
        {
            return _context.Sale.ToList();
        }

        public IEnumerable<Sale> GetSales(string Period)
        {
            return _context.Sale.Where(s => s.Period == Period).ToList();
        }
        public IEnumerable<Account> GetAccounts()
        {
            return _context.Account.ToList<Account>();
        }

        public Account GetAccount(int ID)
        {
            if (ID == 0)
            {
                throw new ArgumentNullException(nameof(ID));
            }

            return _context.Account.FirstOrDefault(i => i.ID == ID);
        }

        public void DeleteAccount(Account account)
        {
            _context.Account.Remove(account);
            Save();
        }

        public void AddAccount(Account account)
        {

            if (account == null)
            {
                throw new ArgumentNullException(nameof(Account));
            }
            _context.Account.Add(account);
            Save();
        }

        public void UpdateAccount(Account account)
        {

            if (account == null)
            {
                throw new ArgumentNullException(nameof(Account));
            }

            Account acct = _context.Account.FirstOrDefault(a => a.ID == account.ID);
            if (acct == null)
            {
                acct = account;
                
            }
            else
            {
                acct.Name = account.Name;
                acct.Budget = account.Budget;
            }

            _context.Account.Update(acct);
            Save();
        }

        public void AddTrans(Tran trans)
        {
            if (trans == null)
            {
                throw new ArgumentNullException(nameof(Tran));
            }

            _context.Tran.Add(trans);
            Save();
        }
        // update the sales from from the transaction record. If the sales record doesnt exists, create
        // it from the transaction and account data. Otherwise, just update the sales amount
        public void UpdateSales(Tran trans)
        {
            if (trans == null)
            {
                throw new ArgumentNullException(nameof(Tran));
            }

            Sale sale = _context.Sale.FirstOrDefault(s => s.Period == trans.Period && s.Name == trans.Name);
            if (sale == null)
            {
                Account acct = _context.Account.FirstOrDefault(a => a.Name == trans.Name);
                if (acct == null)
                {
                    throw new ArgumentNullException(nameof(Account));
                }
                sale=new Sale();
                sale.Period = trans.Period;
                sale.Name = trans.Name;
                sale.Email = "";
                sale.Sales = trans.Amount;
                sale.Budget = acct.Budget;

                _context.Sale.Add(sale);
            }
            else
            {
                sale.Sales += trans.Amount;
                _context.Sale.Update(sale);
            }

            Save();
        }

        public void StartNewPeriod()
        {
            Config config=GetConfig("PERIOD");
            string currentPeriod = config.Value;// get the current period
            Period newPeriod = new Period(currentPeriod);
            newPeriod.bumpPeriod();// generate the next period
            string nextPeriod = newPeriod.PeriodOut;

            // update the new period in the configuration record
            config.Value = nextPeriod;
            _context.Config.Update(config);

            IEnumerable<Account> accounts = _context.Account.ToList();
            foreach (var account in accounts)
            {
                // create a new entry if the entry exists in the current accounting period
                Sale sale = _context.Sale.FirstOrDefault(s => (s.Name == account.Name && s.Period == currentPeriod));
                if (sale != null)
                {
                    Tran trans = new Tran();
                    trans.Period = nextPeriod;
                    trans.Email = "";
                    trans.Amount = 0;
                    trans.Name = account.Name;
                    trans.Note = "Start of new period";

                    AddTrans(trans);
                    UpdateSales(trans);
                }
            }

            Save();
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
}
