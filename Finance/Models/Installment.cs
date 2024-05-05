using Infrastructure.Data;

namespace Finance.Models;

public class Installment : BaseEntity
{
    public int ExpenseId { get; set; }
    public Expense Expense { get; set; } = null!;
    public DateTime? PaymentDate { get; set; }
    public DateTime DueDate { get; set; }
    public int Number { get; set; }
    public decimal Amount { get; set; }
    public InstallmentStatus Status { get; set; }
}

public enum InstallmentStatus
{
    Pending,
    Paid,
    Overdue
}