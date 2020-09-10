using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBudget.Models
{
    public class Tran
    {
        public string Name { get; set; }
        public string Period { get; set; }

        public DateTime Trans_Date { get; set; }

        public Decimal Amount { get; set; }

        public string Note { get; set; }

        public int ID { get; set; }
        public string Email { get; set; }
    }
}
