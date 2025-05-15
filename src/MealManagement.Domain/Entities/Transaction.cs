using MealManagement.Domain.Enums;
using System;

namespace MealManagement.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentReference { get; set; }
        public Guid? AdjustedById { get; set; }
        public string Notes { get; set; }
        
        public virtual User User { get; set; }
        public virtual User AdjustedBy { get; set; }
    }
}