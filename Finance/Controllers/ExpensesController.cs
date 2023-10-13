using AutoMapper;
using Finance.Dtos;
using Finance.Exceptions;
using Finance.Extensions;
using Finance.Infrastructure;
using Finance.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "v1", IgnoreApi = false)]
public class ExpensesController : ControllerBase
{
    private readonly IRepository<Expense> _repository;
    private readonly IMapper _mapper;

    public ExpensesController(IRepository<Expense> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Expense>))]
    public async Task<IActionResult> ListExpenses()
    {
        var expenses = await _repository.ToListAsync();
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Expense))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetExpense(int id)
    {
        try
        {
            var expense = await _repository.FirstAsync(id);
            return Ok(expense);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Expense))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(string))]
    public async Task<IActionResult> AddExpense([FromBody] ExpenseDto expenseDto)
    {
        expenseDto.RequireNotNull();
        Expense expense = _mapper.Map<Expense>(expenseDto);

        try
        {
            await _repository.AddAsync(expense);
        }
        catch (FinanceException e)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
        }

        return Ok(expense);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Expense))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(string))]
    public async Task<IActionResult> UpdateExpense([FromRoute] int id, [FromBody] ExpenseDto expenseDto)
    {
        expenseDto.RequireNotNull();
        try
        {
            Expense expense = await _repository.FirstAsync(id);
            expense = _mapper.Map<Expense>(expenseDto);
            await _repository.UpdateAsync(expense);

            return Ok(expense);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (FinanceException e)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(string))]
    public async Task<IActionResult> DeleteExpense([FromRoute] int id)
    {
        try
        {
            Expense expense = await _repository.FirstAsync(id);
            await _repository.RemoveAsync(expense!);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (FinanceException e)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
        }
    }
}
