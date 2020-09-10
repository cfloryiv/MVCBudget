using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBudget.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using MVCBudget.Models;

namespace BlazorBudget.Pages
{
    public class SalesBase: ComponentBase
    {

        [Inject]
        public IAccountDataService AccountDataService { get; set; }

        [Inject]
        public ILogger<AccountsBase> Logger { get; set; }

        public List<salesProj> salesx { get; set; }

        public string Message { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try { 
            
                salesx = (await AccountDataService.GetSales())
                    .OrderByDescending(s => s.Period)
                    .GroupBy(s => s.Period)
                    .Select(g => new salesProj(g.Key, g.Sum(f => f.Budget),g.Sum(f => f.Sales)))
                    .ToList();
            }
            catch (Exception e)
            {
                Message = "Something went wrong.";
            }

        }
    }
    public class salesProj {
        public salesProj(String period, decimal budget, decimal sales)
        {
            Period = period;
            Budget = budget;
            Sales = sales;
        }
        public string Period { get; set; } 
        public decimal Budget { get; set; }
        public decimal Sales { get; set; }

    }
}
