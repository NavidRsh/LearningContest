using CommonInfrastructure.Kafka;
using CommonInfrastructure.Kafka.Service;
using Confluent.Kafka;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using LearningContest.Application.Contracts.Dto.MarketSocket;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace LearningContest.Application.Contracts.Services
{
    public class KafkaConsumerHostedService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ConsumerConfig _consumerConfig;
        private readonly string[] _topics;
        private readonly double _maxNumAttempts;
        private readonly double _retryIntervalInSec;
        private readonly IKafkaConsumerService<string, KafkaMessage<string>> _consumer;
        public KafkaConsumerHostedService(IServiceScopeFactory scopeFactory, IConfiguration configuration, IKafkaConsumerService<string, KafkaMessage<string>> consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string topicName = "testTopic"; 
            try
            {
                await _consumer.Consume(topicName, stoppingToken);                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{(int)HttpStatusCode.InternalServerError} ConsumeFailedOnTopic {topicName}, {ex}");
            }
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();

            base.Dispose();
        }


    }
}
