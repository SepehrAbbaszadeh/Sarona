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
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<NumberingPool> NumberingPools { get; set; }
        public DbSet<NetworkElement> NetworkElements { get; set; }
        public DbSet<Misc> Miscs { get; set; }

        //public long GetNextLinkSequenceValue()
        //{
        //    SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
        //    {
        //        Direction = System.Data.ParameterDirection.Output
        //    };
        //    Database.ExecuteSqlCommand(
        //               "SELECT @result = (NEXT VALUE FOR LinkSequence)", result);
        //    return (long)result.Value;
        //}
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

            AddSequences(modelBuilder);

            NetworkElementConfig(modelBuilder);

            modelBuilder.Entity<NumberingPoolNetworkElement>()
                .HasKey(x => new { x.NumberingPoolId, x.NetworkElementId });

            modelBuilder.Entity<Abbreviation>()
                .HasIndex(x => x.Abb).IsUnique();
            

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var fk in entity.GetForeignKeys())
                {
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }


            //foreach (var fk in modelBuilder.Model.GetOrAddEntityType(typeof(Link)).GetForeignKeys())
            //{
            //    fk.DeleteBehavior = DeleteBehavior.Restrict;
            //}
            
        }

        private void NetworkElementConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NetworkElement>()
                .HasMany(x => x.LinksOnEnd1)
                .WithOne(x => x.End1);
            modelBuilder.Entity<NetworkElement>()
                .HasMany(x => x.LinksOnEnd2)
                .WithOne(x => x.End2);
            modelBuilder.Entity<NetworkElement>()
                .HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<NetworkElement>()
                .HasMany(x => x.NetworkElements)
                .WithOne(x => x.Parent);
            modelBuilder.Entity<NetworkElement>()
                .HasIndex(x => x.Name).IsUnique();
        }

        private void AddSequences(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasSequence("LinkSequence")
            //    .StartsAt(1)
            //    .IncrementsBy(2);
            modelBuilder.HasSequence("PbxSequence")
                .StartsAt(1)
                .IncrementsBy(1);
            modelBuilder.HasSequence("RemoteSequence")
                .StartsAt(1)
                .IncrementsBy(1);
            modelBuilder.HasSequence("AccessSequence")
                .StartsAt(1)
                .IncrementsBy(1);
        }
    }
}
