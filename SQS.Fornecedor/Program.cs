using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;


namespace SQS.Fornecedor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new AmazonSQSClient(RegionEndpoint.USEast1);
            var request = new SendMessageRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/852704159394/drcash-logs-api-test",
                MessageBody = "Testando Envio de mensagem na fila",
            };
            await client.SendMessageAsync(request);
        }
    }
}