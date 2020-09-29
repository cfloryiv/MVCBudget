using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBudget.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging;
using MVCBudget.Models;

namespace BlazorBudget.Pages
{
    public class PeriodReportBase : ComponentBase
    {

        [Inject]
        public IAccountDataService AccountDataService { get; set; }

        [Inject]
        public ILogger<AccountsBase> Logger { get; set; }

        [Parameter] public string Period { get; set; }
        public List<Sale> sales { get; set; }
        public Config config { get; set; }

        public string Message { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try

        {
            config = ((await AccountDataService.GetConfigs("PERIOD")));
        }
        catch (Exception e)
        {
            Message = "Something went wrong.";
        }

            if (Period == null)
            {
                Period = config.Value;
            }
            try
            {

                sales = (await AccountDataService.GetSales(Period))
                    .OrderBy(s=>s.Name)
                    .ToList();
            }
            catch (Exception e)
            {
                Message = "Something went wrong.";
            }

        }
    }
}

