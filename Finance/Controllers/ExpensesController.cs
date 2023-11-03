using AutoMapper;
using Finance.Data.Repositories;
using Finance.Dtos;
using Finance.Exceptions;
using Finance.Extensions;
using Finance.Models;
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
    [ProducesResponseType(typeof(IEnumerable<Expense>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListExpenses()
    {
        var expenses = await _repository.GetAllAsync();
        return this.ToActionResult(HttpStatusCode.OK, expenses);
    }

    [HttpGet("{id}")]
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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
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
            return this.ToActionResult(HttpStatusCode.NotFound, message: e.Message);
        }
        catch (FinanceException e)
        {
            return this.ToActionResult(HttpStatusCode.NotFound, message: e.Message);
        }
    }
}
