using System;
using System.Collections.Generic;
using System.Text;
using BlazorBudget.Tools;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlazorBudget.Tests.Tools
{
    [TestClass]
    public class PeriodTests
    {
        [TestMethod]
        public void TestInvalidPeriod()
        {
            var period=new Period("2020-10");
            Assert.AreEqual(period.PeriodOut, "2020-10");
        }

        [TestMethod]
        public void TestPeriodBumpsAtNormalMonth()
        {
            var period = new Period("2020-10");
            period.bumpPeriod();
            Assert.AreEqual(period.PeriodOut, "2020-11");
        }

        [TestMethod]
        public void TestPeriodBumpsAtYearEnd()
        {
            var period = new Period("2020-12");
            period.bumpPeriod();
            Assert.AreEqual(period.PeriodOut, "2021-01");
        }

        [TestMethod]
        public void TestPeriodDoesntBumpBadPeriod()
        {
            var period = new Period("junk");
            period.bumpPeriod();
            Assert.AreEqual(period.PeriodOut, "junk");
        }
    }
}
