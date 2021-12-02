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
       private const string QueueUrl = "https://sqs.us-east-1.amazonaws.com/852704159394/drcash-logs-api";

        static async Task Main(string[] args)
        {
            var identity = new Identity
            {
                Service = new Service {
                    Name = "Notifications",
                    Microservice = new Microservice {
                        Name="User"
                        }
                },
                Action = "Update",
                Type = "Information",
                ApiRoute = "https://localhost:5001/api/v1/identity",
                DataSent = "Data object sent",
                ObjectId = "2253253",
                OldData = "2423",
                Description = "Creating a new user",
                User = new User{
                    UserName = "henrique",
                    Email = "henrique@gmail.com"
                }
            };

            var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
            var message = JsonSerializer.Serialize(identity);
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
