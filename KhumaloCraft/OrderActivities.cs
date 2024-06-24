using KhumaloCraft.Data;
using KhumaloCraft.Models;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System.Reactive;

namespace KhumaloCraft
{
    public class OrderActivities
    {

        [FunctionName("UpdateInventory")]
        public static async Task<string> UpdateInventory([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Updating inventory for order.");

            await Task.Delay(1000);

            return "Inventory updated.";
        }

        [FunctionName("ProcessPayment")]
        public static async Task<string> ProcessPayment([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Processing payment for order.");

            await Task.Delay(1000);

            return "Payment processed.";
        }

        [FunctionName("OrderConfirmation")]
        public static async Task<string> OrderConfirmation([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Confirming order.");

            await Task.Delay(1000);

            return "Order confirmed.";
        }

    }
}
