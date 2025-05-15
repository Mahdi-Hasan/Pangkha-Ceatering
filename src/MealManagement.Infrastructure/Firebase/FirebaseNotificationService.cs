using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MealManagement.Application.Common.Interfaces;
using MealManagement.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MealManagement.Infrastructure.Firebase
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly FirebaseMessaging _firebaseMessaging;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<FirebaseNotificationService> _logger;

        public FirebaseNotificationService(
            IConfiguration configuration, 
            IUserRepository userRepository,
            ILogger<FirebaseNotificationService> logger)
        {
            _userRepository = userRepository;
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

            _firebaseMessaging = FirebaseMessaging.DefaultInstance;
        }

        public async Task SendNotificationToUserAsync(Guid userId, string title, string body, Dictionary<string, string> data = null)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || string.IsNullOrEmpty(user.FirebaseToken))
                {
                    _logger.LogWarning($"Cannot send notification to user {userId}: User not found or no Firebase token available");
                    return;
                }

                var message = new Message
                {
                    Token = user.FirebaseToken,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data
                };

                var response = await _firebaseMessaging.SendAsync(message);
                _logger.LogInformation($"Successfully sent notification to user {userId}: {response}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification to user {userId}");
                throw;
            }
        }

        public async Task SendNotificationToTopicAsync(string topic, string title, string body, Dictionary<string, string> data = null)
        {
            try
            {
                var message = new Message
                {
                    Topic = topic,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data
                };

                var response = await _firebaseMessaging.SendAsync(message);
                _logger.LogInformation($"Successfully sent notification to topic {topic}: {response}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification to topic {topic}");
                throw;
            }
        }

        public async Task SendNotificationToAllAsync(string title, string body, Dictionary<string, string> data = null)
        {
            await SendNotificationToTopicAsync("all", title, body, data);
        }

        public async Task SubscribeToTopicAsync(string token, string topic)
        {
            try
            {
                var response = await _firebaseMessaging.SubscribeToTopicAsync(new[] { token }, topic);
                _logger.LogInformation($"Successfully subscribed to topic {topic}: {response.SuccessCount} success, {response.FailureCount} failure");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error subscribing to topic {topic}");
                throw;
            }
        }

        public async Task UnsubscribeFromTopicAsync(string token, string topic)
        {
            try
            {
                var response = await _firebaseMessaging.UnsubscribeFromTopicAsync(new[] { token }, topic);
                _logger.LogInformation($"Successfully unsubscribed from topic {topic}: {response.SuccessCount} success, {response.FailureCount} failure");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error unsubscribing from topic {topic}");
                throw;
            }
        }
    }
}