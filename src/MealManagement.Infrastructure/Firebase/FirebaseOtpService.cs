using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using MealManagement.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MealManagement.Infrastructure.Firebase
{
    public class FirebaseOtpService : IFirebaseOtpService
    {
        private readonly FirebaseAuth _firebaseAuth;
        private readonly ILogger<FirebaseOtpService> _logger;
        private readonly IConfiguration _configuration;

        public FirebaseOtpService(
            IConfiguration configuration,
            ILogger<FirebaseOtpService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Initialize Firebase if not already initialized
            if (FirebaseApp.DefaultInstance == null)
            {
                var firebaseConfigPath = configuration["Firebase:ServiceAccountKeyPath"];
                
                if (string.IsNullOrEmpty(firebaseConfigPath))
                {
                    throw new InvalidOperationException("Firebase service account key path is not configured.");
                }

                if (!File.Exists(firebaseConfigPath))
                {
                    throw new FileNotFoundException($"Firebase service account key file not found at: {firebaseConfigPath}");
                }

                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(firebaseConfigPath)
                });
            }

            _firebaseAuth = FirebaseAuth.DefaultInstance;
        }

        public async Task<string> SendOtpAsync(string phoneNumber)
        {
            try
            {
                // Generate a verification ID for the phone number
                // This is a simplified implementation - in a real app, you would use Firebase SDK
                // to send the actual SMS with OTP
                var sessionCookie = await _firebaseAuth.CreateSessionCookieAsync(
                    Guid.NewGuid().ToString(), // This would be a Firebase ID token in a real implementation
                    new SessionCookieOptions()
                    {
                        ExpiresIn = TimeSpan.FromMinutes(5)
                    });
                _logger.LogInformation($"OTP sent to {phoneNumber}");

                // Return the verification ID which will be used to verify the OTP
                return sessionCookie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending OTP to {phoneNumber}");
                throw;
            }
        }

        public async Task<bool> VerifyOtpAsync(string verificationId, string otp)
        {
            try
            {
                // In a real implementation, you would verify the OTP with Firebase
                // For this example, we'll simulate verification
                
                // Verify the session cookie
                var decodedToken = await _firebaseAuth.VerifySessionCookieAsync(verificationId);
                
                // In a real implementation, you would check if the OTP matches
                // For this example, we'll assume it matches if the verification ID is valid
                
                _logger.LogInformation("OTP verified successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying OTP");
                return false;
            }
        }
    }
}