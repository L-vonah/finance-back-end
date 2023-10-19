using AutoMapper;
using Finance.Data.Repositories;
using Finance.Dtos;
using Finance.Exceptions;
using Finance.Extensions;
using Finance.Models;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(typeof(IEnumerable<Expense>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListExpenses()
    {
        var expenses = await _repository.GetAllAsync();
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExpense(int id)
    {
        try
        {
            var expense = await _repository.FindAsync(id);
            return Ok(expense);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
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
    [ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> UpdateExpense([FromRoute] int id, [FromBody] ExpenseDto expenseDto)
    {
        expenseDto.RequireNotNull();
        try
        {
            Expense expense = await _repository.FindAsync(id);
            expenseDto.Id = id;
            _mapper.Map(expenseDto, expense);
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
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeleteExpense([FromRoute] int id)
    {
        try
        {
            Expense expense = await _repository.FindAsync(id);
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
