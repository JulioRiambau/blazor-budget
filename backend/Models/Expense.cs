namespace backend;

public class Expense
{
    public int Id { get; set; }

    public string Description { get; set; }

    public decimal Amount { get; set; }

    public int BudgetId { get; set; }

    public DateOnly Date { get; set; }

    public Budget Budget { get; set; }
}