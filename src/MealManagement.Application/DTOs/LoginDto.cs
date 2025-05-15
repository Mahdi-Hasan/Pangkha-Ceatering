namespace MealManagement.Application.DTOs
{
    public class LoginDto
    {
        public string Identifier { get; set; } // Phone, Email, or Username
        public string Password { get; set; }
    }
}