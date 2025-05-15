using MediatR;

namespace MealManagement.Api.Controllers
{
    internal class GetUserByIdQuery : IRequest<object>
    {
        public Guid Id { get; set; }
    }
}