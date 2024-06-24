using KhumaloCraft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace KhumaloCraft
{
    public class NotificationOrchestrator
    {
        [FunctionName("NotificationOrchestrator")]
        public static async Task RunNotificationOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            await context.CallActivityAsync("SendOrderReceivedNotification", null);
            await context.CallActivityAsync("SendOrderProcessedNotification", null);
            await context.CallActivityAsync("SendOrderShippedNotification", null);
        }
    }
}
