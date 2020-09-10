using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBudget.Models
{
    public class Sale
    {
        public string Name { get; set; }
        public string Period { get; set; }

        public Decimal Budget { get; set; }

        public Decimal Sales { get; set; }
        public int ID { get; set; }
        public string Email { get; set; }
    }
}
