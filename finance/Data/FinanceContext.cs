using Finance.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Finance.Data
{
    public class FinanceContext : DbContext
    {
        public static bool FilterDeleted { get; set; } = false;
        public const string DeletePropertyName = "DeletedAt";

        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options) { }

        public virtual DbSet<Expense> Expenses => Set<Expense>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!FilterDeleted && typeof(SoftableDeleted).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, DeletePropertyName);
                    var nullConstant = Expression.Constant(null, typeof(DateTime?));
                    var condition = Expression.Equal(property, nullConstant);

                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            modelBuilder.Entity<Expense>(e =>
            {
                e.HasKey(e => e.Id);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is SoftableDeleted))
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        (entry.Entity as SoftableDeleted)!.DeletedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        if (entry.Entity is BaseEntity)
                        {
                            var now = DateTime.UtcNow;
                            (entry.Entity as BaseEntity)!.CreatedAt = now;
                            (entry.Entity as BaseEntity)!.UpdatedAt = now;
                        }
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is BaseEntity)
                            (entry.Entity as BaseEntity)!.UpdatedAt = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
