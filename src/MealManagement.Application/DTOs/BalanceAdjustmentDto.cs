using System;

namespace MealManagement.Application.DTOs
{
    public class BalanceAdjustmentDto
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }
}