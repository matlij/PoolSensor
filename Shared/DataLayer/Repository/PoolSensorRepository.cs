using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{

    public class PoolSensorRepository : IPoolSensorRepository
    {
        private const string TableName = "pooldata";
        private readonly ITableStorage _tableStorage;

        public PoolSensorRepository(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<PoolDataEntity>> Get(string deviceid, DateTimeOffset? fromDate = null)
        {
            var deviceFilter = TableQuery.GenerateFilterCondition(
                nameof(PoolDataEntity.PartitionKey), 
                QueryComparisons.Equal, 
                deviceid);

            var fromDateFilter = TableQuery.GenerateFilterCondition(
                nameof(PoolDataEntity.RowKey), 
                QueryComparisons.GreaterThanOrEqual, 
                fromDate?.Ticks.ToString() ?? DateTimeOffset.MinValue.Ticks.ToString());

            var filter = TableQuery.CombineFilters(deviceFilter, TableOperators.And, fromDateFilter);

            return await _tableStorage.GetAsync<PoolDataEntity>(TableName, filter);
        }

        public async Task CreateOrUpdate(PoolDataEntity entity)
        {
            await _tableStorage.AddOrUpdateAsync(TableName, entity);
        }
    }
}
