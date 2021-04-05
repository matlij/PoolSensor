using Microsoft.Azure.Cosmos.Table;

namespace DataLayer.Models
{
    public class PoolDataEntity : TableEntity
    {
        public double AvgTemperature { get; set; }
    }
}
