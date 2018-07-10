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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
}
