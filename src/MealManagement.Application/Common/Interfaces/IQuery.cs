using MediatR;

namespace MealManagement.Application.Common.Interfaces
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}