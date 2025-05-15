namespace MealManagement.Application.DTOs
{
    public class AddDepositDto
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentReference { get; set; }
    }
}