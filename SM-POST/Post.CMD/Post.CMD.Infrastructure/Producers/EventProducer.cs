using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;

namespace Post.CMD.Infrastructure.Producers
{
    public class EventProducer : IEventProducer
    {
        private readonly ProducerConfig _config;

        private EventProducer(IOptions<ProducerConfig> config)
        {
            _config = config.Value;

            var testConfig = new ProducerConfig{
                BootstrapServers = "localhost:9092",
            };
        }

        public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
        {
            using var producer = new ProducerBuilder<string, string>(_config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

            var eventMessage = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };

            var result = await producer.ProduceAsync(topic, eventMessage);

            if (result.Status != PersistenceStatus.Persisted)
            {
                throw new Exception($"Could not produce event {@event.GetType().Name} to topic {topic}  - {result.Message} - {result.Status}");
            }
        }
    }
}