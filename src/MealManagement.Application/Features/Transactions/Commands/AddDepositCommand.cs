using MediatR;

namespace MealManagement.Api.Controllers
{
    public AddDepositCommand : IRequest<object>
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentReference { get; set; }
    }
}