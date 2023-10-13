using System.ComponentModel;

namespace Finance.Models;

public class Expense
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public ExpenseCategory Category { get; set; }
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
    [Description("Despesas Variáveis")] VariableExpenses
}
