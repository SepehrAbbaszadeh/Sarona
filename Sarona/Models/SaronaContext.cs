using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona.Models
{
    public class SaronaContext : DbContext
    {
        public SaronaContext(DbContextOptions<SaronaContext> opts) : base(opts) { }
        public DbSet<Abbreviation> Abbreviations { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<NumberingPool> NumberingPools { get; set; }
        public DbSet<NetworkElement> NetworkElements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NetworkElement>()
                .HasMany(x => x.LinksOnEnd1)
                .WithOne(x => x.End1)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NetworkElement>()
                .HasMany(x => x.LinksOnEnd2)
                .WithOne(x => x.End2)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
