using BlazorBudget.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MVCBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBudget.Pages
{
    public class AccountsBase : ComponentBase
    {
        [Inject]
        public IAccountDataService AccountDataService { get; set; }

        [Inject]
        public ILogger<AccountsBase> Logger { get; set; }

        public List<Account> accounts { get; set; }

        public string Message { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                accounts = (await AccountDataService.GetAccounts())
                    .OrderBy(a => a.Name)
                    .ToList();
            }
            catch (Exception e)
            {
                Message = "Something went wrong.";
            }

        }
        /*
        public async void AddEmployeeDialog_OnDialogClose()
        {
            Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
            StateHasChanged();
        }

        protected void QuickAddEmployee()
        {
            AddEmployeeDialog.Show();
        }*/
    }
}
