using MealManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MealManagement.Domain.Services
{
    public interface IMealService
    {
        Task<bool> ToggleMealStatusAsync(Guid userId, DateTime date);
        Task<IEnumerable<Meal>> GetUserMealHistoryAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Meal>> GetTodayMealsAsync();
        Task<Dictionary<string, List<Meal>>> GetTodayMealsByAddressAsync();
    }
}