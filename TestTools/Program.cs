using System;
using BlazorBudget.Tools;

namespace TestTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Period p=new Period("2020-09");
            
            p.bumpPeriod();
            Console.WriteLine(p.PeriodOut);
        }
    }
}
