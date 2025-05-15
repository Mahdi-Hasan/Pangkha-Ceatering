using MealManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MealManagement.Domain.Repositories
{
    public interface IMealRepository
    {
        Task<Meal> GetByIdAsync(Guid id);
        Task<IEnumerable<Meal>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Meal>> GetByDateAsync(DateTime date);
        Task<IEnumerable<Meal>> GetByUserIdAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<Meal> GetByUserIdAndDateAsync(Guid userId, DateTime date);
        Task AddAsync(Meal meal);
        Task UpdateAsync(Meal meal);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}