using MediatR;

namespace MealManagement.Api.Controllers
{
    internal class UpdateUserCommand : IRequest<object>
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}