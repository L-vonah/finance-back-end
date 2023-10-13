using AutoMapper;
using Finance.Dtos;
using Finance.Models;

namespace Finance;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ExpenseDto, Expense>().ReverseMap();
    }
}
