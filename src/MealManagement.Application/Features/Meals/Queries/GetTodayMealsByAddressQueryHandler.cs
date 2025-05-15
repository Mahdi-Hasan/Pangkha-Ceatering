//using MealManagement.Application.DTOs;
//using MealManagement.Domain.Repositories;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace MealManagement.Application.Features.Meals.Queries
//{
//    public class GetTodayMealsByAddressQueryHandler : IRequestHandler<GetTodayMealsByAddressQuery, Dictionary<string, List<MealDto>>>
//    {
//        private readonly IMealRepository _mealRepository;

//        public GetTodayMealsByAddressQueryHandler(IMealRepository mealRepository)
//        {
//            _mealRepository = mealRepository;
//        }

//        public async Task<Dictionary<string, List<MealDto>>> Handle(GetTodayMealsByAddressQuery request, CancellationToken cancellationToken)
//        {
//            var today = DateTime.Today;
//            var meals = await _mealRepository.GetByDateAsync(today);
            
//            var result = new Dictionary<string, List<MealDto>>();
            
//            foreach (var meal in meals.Where(m => m.Status))
//            {
//                var address = meal.User?.Address ?? "Unknown";
                
//                if (!result.ContainsKey(address))
//                {
//                    result[address] = new List<MealDto>();
//                }
                
//                result[address].Add(new MealDto
//                {
//                    Id = meal.Id,
//                    Date = meal.Date,
//                    Status = meal.Status,
//                    UserId = meal.UserId,
//                    UserName = meal.User?.Name
//                });
//            }
            
//            return result;
//        }
//    }
//}