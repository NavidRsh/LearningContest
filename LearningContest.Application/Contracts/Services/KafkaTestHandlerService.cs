using CommonInfrastructure.Kafka;
using CommonInfrastructure.Kafka.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Services
{
    public class KafkaTestHandlerService : IKafkaHandlerService<string, KafkaMessage<string>>
    {
        private readonly IKafkaProducerService<string, KafkaMessage<string>> _producer;

        public KafkaTestHandlerService()
        {
            //_producer = producer;
        }
        public Task HandleAsync(string key, KafkaMessage<string> value)
        {
            // Here we can actually write the code to register a User
            Console.WriteLine($"Consuming UserRegistered topic message with the below data\n FirstName:"); // {value.FirstName}\n LastName: {value.LastName}\n UserName: {value.UserName}\n EmailId: {value.EmailId}");




            //After successful operation, suppose if the registered user has User Id as 1 the we can produce message for other service's consumption

            return Task.CompletedTask;
        }
    }
}
