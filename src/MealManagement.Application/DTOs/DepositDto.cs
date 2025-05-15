namespace MealManagement.Application.DTOs
{
    public class DepositDto
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentReference { get; set; }
    }
}