using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public DbSet<Misc> Miscs { get; set; }

        public long GetNextLinkSequenceValue()
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            Database.ExecuteSqlCommand(
                       "SELECT @result = (NEXT VALUE FOR LinkSequence)", result);
            return (long)result.Value;
        }
        public long GetNextPbxSequenceValue()
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            Database.ExecuteSqlCommand(
                       "SELECT @result = (NEXT VALUE FOR PbxSequence)", result);
            return (long)result.Value;
        }
        public long GetNextRemoteSequenceValue()
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            Database.ExecuteSqlCommand(
                       "SELECT @result = (NEXT VALUE FOR RemoteSequence)", result);
            return (long)result.Value;
        }
        public long GetNexAccessSequenceValue()
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            Database.ExecuteSqlCommand(
                       "SELECT @result = (NEXT VALUE FOR AccessSequence)", result);
            return (long)result.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence("LinkSequence")
                .StartsAt(1)
                .IncrementsBy(2);
            modelBuilder.HasSequence("PbxSequence")
                .StartsAt(1)
                .IncrementsBy(1);
            modelBuilder.HasSequence("RemoteSequence")
                .StartsAt(1)
                .IncrementsBy(1);
            modelBuilder.HasSequence("AccessSequence")
                .StartsAt(1)
                .IncrementsBy(1);
            modelBuilder.Entity<NetworkElement>()
                .HasMany(x => x.LinksOnEnd1)
                .WithOne(x => x.End1)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NetworkElement>()
                .HasMany(x => x.LinksOnEnd2)
                .WithOne(x => x.End2)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NumberingPoolNetworkElement>()
                .HasKey(x => new { x.NumberingPoolId, x.NetworkElementId });
            modelBuilder.Entity<Abbreviation>()
                .HasIndex(x => x.Abb).IsUnique();
            
            foreach(var fk in modelBuilder.Model.GetOrAddEntityType(typeof(Link)).GetForeignKeys())
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
        }
    }
}
