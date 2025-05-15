using System;

namespace MealManagement.Domain.Entities
{
    public class Meal
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public Guid UserId { get; set; }
        
        public virtual User User { get; set; }
    }
}