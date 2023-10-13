using Finance.Models;
using System.ComponentModel.DataAnnotations;

namespace Finance.Dtos
{
    public class ExpenseDto
    {
        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public ExpenseCategory Category { get; set; }
    }
}
