using KhumaloCraft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace KhumaloCraft
{
    public class OrderOrchestrator
    {
        [FunctionName("OrderOrchestrator")]
        public static async Task<List<string>> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>("UpdateInventory", null));
            outputs.Add(await context.CallActivityAsync<string>("ProcessPayment", null));
            outputs.Add(await context.CallActivityAsync<string>("OrderConfirmation", null));
            outputs.Add(await context.CallActivityAsync<string>("SendNotification", null));

            return outputs;
        }

        [FunctionName("EntryPoint")]
        public static async Task<IActionResult> HttpStart([HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest req, [DurableClient] IDurableOrchestrationClient starter, ILogger log)
        {
            string instanceID = await starter.StartNewAsync("OrderOrchestrator", null);
            log.LogInformation($"Started orchestration with ID = '{instanceID}'.");

            return starter.CreateCheckStatusResponse(req, instanceID);
        }
    }
}
