using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQSPublisher;
using System.Text.Json;

var sqsClient = new AmazonSQSClient("AKIAY3U6AD3IO6Z53DU2", "mhVhi9uTUlL5A2TNwoOV74dskQRSJ4ZGzDKmd8Qz", RegionEndpoint.APSoutheast1);

var sqsQueryResponse = await sqsClient.GetQueueUrlAsync("email-queue");

var customer = new CustomerCreated
{
    DateOfBirth = new DateTime(1999, 1, 24),
    Email = "hoa@yopmail.com",
    GithubUsername = "admin",
    Id = Guid.NewGuid(),
    Name = "Hoa Le"
};

var messageRequest = new SendMessageRequest
{
    QueueUrl = sqsQueryResponse?.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
    {
        {
            "MessageType",
            new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

var result = await sqsClient.SendMessageAsync(messageRequest);

Console.WriteLine(result);