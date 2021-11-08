using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Data;


namespace SQS.Fornecedor
{
    class Fornecedor
    {
       private const string QueueUrl = "https://sqs.us-east-1.amazonaws.com/852704159394/drcash-logs-api-test";

        static async Task Main(string[] args)
        {
            var user = new Users
            {
                Id = 1,
                Nome = "Dercio",
                Email = "derciosinione@gmail.com",
                Idade = 21
            };

            var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
            var message = JsonSerializer.Serialize(user);
            await SendMessageAsync(sqsClient, QueueUrl, message);
        }

        private static async Task SendMessageAsync(IAmazonSQS sqsClient, string qUrl, string messageBody)
        {
            var request = new SendMessageRequest
            {
                QueueUrl = qUrl,
                MessageBody = messageBody
            };
            
            var response = await sqsClient.SendMessageAsync(request);

            if(response.HttpStatusCode == HttpStatusCode.OK)
                Console.WriteLine("Mensagem enviada com sucesso!!!");
        }
    }
}