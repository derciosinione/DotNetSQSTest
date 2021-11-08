using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Data;
using System.Text.Json;

namespace SQS.Consumidor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string QueueAmazonawsUrl = "https://sqs.us-east-1.amazonaws.com/852704159394/drcash-logs-api-test";
            const int WaitTimeSecondsforRecived = 15;

            var client = new AmazonSQSClient(RegionEndpoint.USEast1);
            var request = new ReceiveMessageRequest
            {
                QueueUrl = QueueAmazonawsUrl,
                WaitTimeSeconds = WaitTimeSecondsforRecived
            };

            var response = await client.ReceiveMessageAsync(request);
            var user = new Users();

            foreach (var message in response.Messages)
            {
                Console.WriteLine(message.Body);

                user = JsonSerializer.Deserialize<Users>(message.Body);

                await client.DeleteMessageAsync(queeueUrl, message.ReceiptHandle);
            }

            this.ReceiveMessageUser(response.Messages.Count, user);

        }

        public void ReceiveMessageUser(int countMessage,Users user){
            if (countMessaget > 0)
            {
                Console.WriteLine($"Recived user {user.Nome} with email: {user.Email}");
                Console.WriteLine("Mensagem recebida com sucesso!!!");
            }
        }

    }
}