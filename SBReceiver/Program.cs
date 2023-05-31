using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
     .AddJsonFile($"appsettings.json");

var config = configuration.Build();
var connectionString = config.GetConnectionString("AzureServiceBus");


var receiver = new SBTopicReceiver(connectionString);
Console.ReadLine();