using System;

namespace Models
{
    public class PoolSensorData
    {
        public string DeviceId { get; set; }
        public double Temperature { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
