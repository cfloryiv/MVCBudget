using System;
using System.Collections.Generic;
using MVCBudget.Models;

namespace APIBudget.Repositories
{
    public interface IBudgetLibraryRepository
    {
        IEnumerable<Config> GetConfigs();
        Config GetConfig(string key);
        void UpdateConfig(Config config); 
        IEnumerable<Tran> GetTrans(string accountName, string period);
        Tran GetTrans(int id);
        void AddTrans(Tran trans);
        IEnumerable<Sale> GetSales();
        void UpdateSales(Tran trans);
        IEnumerable<Sale> GetSales(string Period);
        IEnumerable<Account> GetAccounts();
        Account GetAccount(int ID);
        void DeleteAccount(Account account);
        void AddAccount(Account account);
        void UpdateAccount(Account account);
        bool Save();
        void Dispose();
    }
}