using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Data;
using System.Text.Json;

namespace SQS.Consumidor
{
    class Consumidor
    {
        private const string QueeueUrl = "https://sqs.us-east-1.amazonaws.com/852704159394/drcash-logs-api-test";

        static async Task Main(string[] args)
        {
            var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
            
            const int messageWaitTime = 15;

            var response = await ReciveMessageAsync(sqsClient, QueeueUrl, messageWaitTime);

            var users = await ReadMessageAsync(sqsClient, response);
            
            VerifyIfThereIsRecivedMessage(users, response.Messages.Any());
        }

        private static async Task<ReceiveMessageResponse> ReciveMessageAsync(IAmazonSQS sqsClient, string qUrl, int waitTime=0)
        {
            const int maxMessages = 5;
            var request = new ReceiveMessageRequest
            {
                QueueUrl = qUrl,
                WaitTimeSeconds = waitTime,
                MaxNumberOfMessages = maxMessages
            };
            
            return await sqsClient.ReceiveMessageAsync(request);
        }

        private static async Task<IEnumerable<Users>> ReadMessageAsync(IAmazonSQS sqsClient, ReceiveMessageResponse response)
        {
            List<Users> usersList = new List<Users>();
            
            if (response.Messages.Any())
            {
                foreach (var message in response.Messages)
                { 
                    var user = JsonSerializer.Deserialize<Users>(message.Body);
                    Console.WriteLine($"Recived username: {user.Nome}");
                    usersList.Add(user);
                    await DeleteMessageAsync(sqsClient, QueeueUrl, message);
                }
            }
            return usersList;
        }
        
        private static async Task DeleteMessageAsync(IAmazonSQS sqsClient, string qUrl, Message message)
        {
            await sqsClient.DeleteMessageAsync(qUrl, message.ReceiptHandle);
        }
        
        private static void VerifyIfThereIsRecivedMessage(IEnumerable<Users> users, bool hasMessage)
        {
            if (hasMessage)
            { 
                Console.WriteLine($"Recived {users.Count()} users"); 
                Console.WriteLine("Mensagem recebida com sucesso!!!");
            }
        }
    }
}