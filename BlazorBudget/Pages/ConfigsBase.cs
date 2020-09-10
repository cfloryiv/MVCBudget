using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBudget.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MVCBudget.Models;

namespace BlazorBudget.Pages
{
    public class ConfigsBase : ComponentBase
    {
        [Inject]
        public IAccountDataService AccountDataService { get; set; }

        [Inject]
        public ILogger<AccountsBase> Logger { get; set; }

        public List<Config> configs { get; set; }

        public string Message { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                configs = (await AccountDataService.GetConfigs()).ToList();
            }
            catch (Exception e)
            {
                Message = "Something went wrong.";
            }

        }
    }
}
