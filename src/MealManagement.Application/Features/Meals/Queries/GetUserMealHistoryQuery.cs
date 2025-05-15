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
    public class GetUserMealHistoryQuery : IRequest<List<MealDto>>
    {
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetUserMealHistoryQueryHandler : IRequestHandler<GetUserMealHistoryQuery, List<MealDto>>
    {
        private readonly IMealRepository _mealRepository;
        private readonly IUserRepository _userRepository;

        public GetUserMealHistoryQueryHandler(IMealRepository mealRepository, IUserRepository userRepository)
        {
            _mealRepository = mealRepository;
            _userRepository = userRepository;
        }

        public async Task<List<MealDto>> Handle(GetUserMealHistoryQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var meals = await _mealRepository.GetByUserIdAndDateRangeAsync(
                request.UserId, 
                request.StartDate.Date, 
                request.EndDate.Date.AddDays(1).AddSeconds(-1)
            );

            return meals
                .Select(m => new MealDto
                {
                    Id = m.Id,
                    Date = m.Date,
                    Status = m.Status,
                    UserId = m.UserId,
                    UserName = user.Name
                })
                .ToList();
        }
    }
}