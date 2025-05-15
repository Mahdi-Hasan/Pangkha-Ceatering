using System.Threading.Tasks;

namespace MealManagement.Infrastructure.Firebase
{
    public interface IFirebaseOtpService
    {
        Task<string> SendOtpAsync(string phoneNumber);
        Task<bool> VerifyOtpAsync(string verificationId, string otp);
    }
}