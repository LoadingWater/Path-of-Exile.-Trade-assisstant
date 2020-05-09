using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("StashConnection") { }
        public DbSet<Tab> Tabs { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}