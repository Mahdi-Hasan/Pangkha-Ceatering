using MediatR;

namespace MealManagement.Api.Controllers
{
    internal class GetUserTransactionsQuery : IRequest<object>
    {
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}