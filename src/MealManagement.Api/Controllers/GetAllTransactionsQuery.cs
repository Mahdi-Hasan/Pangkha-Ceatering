using MediatR;

namespace MealManagement.Api.Controllers
{
    internal class GetAllTransactionsQuery : IRequest<object>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}