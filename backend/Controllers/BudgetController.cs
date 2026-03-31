using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class BudgetController : ControllerBase
{


    private readonly AppDbContext _context;

    public BudgetController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetBudgets")]
    public IEnumerable<Budget> Get()
    {
        return _context.Budgets.ToList();
    }

    [HttpPost(Name = "CreateBudget")]
    public IActionResult Create(Budget budget)
    {
        budget.Id = 0; // Ensure ID is 0 for new entries

        if (string.IsNullOrWhiteSpace(budget.Name))
        {
            return BadRequest("Budget name is required.");
        }

        if (budget.Amount < 0)
        {
            return BadRequest("Budget amount cannot be negative.");
        }

        _context.Budgets.Add(budget);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = budget.Id }, budget);
    }

    [HttpPut("{id}", Name = "UpdateBudget")]
    public IActionResult Update(int id, Budget newBudget)
    {
        if (id != newBudget.Id)
        {
            return BadRequest("Budget ID mismatch.");
        }

        var existingBudget = _context.Budgets.Find(id);
        if (existingBudget == null)
        {
            return NotFound("Budget not found.");
        }

         // Update properties
        if (string.IsNullOrWhiteSpace(newBudget.Name))
        {
            return BadRequest("Budget name is required.");
        }

        if (newBudget.Amount < 0)
        {
            return BadRequest("Budget amount cannot be negative.");
        }

        existingBudget.Name = newBudget.Name;
        existingBudget.Amount = newBudget.Amount;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteBudget")]
    public IActionResult Delete(int id)
    {
        var budget = _context.Budgets.Find(id);
        if (budget == null)
        {
            return NotFound("Budget not found.");
        }

        // Check for associated expenses
        bool hasExpenses = _context.Expenses.Any(e => e.BudgetId == id);
        if (hasExpenses)
        {
            return Conflict("Cannot delete budget with associated expenses.");
        }

        _context.Budgets.Remove(budget);
        _context.SaveChanges();
        return NoContent();
    }
}
