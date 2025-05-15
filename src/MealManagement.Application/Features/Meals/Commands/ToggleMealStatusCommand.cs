using MediatR;
using MealManagement.Application.Services;
using MealManagement.Domain.Entities;
using MealManagement.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MealManagement.Application.Features.Meals.Commands
{
    public class ToggleMealStatusCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
    }

    public class ToggleMealStatusCommandHandler : IRequestHandler<ToggleMealStatusCommand, bool>
    {
        private readonly IMealRepository _mealRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public ToggleMealStatusCommandHandler(
            IMealRepository mealRepository, 
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            _mealRepository = mealRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<bool> Handle(ToggleMealStatusCommand request, CancellationToken cancellationToken)
        {
            // Check if user exists
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                throw new Exception("User account is inactive");
            }

            // Normalize date to remove time component
            var normalizedDate = request.Date.Date;

            // Check if meal exists for this user and date
            var meal = await _mealRepository.GetByUserIdAndDateAsync(request.UserId, normalizedDate);

            if (meal == null)
            {
                // Create new meal with status true
                meal = new Meal
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Date = normalizedDate,
                    Status = true
                };

                await _mealRepository.AddAsync(meal);
                
                // Send notification
                await _notificationService.SendMealStatusChangedNotificationAsync(request.UserId, normalizedDate, true);
                
                return true;
            }
            else
            {
                // Toggle existing meal status
                meal.Status = !meal.Status;
                await _mealRepository.UpdateAsync(meal);
                
                // Send notification
                await _notificationService.SendMealStatusChangedNotificationAsync(request.UserId, normalizedDate, meal.Status);
                
                return meal.Status;
            }
        }
    }
}