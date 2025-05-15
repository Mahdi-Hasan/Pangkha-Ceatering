using MediatR;
using MealManagement.Application.DTOs;
using MealManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MealManagement.Application.Features.Meals.Queries
{
    public class GetTodayMealsQuery : IRequest<List<MealDto>>
    {
    }

    public class GetTodayMealsQueryHandler : IRequestHandler<GetTodayMealsQuery, List<MealDto>>
    {
        private readonly IMealRepository _mealRepository;

        public GetTodayMealsQueryHandler(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<List<MealDto>> Handle(GetTodayMealsQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var meals = await _mealRepository.GetByDateAsync(today);

            return meals.Select(m => new MealDto
            {
                Id = m.Id,
                Date = m.Date,
                Status = m.Status,
                UserId = m.UserId,
                UserName = m.User?.Name
            }).ToList();
        }
    }
}