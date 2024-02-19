using Entities.Common;
using HelloJob.Core.Configuration.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.DBContexts.SQLSERVER
{
    public class HelloJobDbContext:IdentityDbContext
    {
        public HelloJobDbContext(DbContextOptions<HelloJobDbContext> options) : base(options)
        {

        }
        public HelloJobDbContext() : base(new DbContextOptions<HelloJobDbContext>())
        {

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow.AddHours(4);
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //builder.Entity<Comment>().HasOne(x => x.AppUser).WithMany(x=>x.Comments).HasForeignKey(x=>x.Id);
            base.OnModelCreating(builder);
        }

    }
}
