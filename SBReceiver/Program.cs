using Microsoft.Azure.ServiceBus;
using SBShared.Models;
using System.Text;
using System.Text.Json;

const string connectionString = @"[Replace]";
const string queueName = "personqueue";
IQueueClient queueClient;

queueClient = new QueueClient(connectionString, queueName);

var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
{
    MaxConcurrentCalls = 1,
    AutoComplete = false
};

queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

Console.ReadLine();

await queueClient.CloseAsync();

async Task ProcessMessagesAsync(Message message, CancellationToken token)
{
    var jsonString = Encoding.UTF8.GetString(message.Body);
    var person = JsonSerializer.Deserialize<PersonModel>(jsonString);
    Console.WriteLine($"Person received: {person?.FirstName} {person?.LastName}");
    await queueClient.CompleteAsync(message.SystemProperties.LockToken);
}

Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
{
    Console.WriteLine($"MEssage handler exception: {args.Exception}");
    return Task.CompletedTask;
}