namespace MealManagement.Application.DTOs
{
    public class ResetPasswordDto
    {
        public string PhoneNumber { get; set; }
        public string VerificationId { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }
}