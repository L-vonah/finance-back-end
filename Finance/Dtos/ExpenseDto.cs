﻿using Finance.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Finance.Dtos
{
    public class ExpenseDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public ExpenseCategory Category { get; set; }
    }
}
