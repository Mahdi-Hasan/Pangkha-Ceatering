using System;

namespace MealManagement.Application.DTOs
{
    public class MealDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}