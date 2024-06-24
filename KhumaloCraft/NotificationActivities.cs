using KhumaloCraft.Data;
using KhumaloCraft.Models;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System.Reactive;

namespace KhumaloCraft
{
    public class NotificationActivities
    {
        [FunctionName("SendOrderConfirmedNotification")]
        public static async Task SendOrderConfirmedNotification([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Sending order confirmed notification.");

            await Task.Delay(1000);
        }

        [FunctionName("SendPaymentProcessedNotification")]
        public static async Task SendPaymentProcessedNotification([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Sending payment processed notification.");

            await Task.Delay(1000);
        }
    }
}
