//using MealManagement.Application.DTOs;
//using MealManagement.Domain.Repositories;
//using MediatR;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace MealManagement.Application.Features.Meals.Queries
//{
//    public class GetUserMealHistoryQueryHandler : IRequestHandler<GetUserMealHistoryQuery, List<MealDto>>
//    {
//        private readonly IMealRepository _mealRepository;

//        public GetUserMealHistoryQueryHandler(IMealRepository mealRepository)
//        {
//            _mealRepository = mealRepository;
//        }

//        public async Task<List<MealDto>> Handle(GetUserMealHistoryQuery request, CancellationToken cancellationToken)
//        {
//            var meals = await _mealRepository.GetByUserIdAndDateRangeAsync(
//                request.UserId, 
//                request.StartDate.Date, 
//                request.EndDate.Date);

//            return meals.Select(m => new MealDto
//            {
//                Id = m.Id,
//                Date = m.Date,
//                Status = m.Status,
//                UserId = m.UserId,
//                UserName = m.User?.Name
//            }).ToList();
//        }
//    }
//}