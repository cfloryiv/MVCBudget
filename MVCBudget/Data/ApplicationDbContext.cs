using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MVCBudget.Models;

namespace MVCBudget.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MVCBudget.Models.Account> Account { get; set; }
        public DbSet<MVCBudget.Models.Tran> Tran { get; set; }
        public DbSet<MVCBudget.Models.Sale> Sale { get; set; }
        public DbSet<MVCBudget.Models.Config> Config { get; set; }
        public object Identity { get; internal set; }
    }
}
