using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;

namespace BlazorBudget.Tools
{
    public class Period
    {
        private string _period;
        public Period(string period)
        {
            _period = period;
        }

        public string PeriodOut
        {
            get { return _period; }
        }
        public void bumpPeriod()
        {
            int ii = 0;
            try
            {
                ii = _period.IndexOf("-");
            }
            catch(ArgumentNullException ex)
            {
                ii = 0;
            }

            if (ii == 4 && _period.Length == 7)
            {
                string year = _period.Substring(0, 4);
                string month = _period.Substring(5, 2);
                int yr, mth;
                if (Int32.TryParse(year, out yr) && Int32.TryParse(month, out mth))
                {
                    mth++;
                    if (mth > 12)
                    {
                        mth = 1;
                        yr++;
                    }

                    _period = $"{yr}-{mth:D2}";
                }
            }
        }
    }
}
