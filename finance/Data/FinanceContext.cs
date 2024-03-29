using Finance.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Finance.Data
{
    public class FinanceContext : DbContext
    {
        public static bool FilterDeleted { get; set; } = false;

        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options) { }

        public virtual DbSet<Expense> Expenses => Set<Expense>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            foreach (var entityType in mb.Model.GetEntityTypes())
            {
                if (!FilterDeleted && typeof(SoftableDeleted).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "DeletedAt");
                    var nullConstant = Expression.Constant(null, typeof(DateTime?));
                    var condition = Expression.Equal(property, nullConstant);

                    var lambda = Expression.Lambda(condition, parameter);

                    mb.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            mb.Entity<Expense>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Category).HasConversion(new EnumToStringConverter<ExpenseCategory>());
            });
            mb.Entity<Installment>(e =>
            {
                e.HasKey(e => e.Id);
                e.HasIndex(e => new { e.ExpenseId, e.DueDate });
                e.HasOne(e => e.Expense).WithMany().HasForeignKey(e => e.ExpenseId);
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
