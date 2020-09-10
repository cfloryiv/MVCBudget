using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using BlazorBudget.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using MVCBudget.Models;

namespace BlazorBudget.Pages
{
    public class AccountEditBase : ComponentBase
    {
        [Inject]
        public IAccountDataService AccountDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int accountID { get; set; }
        public Account account { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (accountID == 0)
            {

                account = new Account()
                {
                    ID = 0,
                    Name = "",
                    Email = "",
                    Budget = 0
                };
            }
            else
            {
                account = await AccountDataService.GetAccount(accountID);
            }
        }

        public async Task HandleSubmit()
        { 
            if (accountID==0)
            {
                await AccountDataService.AddAccount(account);
            }
            else
            {
                await AccountDataService.UpdateAccount(account);
            }
            NavigationManager.NavigateTo("/accounts");
        }

        protected async Task DeleteAccount()
        {
            if (accountID > 0)
            {
                await AccountDataService.DeleteAccount(accountID);
                NavigationManager.NavigateTo("/accounts");
            }
        }
    }
}
