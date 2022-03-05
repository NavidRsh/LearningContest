using LearningContest.Domain.Common;
using LearningContest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Threading;
using System.Threading.Tasks;
using LearningContest.Domain.Entities.General;
using LearningContest.Persistence.Configs.General;

namespace LearningContest.Persistence
{
    public class LearningContestDbContext : DbContext
    {
        public LearningContestDbContext(DbContextOptions<LearningContestDbContext> options)
           : base(options)
        {
        }

       
     
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<BasicConfig> BasicConfigs { get; set; }
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearningContestDbContext).Assembly);

           
            #region General
            modelBuilder.ApplyConfiguration(new BasicConfigMap());
            #endregion

           
            
                       

           
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public class LearningContestDbContextFactory : IDesignTimeDbContextFactory<LearningContestDbContext>
        {
            public LearningContestDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<LearningContestDbContext>();
                optionsBuilder.UseSqlServer("Data Source=192.168.10.17,30871;Initial Catalog=LearningContest;User ID=sa;Password=vbfhg@75f4;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                return new LearningContestDbContext(optionsBuilder.Options);
            }
        }

    }
}
