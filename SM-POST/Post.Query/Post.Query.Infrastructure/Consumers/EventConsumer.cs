using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;

namespace Post.Query.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _config;
        private readonly IEventHandler _eventHandler;

        public EventConsumer(IOptions<ConsumerConfig> config, IEventHandler eventHandler)
        {
            _config = config.Value;
            _eventHandler = eventHandler;
        }

        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(_config)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

            consumer.Subscribe(topic);

            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume();

                    if (consumeResult?.Message == null)
                    {
                        continue;
                    }

                    var options = new JsonSerializerOptions
                    {
                        Converters = { new EventJsonConverter() }
                    };

                    var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);

                    var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });

                    if (handlerMethod == null)
                    {
                        throw new ArgumentNullException($"Handler method for event {nameof(handlerMethod)} not found");
                    }

                    handlerMethod.Invoke(_eventHandler, new object[] { @event });
                    consumer.Commit();
                }
                catch (ConsumeException e)
                {
                    // Log the error and decide whether to continue or not
                    Console.WriteLine($"Error consuming message: {e.Error.Reason}");
                }
            }
        }
    }
}