using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;

namespace SBSender.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration configuration;

        public QueueService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendMessageAsync<T>(T serviceBusMessage, string queueName)
        {
            var queueClient = new QueueClient(configuration.GetConnectionString("AzureServiceBus"), queueName);
            var messageBody = JsonSerializer.Serialize(serviceBusMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));            

            await queueClient.SendAsync(message);
        }
    }
}
