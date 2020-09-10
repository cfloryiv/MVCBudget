using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBudget.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore.Metadata;
using MVCBudget.Models;

namespace BlazorBudget.Pages
{
    public class ConfigEditBase : ComponentBase
    {
        [Inject] public IAccountDataService AccountDataService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        public Config config { get; set; }

        protected override async Task OnInitializedAsync()
        {
            config = await AccountDataService.GetConfigs("PERIOD");
        }

        public async Task HandleSubmit()
        { 
            await AccountDataService.UpdateConfigs(config);
            NavigationManager.NavigateTo("/");
        }

        public void CancelHandler()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
