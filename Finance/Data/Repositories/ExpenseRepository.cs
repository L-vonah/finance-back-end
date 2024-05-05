﻿using AutoMapper;
using Finance.Dtos;
using Finance.Exceptions;
using Finance.Models;
using System.Linq.Expressions;

namespace Finance.Data.Repositories
{
    public class ExpenseRepository : FinanceRepository<Expense>
    {
        private readonly IMapper _mapper;

        public ExpenseRepository(FinanceContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<Expense>> GetByCategoryAsync(ExpenseCategory category)
        {
            return await base.GetAllAsync(e => e.Category == category);
        }

        public IQueryable<Expense> FilterBy(Expression<Func<Expense, bool>> predicate)
        {
            return base.Where(predicate);
        }

        public async Task<Expense> AddExpense(ExpenseDto expenseDto)
        {
            return expenseDto.Type switch
            {
                ExpenseType.Installment => await AddInstallmentExpense(expenseDto),
                ExpenseType.Recurrent => await AddRecurrentExpense(expenseDto),
                _ => throw new ArgumentException("Invalid expense type")
            };
        }

        private async Task<Expense> AddInstallmentExpense(ExpenseDto expenseDto)
        {
            if (expenseDto.InstallmentCount == null || expenseDto.InstallmentCount == 0)
                throw new FinanceException("Installment expense must have an installment count");

            var expense = _mapper.Map<Expense>(expenseDto);

            expense.NextPaymentDate = expense.FirstPaymentDate;
            expense.Installments = [];

            while (expenseDto.InstallmentCount > 0)
            {
                expense.Installments.Add(new Installment
                {
                    Amount = expense.TotalAmount / expenseDto.InstallmentCount!.Value,
                    DueDate = expenseDto.FirstPaymentDate.AddMonths(expenseDto.InstallmentCount!.Value - 1),
                    Number = expenseDto.InstallmentCount!.Value,
                    Status = InstallmentStatus.Pending
                });

                expenseDto.InstallmentCount--;
            }

            await base.AddAsync(expense);
            return expense;
        }

        private async Task<Expense> AddRecurrentExpense(ExpenseDto expenseDto)
        {
            if (expenseDto.RecurrencyType == null)
                throw new FinanceException("Recurrent expense must have a recurrency type");

            var expense = _mapper.Map<Expense>(expenseDto);
            expense.NextPaymentDate = expenseDto.FirstPaymentDate;

            await base.AddAsync(expense);

            return expense;
        }
    }
}
