using Amazon.SQS.Model;

namespace CustomerProjectApi.Messaging
{
    public interface ISqsMessage
    {
        Task<SendMessageResponse> SendMessageAsync<T>(T message);
    }
}