using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sarona.Models
{
    public partial class SwitchContext : DbContext
    {
        public SwitchContext()
        {
        }

        public SwitchContext(DbContextOptions<SwitchContext> options)
            : base(options)
        {
        }

        public virtual DbQuery<PRA_SIP_Sarona> RedFolder { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("name=switch");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Query<PRA_SIP_Sarona>().ToView("PRA_SIP_Sarona");
        }
    }
}
