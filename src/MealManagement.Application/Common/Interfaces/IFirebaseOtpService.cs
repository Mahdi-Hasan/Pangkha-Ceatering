using System.Threading.Tasks;

namespace MealManagement.Application.Common.Interfaces
{
    public interface IFirebaseOtpService
    {
        Task<string> SendOtpAsync(string phoneNumber);
        Task<bool> VerifyOtpAsync(string verificationId, string otp);
    }
}