using backend;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ExpenseController> _logger;

    public ExpenseController(AppDbContext context, ILogger<ExpenseController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet(Name = "GetExpenses")]
    public IEnumerable<Expense> Get()
    {
        return _context.Expenses.ToList();
    }

    [HttpPost(Name = "CreateExpense")]
    public IActionResult Create(ExpenseDto expense)
    {
        // Check if the associated budget exists    
        var associatedBudget = _context.Budgets.Find(expense.BudgetId);
        if (associatedBudget == null)
        {
            return BadRequest("Associated budget not found.");
        }

        expense.Id = 0; // Ensure ID is 0 for new entries

        if (string.IsNullOrWhiteSpace(expense.Description))
        {
            return BadRequest("Expense description is required.");
        }

        if (expense.Amount < 0)
        {
            return BadRequest("Expense amount cannot be negative.");
        }

        var newExpense = new Expense
        {
            Description = expense.Description,
            Amount = expense.Amount,
            BudgetId = expense.BudgetId,
            Date = expense.Date,
            Budget = associatedBudget
        };

        _context.Expenses.Add(newExpense);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = newExpense.Id }, newExpense);
    }

    [HttpPut("{id}", Name = "UpdateExpense")]
    public IActionResult Update(int id, ExpenseDto newExpense)
    {
        _logger.LogInformation("Updating expense with ID {ExpenseId} {Expense}", id, JsonSerializer.Serialize(newExpense));

        if (id != newExpense.Id)
        {
            return BadRequest("Expense ID mismatch.");
        }

        var existingExpense = _context.Expenses.Find(id);
        if (existingExpense == null)
        {
            return NotFound("Expense not found.");
        }

        // Check if the associated budget exists    
        var associatedBudget = _context.Budgets.Find(newExpense.BudgetId);
        if (associatedBudget == null)
        {
            return BadRequest("Associated budget not found.");
        }

        // Update properties
        existingExpense.Description = newExpense.Description;
        existingExpense.Amount = newExpense.Amount;
        existingExpense.BudgetId = newExpense.BudgetId;
        existingExpense.Date = newExpense.Date;
        existingExpense.Budget = associatedBudget;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteExpense")]
    public IActionResult Delete(int id)
    {
        var expense = _context.Expenses.Find(id);
        if (expense == null)
        {
            return NotFound("Expense not found.");
        }

        _context.Expenses.Remove(expense);
        _context.SaveChanges();
        return NoContent();
    }
}
