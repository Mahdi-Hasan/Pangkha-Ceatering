using System;
using System.Threading.Tasks;

namespace MealManagement.Application.Services
{
    public interface INotificationService
    {
        Task SendMealStatusChangedNotificationAsync(Guid userId, DateTime date, bool status);
        Task SendLowBalanceNotificationAsync(Guid userId, decimal balance);
        Task SendDepositConfirmationAsync(Guid userId, decimal amount);
        Task SendAdminMealSummaryAsync(int totalMeals);
    }
}