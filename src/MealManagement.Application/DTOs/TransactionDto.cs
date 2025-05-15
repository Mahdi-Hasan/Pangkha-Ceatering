using MealManagement.Domain.Enums;
using System;

namespace MealManagement.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentReference { get; set; }
        public Guid? AdjustedById { get; set; }
        public string AdjustedByName { get; set; }
        public string Notes { get; set; }
    }
}