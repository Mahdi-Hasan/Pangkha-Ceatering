using MediatR;

namespace MealManagement.Api.Controllers
{
    internal class ToggleUserStatusCommand : IRequest<object>
    {
        public Guid Id { get; set; }
    }
}