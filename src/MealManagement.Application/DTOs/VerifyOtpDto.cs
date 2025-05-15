namespace MealManagement.Application.DTOs
{
    public class VerifyOtpDto
    {
        public string PhoneNumber { get; set; }
        public string VerificationId { get; set; }
        public string Otp { get; set; }
    }
}