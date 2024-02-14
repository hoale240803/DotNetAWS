using Amazon.SQS.Model;
using Amazon.SQS;
using SQSPublisher;
using System.Text.Json;
using Amazon;


var cts = new CancellationTokenSource();

var sqsClient = new AmazonSQSClient("AKIAY3U6AD3IO6Z53DU2", "mhVhi9uTUlL5A2TNwoOV74dskQRSJ4ZGzDKmd8Qz", RegionEndpoint.APSoutheast1);

var sqsQueryResponse = await sqsClient.GetQueueUrlAsync("email-queue");


var messageRequest = new ReceiveMessageRequest
{
    QueueUrl = sqsQueryResponse?.QueueUrl,
};



while (!cts.IsCancellationRequested)
{
    var response = await sqsClient.ReceiveMessageAsync(messageRequest);

    foreach (var message in response.Messages)
    {
        Console.WriteLine($"Message Id: {message.MessageId}");
        Console.WriteLine($"Message Body: {message.Body}");

        await sqsClient.DeleteMessageAsync(sqsQueryResponse?.QueueUrl, message.ReceiptHandle);
    }

    await Task.Delay(2000);
}

