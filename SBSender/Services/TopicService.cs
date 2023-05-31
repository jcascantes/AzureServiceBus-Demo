using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;

namespace SBSender.Services
{
    public class TopicService : ITopicService
    {
        private readonly IConfiguration configuration;

        public TopicService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendMessageAsync<T>(T serviceBusMessage, string topicName)
        {
            var topicClient = new TopicClient(configuration.GetConnectionString("AzureServiceBus"), topicName);
            var messageBody = JsonSerializer.Serialize(serviceBusMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            message.UserProperties.Add("messageType", typeof(T).Name);

            await topicClient.SendAsync(message);
        }
    }
}
