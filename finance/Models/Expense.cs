using Infrastructure.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Finance.Models;

public class Expense : BaseEntity
{
    public string Name { get; set; } = null!;

    [MaxLength(100)]
    public string? Description { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextPaymentDate { get; set; }
    public bool Finished { get; set; }
    public ExpenseCategory Category { get; set; }
    public ExpenseType Type { get; set; }
    public RecurrencyType? RecurrencyType { get; set; }
    public int? InstallmentCount { get; set; }
    public ICollection<Installment>? Installments { get; set; }
}

public enum ExpenseCategory
{
    [Description("Alimentação")] Food,
    [Description("Moradia")] Housing,
    [Description("Transporte")] Transportation,
    [Description("Lazer e Entretenimento")] Entertainment,
    [Description("Saúde")] Health,
    [Description("Educação")] Education,
    [Description("Dívidas")] Debts,
    [Description("Economias")] Savings,
    [Description("Investimentos")] Investments,
    [Description("Vestimentas")] Clothing,
    [Description("Doações")] Donations,
    [Description("Impostos")] Taxes,
    [Description("Seguro")] Insurance,
    [Description("Família")] Family,
    [Description("Tecnologia")] Technology,
    [Description("Passatempos")] Hobbies,
    [Description("Animais de Estimação")] Pets,
    [Description("Presentes")] Gifts,
    [Description("Negócios")] Business,
    [Description("Outros")] Others
}

public enum ExpenseType { Installment, Recurrent }

public enum RecurrencyType { FixedDay, Daily, Weekly, Monthly, Yearly }