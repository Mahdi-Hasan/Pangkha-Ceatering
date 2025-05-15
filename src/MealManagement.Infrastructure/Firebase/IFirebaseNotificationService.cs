using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MealManagement.Infrastructure.Firebase
{
    public interface IFirebaseNotificationService
    {
        Task SendNotificationToUserAsync(Guid userId, string title, string body, Dictionary<string, string> data = null);
        Task SendNotificationToTopicAsync(string topic, string title, string body, Dictionary<string, string> data = null);
        Task SendNotificationToAllAsync(string title, string body, Dictionary<string, string> data = null);
        Task SubscribeToTopicAsync(string token, string topic);
        Task UnsubscribeFromTopicAsync(string token, string topic);
    }
}