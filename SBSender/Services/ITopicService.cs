namespace SBSender.Services
{
    public interface ITopicService
    {
        Task SendMessageAsync<T>(T serviceBusMessage, string topicName);
    }
}