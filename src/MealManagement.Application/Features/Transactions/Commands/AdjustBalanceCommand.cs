using MediatR;

namespace MealManagement.Api.Controllers
{
    internal class AdjustBalanceCommand : IRequest<object>
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public Guid AdjustedById { get; set; }
        public string Notes { get; set; }
    }
}