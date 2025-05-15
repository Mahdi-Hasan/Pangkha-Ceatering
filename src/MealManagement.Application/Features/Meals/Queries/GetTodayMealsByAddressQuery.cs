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
    public class GetTodayMealsByAddressQuery : IRequest<Dictionary<string, List<MealDto>>>
    {
    }

    public class GetTodayMealsByAddressQueryHandler : IRequestHandler<GetTodayMealsByAddressQuery, Dictionary<string, List<MealDto>>>
    {
        private readonly IMealRepository _mealRepository;

        public GetTodayMealsByAddressQueryHandler(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<Dictionary<string, List<MealDto>>> Handle(GetTodayMealsByAddressQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var meals = await _mealRepository.GetByDateAsync(today);

            // Filter only active meals
            var activeMeals = meals.Where(m => m.Status).ToList();

            // Group by address
            var mealsByAddress = activeMeals
                .GroupBy(m => m.User?.Address ?? "Unknown")
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(m => new MealDto
                    {
                        Id = m.Id,
                        Date = m.Date,
                        Status = m.Status,
                        UserId = m.UserId,
                        UserName = m.User?.Name
                    }).ToList()
                );

            return mealsByAddress;
        }
    }
}