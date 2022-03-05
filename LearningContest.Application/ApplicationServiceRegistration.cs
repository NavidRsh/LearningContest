using LearningContest.Application.Contracts;
using LearningContest.Application.Contracts.HttpCall;
using LearningContest.Application.Contracts.Infrastructure;
using LearningContest.Application.Extension;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;
using CommonInfrastructure.Kafka.Service;
using LearningContest.Application.Contracts.Services;
using CommonInfrastructure.Kafka;
using CommonInfrastructure.CancellationToken;
using CommonInfrastructure.Encryption;

namespace LearningContest.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
                  
            services.AddScoped<Application.Extension.ILogger, Serilogger>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{configuration.GetValue<string>("Redis:Server")}:{configuration.GetValue<int>("Redis:Port")},defaultDatabase={configuration.GetValue<int>("Redis:Db")}";
            });
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddSingleton<IEncryptionService, EncryptionService>();
            return services;
        }

        public static IServiceCollection AddKafkaServices(this IServiceCollection services, string kafkaUrl)
        {
            //ProducerServices 
            var clientConfig = new ClientConfig
            {
                BootstrapServers = kafkaUrl,
                //SaslUsername = "admin"
            }; 
            var producerConfig = new ProducerConfig(clientConfig);
            services.AddSingleton(producerConfig);
            services.AddSingleton(typeof(IKafkaProducerService<,>), typeof(KafkaProducerService<,>));
            //ConsumerServices 
            var consumerConfig = new ConsumerConfig(clientConfig)
            {
                GroupId = "SourceApp",
                EnableAutoCommit = true,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000
            };                        
            services.AddSingleton(consumerConfig);
            services.AddScoped<IKafkaHandlerService<string, KafkaMessage<string>>, KafkaTestHandlerService>();
            services.AddSingleton(typeof(IKafkaConsumerService<,>), typeof(KafkaConsumerService<,>));
            //services.AddHostedService<KafkaConsumerHostedService>();


            return services; 
        }

        
    }
}
