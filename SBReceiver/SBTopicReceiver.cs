using Microsoft.Azure.ServiceBus;
using SBShared.Models;
using System.Text;
using System.Text.Json;

public class SBTopicReceiver
{    
    const string topicName = "exampletopic";
    const string subscriptionName = "orders";
    ISubscriptionClient subscriptionClient;

    public SBTopicReceiver(string connectionString)
    {
        subscriptionClient = new SubscriptionClient(connectionString, topicName, subscriptionName);

        var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
            MaxConcurrentCalls = 1,
            AutoComplete = false
        };

        subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
    }

    async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
        var jsonString = Encoding.UTF8.GetString(message.Body);
        var order = JsonSerializer.Deserialize<OrderModel>(jsonString);
        Console.WriteLine($"Order received: Id:{order?.OrderId} Amount: {order?.Amount}");
        await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
    }


    Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
    {
        Console.WriteLine($"Message handler exception: {args.Exception}");
        return Task.CompletedTask;
    }
}
