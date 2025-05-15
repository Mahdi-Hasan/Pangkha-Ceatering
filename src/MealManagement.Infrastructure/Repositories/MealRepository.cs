using MealManagement.Domain.Entities;
using MealManagement.Domain.Repositories;
using MealManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealManagement.Infrastructure.Repositories
{
    public class MealRepository : IMealRepository
    {
        private readonly ApplicationDbContext _context;

        public MealRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Meal> GetByIdAsync(Guid id)
        {
            return await _context.Meals
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Meal>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Meals
                .Include(m => m.User)
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meal>> GetByDateAsync(DateTime date)
        {
            return await _context.Meals
                .Include(m => m.User)
                .Where(m => m.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meal>> GetByUserIdAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Meals
                .Include(m => m.User)
                .Where(m => m.UserId == userId && m.Date >= startDate && m.Date <= endDate)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }

        public async Task<Meal> GetByUserIdAndDateAsync(Guid userId, DateTime date)
        {
            return await _context.Meals
                .FirstOrDefaultAsync(m => m.UserId == userId && m.Date.Date == date.Date);
        }

        public async Task AddAsync(Meal meal)
        {
            await _context.Meals.AddAsync(meal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Meal meal)
        {
            _context.Meals.Update(meal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if (meal != null)
            {
                _context.Meals.Remove(meal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Meals.AnyAsync(m => m.Id == id);
        }
    }
}