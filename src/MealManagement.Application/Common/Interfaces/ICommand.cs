using MediatR;

namespace MealManagement.Application.Common.Interfaces
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}