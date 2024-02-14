using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using ConsumerApi.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ConsumerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerConsumerController : ControllerBase
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerConsumerController> _logger;

        public CustomerConsumerController(IAmazonSQS sqsClient, IConfiguration configuration, IMediator mediator, ILogger<CustomerConsumerController> logger)
        {
            _sqsClient = sqsClient;
            _configuration = configuration;
            var accessKey = _configuration.GetValue<string>("AWS:AccessKey");
            var secretKey = _configuration.GetValue<string>("AWS:SecretKey");
            _sqsClient = new AmazonSQSClient(accessKey, secretKey, RegionEndpoint.APSoutheast1);
            _configuration = configuration;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet(Name = "customer-handler")]
        public async Task HandleCustomer()
        {
            var cts = new CancellationTokenSource();
            var queueName = _configuration.GetValue<string>("QueueSettings:QueueName");
            var sqsQueryResponse = await _sqsClient.GetQueueUrlAsync(queueName);

            var messageRequest = new ReceiveMessageRequest
            {
                QueueUrl = sqsQueryResponse?.QueueUrl,
                AttributeNames = new List<string> { "All" },
                MessageAttributeNames = new List<string> { "All" },
                MaxNumberOfMessages = 1
            };

            while (!cts.IsCancellationRequested)
            {
                var response = await _sqsClient.ReceiveMessageAsync(messageRequest);

                foreach (var message in response.Messages)
                {
                    Console.WriteLine($"Message Id: {message.MessageId}");
                    Console.WriteLine($"Message Body: {message.Body}");
                    var messageType = message.MessageAttributes["MessageType"].StringValue;

                    var type = Type.GetType($"ConsumerApi.Messages.{messageType}");
                    var typeOff = typeof(CustomerUpdated).Name;

                    if (type is null)
                    {
                        _logger.LogWarning($"Failed when type {messageType}");
                        continue;
                    }

                    if (messageType != null)
                    {
                        try
                        {
                            var typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type)!;
                            await _mediator.Send(typedMessage, cts.Token);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            continue;
                        }

                        await _sqsClient.DeleteMessageAsync(sqsQueryResponse?.QueueUrl, message.ReceiptHandle);
                    }
                }

                await Task.Delay(2000);
            }
        }
    }
}