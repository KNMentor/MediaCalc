using MediaCalc.L.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
namespace MediaCalc.L.Data
{

    public class MediaCalcDbContext : DbContext
    {
        public MediaCalcDbContext() : base("name=MediaCalcDbContext")
        {

        }

        public DbSet<Flat> Flats { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ConstFees> ConstFees { get; set; }

        public DbSet<Lease> Leases { get; set; }
    }
}