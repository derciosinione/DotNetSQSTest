using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SQS.Consumidor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string queeueUrl = "https://sqs.us-east-1.amazonaws.com/852704159394/drcash-logs-api-test";
            
            var client = new AmazonSQSClient(RegionEndpoint.USEast1);
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queeueUrl,
                // WaitTimeSeconds = 20
            };
            
            var response = await client.ReceiveMessageAsync(request);

            foreach (var message in response.Messages)
            { 
                Console.WriteLine(message.Body);
                // await client.DeleteMessageAsync(queeueUrl, message.ReceiptHandle);
            }
            
            Console.WriteLine("Mensagem recebida com sucesso!!!");

        }
    }
}