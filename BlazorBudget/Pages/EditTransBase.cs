using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBudget.Services;
using Microsoft.AspNetCore.Components;
using MVCBudget.Models;

namespace BlazorBudget.Pages
{
    public class EditTransBase : ComponentBase
    {
        [Inject] public IAccountDataService AccountDataService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public int ID { get; set; }
        public Tran tran { get; set; }
        public Config config { get; set; }
        public List<Account> accounts { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // get current period
            config = await AccountDataService.GetConfigs("PERIOD");
            accounts = (await AccountDataService.GetAccounts()).OrderBy(a=>a.Name).ToList();
            if (ID == 0)
            {
                tran = new Tran()
                {
                    ID=0,
                    Name = "",
                    Period = config.Value,
                    Note="",
                    Trans_Date =DateTime.Now,
                    Amount = 0
                };

            }
            else
            {
                tran = await AccountDataService.GetTrans(ID);
            }
        }

        public async Task HandleSubmit()
        {
            if (ID == 0)
            {
                await AccountDataService.AddTrans(tran);
            }
            NavigationManager.NavigateTo("/");
        }

        public void CancelHandler()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
