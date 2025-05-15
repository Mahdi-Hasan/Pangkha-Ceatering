//using MealManagement.Domain.Entities;
//using MealManagement.Domain.Repositories;
//using MediatR;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace MealManagement.Application.Features.Meals.Commands
//{
//    public class ToggleMealStatusCommandHandler : IRequestHandler<ToggleMealStatusCommand, bool>
//    {
//        private readonly IMealRepository _mealRepository;
//        private readonly IUserRepository _userRepository;

//        public ToggleMealStatusCommandHandler(IMealRepository mealRepository, IUserRepository userRepository)
//        {
//            _mealRepository = mealRepository;
//            _userRepository = userRepository;
//        }

//        public async Task<bool> Handle(ToggleMealStatusCommand request, CancellationToken cancellationToken)
//        {
//            var user = await _userRepository.GetByIdAsync(request.UserId);
//            if (user == null)
//            {
//                throw new Exception("User not found");
//            }

//            var meal = await _mealRepository.GetByUserIdAndDateAsync(request.UserId, request.Date.Date);

//            if (meal == null)
//            {
//                // Create a new meal with status true
//                meal = new Meal
//                {
//                    Id = Guid.NewGuid(),
//                    UserId = request.UserId,
//                    Date = request.Date.Date,
//                    Status = true
//                };
//                await _mealRepository.AddAsync(meal);
//                return true;
//            }
//            else
//            {
//                // Toggle the status
//                meal.Status = !meal.Status;
//                await _mealRepository.UpdateAsync(meal);
//                return meal.Status;
//            }
//        }
//    }
//}