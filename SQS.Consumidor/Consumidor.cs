﻿using System;
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
        private const string QueueUrl = "https://sqs.us-east-1.amazonaws.com/852704159394/drcash-logs-api-test";

        static async Task Main(string[] args)
        {
            var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
            
            const int messageWaitTime = 15;

            var response = await ReciveMessageAsync(sqsClient, QueueUrl, messageWaitTime);

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

        private static async Task<IEnumerable<Identity>> ReadMessageAsync(IAmazonSQS sqsClient, ReceiveMessageResponse response)
        {
            List<Identity> identityList = new List<Identity>();
            
            if (response.Messages.Any())
            {
                foreach (var message in response.Messages)
                { 
                    var identity = JsonSerializer.Deserialize<Identity>(message.Body);
                    Console.WriteLine($"Recived username: {identity?.Service.Name}");
                    identityList.Add(identity);
                    await DeleteMessageAsync(sqsClient, QueueUrl, message);
                }
            }
            return identityList;
        }
        
        private static async Task DeleteMessageAsync(IAmazonSQS sqsClient, string qUrl, Message message)
        {
            await sqsClient.DeleteMessageAsync(qUrl, message.ReceiptHandle);
        }
        
        private static void VerifyIfThereIsRecivedMessage(IEnumerable<Identity> identity, bool hasMessage)
        {
            if (hasMessage)
            { 
                Console.WriteLine($"Recived {identity.Count()} users"); 
                Console.WriteLine("Mensagem recebida com sucesso!!!");
            }
        }
    }
}