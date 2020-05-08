using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Database
{
    class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("StashConnection") { }
        public DbSet<Stash> Stashes { get; set; }
        public DbSet<Tab> Tabs { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}