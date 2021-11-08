using System;
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
        static async Task Main(string[] args)
        {
            var user = new Users
            {
                Id = 1,
                Nome = "Dercio",
                Email = "derciosinione@gmail.com",
                Idade = 21
            };

            var convertedObject = JsonSerializer.Serialize(user);
            
            var client = new AmazonSQSClient(RegionEndpoint.USEast1);
            var request = new SendMessageRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/852704159394/drcash-logs-api-test",
                MessageBody = convertedObject,
            };
            
            await client.SendMessageAsync(request);
            
            Console.WriteLine("Mensagem enviada com sucesso!!!");
        }
    }
}