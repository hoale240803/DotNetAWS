using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CustomerProjectApi.Messaging
{
    public class SqsMessenger : ISqsMessage
    {
        private string? _queueUrl;
        
        private readonly IAmazonSQS _sqsClient;

        private readonly IOptions<QueueSettings> _queueSetting;

        private readonly IConfiguration _configuration;

        public SqsMessenger(IOptions<QueueSettings> queueSetting, IConfiguration configuration)
        {

            _configuration = configuration;
            var accessKey = _configuration.GetValue<string>("AWS:AccessKey");
            var secretKey = _configuration.GetValue<string>("AWS:SecretKey");
            _sqsClient = new AmazonSQSClient(accessKey, secretKey, RegionEndpoint.APSoutheast1);
            _queueSetting = queueSetting;
        }

        public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
        {
            string queueUrl = await GetQueueUrlAsync();

            var messageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = JsonSerializer.Serialize(message),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>()
                {
                    {
                        "MessageType",
                        new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = typeof(T).Name
                        }
                    }
                }
            };

            var result = await _sqsClient.SendMessageAsync(messageRequest);

            return result;
        }

        private async Task<string> GetQueueUrlAsync()
        {
            if (_queueUrl is not null)
            {
                return _queueUrl;
            }
            var queueName = _configuration.GetValue<string>("QueueSettings:QueueName");
            var response = await _sqsClient.GetQueueUrlAsync(queueName);
            _queueUrl = response.QueueUrl;

            return response.QueueUrl;
        }
    }
}