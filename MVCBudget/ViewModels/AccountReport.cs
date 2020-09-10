using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBudget.Models
{
        public class AccountReport
        {
            public string Name { get; set; }
            public decimal Sales { get; set; }
            public decimal Budget { get; set; }
            public decimal Net { get; set; }
        }
    }

