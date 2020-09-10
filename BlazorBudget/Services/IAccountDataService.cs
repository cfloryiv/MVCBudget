using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MVCBudget.Models;

namespace BlazorBudget.Services
{
    public interface IAccountDataService
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account> GetAccount(int ID);
        Task<Account> AddAccount(Account account);
        Task UpdateAccount(Account account);
        Task DeleteAccount(int ID);
        Task<IEnumerable<Tran>> GetTrans(string accountName, string period);
        Task<Tran> GetTrans(int id);
        Task<Tran> AddTrans(Tran tran);
        Task<IEnumerable<Config>> GetConfigs();
        Task<Config> GetConfigs(string key);
        Task UpdateConfigs(Config config);
        Task<IEnumerable<Sale>> GetSales();
        Task<IEnumerable<Sale>> GetSales(string period);
    }
}