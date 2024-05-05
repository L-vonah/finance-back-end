using AutoMapper;
using finance.Extensions;
using Finance.Data.Repositories;
using Finance.Dtos;
using Finance.Exceptions;
using Finance.Extensions;
using Finance.Models;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Finance.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "v1", IgnoreApi = false)]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseRepository _repository;
    private readonly IMapper _mapper;

    public ExpensesController(ExpenseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<Expense>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListExpenses([FromQuery] ExpenseFilter filter)
    {
        filter.Validate(typeof(Expense));

        var expenses = await _repository.FilterBy(e =>
                filter.Category == null || e.Category == filter.Category
            ).ToPaginatedListAsync(filter);

        return this.ToActionResult(HttpStatusCode.OK, expenses);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExpense(int id)
    {
        try
        {
            var expense = await _repository.FindAsync(id);
            return this.ToActionResult(HttpStatusCode.OK, expense);
        }
        catch (EntityNotFoundException e)
        {
            return this.ToActionResult(HttpStatusCode.NotFound, message: e.Message);
        }
    }

    [HttpGet("catgories")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public IActionResult ListExpenseCategories()
    {
        var categories = Enum.GetValues(typeof(ExpenseCategory))
            .Cast<ExpenseCategory>()
            .Select(c => c.GetDescription());

        return this.ToActionResult(HttpStatusCode.OK, categories);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> AddExpense([FromBody] ExpenseDto expenseDto)
    {
        Expense expense = _mapper.Map<Expense>(expenseDto.RequireNotNull());

        try
        {
            await _repository.AddAsync(expense);
        }
        catch (FinanceException e)
        {
            return this.ToActionResult(HttpStatusCode.ServiceUnavailable, message: e.Message);
        }

        return this.ToActionResult(HttpStatusCode.OK, expense);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> UpdateExpense([FromRoute] int id, [FromBody] ExpenseDto expenseDto)
    {
        expenseDto.RequireNotNull();
        try
        {
            Expense expense = await _repository.FindAsync(id);
            expenseDto.Id = id;
            _mapper.Map(expenseDto, expense);
            await _repository.UpdateAsync(expense);

            return this.ToActionResult(HttpStatusCode.OK, expense);
        }
        catch (FinanceException e)
        {
            return this.ToActionResult(HttpStatusCode.ServiceUnavailable, message: e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeleteExpense([FromRoute] int id)
    {
        try
        {
            Expense expense = await _repository.FindAsync(id);
            await _repository.RemoveAsync(expense);
            return this.ToActionResult(HttpStatusCode.OK, expense);
        }
        catch (EntityNotFoundException e)
        {
            return this.ToActionResult(HttpStatusCode.NotFound, message: e.Message);
        }
        catch (FinanceException e)
        {
            return this.ToActionResult(HttpStatusCode.ServiceUnavailable, message: e.Message);
        }
    }
}

public class ExpenseFilter : PageFilter
{
    public ExpenseCategory? Category { get; set; }
}