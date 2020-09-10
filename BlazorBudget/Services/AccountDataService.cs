using MVCBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorBudget.Services
{
    public class AccountDataService : IAccountDataService
    {
        private readonly HttpClient _httpClient;

        public AccountDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Account>>
                (await _httpClient.GetStreamAsync($"api/accounts"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<Account> GetAccount(int ID)
        {
            return await JsonSerializer.DeserializeAsync<Account>
                (await _httpClient.GetStreamAsync($"api/accounts/{ID}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<Account> AddAccount(Account account)
        {
            var AccountJson =
                new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/accounts", AccountJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Account>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateAccount(Account account)
        {
            var AccountJson =
                new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/accounts", AccountJson);
        }

        public async Task DeleteAccount(int AccountId)
        {
            await _httpClient.DeleteAsync($"api/accounts/{AccountId}");
        }
        public async Task<IEnumerable<Config>> GetConfigs()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Config>>
                (await _httpClient.GetStreamAsync($"api/configs"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<Config> GetConfigs(string key)
        {
            return await JsonSerializer.DeserializeAsync<Config>
                (await _httpClient.GetStreamAsync($@"api/configs/{key}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task UpdateConfigs(Config config)
        {
            var configJson =
                new StringContent(JsonSerializer.Serialize(config), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/configs", configJson);
        }
        public async Task<IEnumerable<Sale>> GetSales()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Sale>>
                (await _httpClient.GetStreamAsync($"api/sales"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<IEnumerable<Sale>> GetSales(string period)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Sale>>
                (await _httpClient.GetStreamAsync($@"api/sales/{period}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<IEnumerable<Tran>> GetTrans(string accountName, string period)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Tran>>
                (await _httpClient.GetStreamAsync($@"api/trans/{accountName}/{period}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<Tran> GetTrans(int id)

        {
            return await JsonSerializer.DeserializeAsync<Tran>
                (await _httpClient.GetStreamAsync($@"api/trans/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<Config> StartNewPeriod()
        {
            return await JsonSerializer.DeserializeAsync<Config>
                (await _httpClient.GetStreamAsync($"api/sales/startnewperiod"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<Tran> AddTrans(Tran trans)
        {
            var TranJson =
                new StringContent(JsonSerializer.Serialize(trans), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/trans", TranJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Tran>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }
    }
}
