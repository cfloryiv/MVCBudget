using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBudget.Services;
using Microsoft.AspNetCore.Components;
using MVCBudget.Models;

namespace BlazorBudget.Pages
{
    public class ViewTransBase : ComponentBase
    {
        [Inject]
        public IAccountDataService AccountDataService { get; set; }
        [Parameter]
        public string AccountName { get; set; }
        [Parameter]
        public string Period { get; set; }
        public List<Tran> trans { get; set; }
        public string Message { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AccountName == null || Period == null)
            {
                trans = new List<Tran>();
            }
            else
            {
                trans = (await AccountDataService.GetTrans(AccountName, Period)).ToList();
            }
        }
    }
}
