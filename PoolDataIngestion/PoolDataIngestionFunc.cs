using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace PoolDataIngestion
{
    public class PoolDataIngestionFunc
    {
        private readonly IPoolSensorRepository _poolSensorRepository;

        public PoolDataIngestionFunc(IPoolSensorRepository poolSensorRepository)
        {
            _poolSensorRepository = poolSensorRepository;
        }

        [FunctionName("PoolDataIngestion")]
        public async Task Run([IoTHubTrigger("messages/events", Connection = "IoTHubConnectionString")] EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");

            var poolSensorData = JsonSerializer.Deserialize<PoolSensorData>(message.Body.Array);
            if (!message.SystemProperties.TryGetValue("iothub-connection-device-id", out var deviceId))
            {
                throw new InvalidOperationException("Failed to get DeviceID");
            }

            var poolData = new PoolDataEntity
            {
                AvgTemperature = poolSensorData.Temperature,
                PartitionKey = deviceId.ToString(),
                RowKey = poolSensorData.TimeStamp.Ticks.ToString(),
                Timestamp = poolSensorData.TimeStamp
            };

            await _poolSensorRepository.CreateOrUpdate(poolData);
        }
    }
}