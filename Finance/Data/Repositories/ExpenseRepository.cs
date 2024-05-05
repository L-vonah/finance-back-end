using Finance.Models;
using System.Linq.Expressions;

namespace Finance.Data.Repositories
{
    public class ExpenseRepository : FinanceRepository<Expense>
    {
        public ExpenseRepository(FinanceContext context) : base(context) { }

        public async Task<IEnumerable<Expense>> GetByCategoryAsync(ExpenseCategory category)
        {
            return await base.GetAllAsync(e => e.Category == category);
        }

        public IQueryable<Expense> FilterBy(Expression<Func<Expense, bool>> predicate)
        {
            return base.Where(predicate);
        }
    }
}
