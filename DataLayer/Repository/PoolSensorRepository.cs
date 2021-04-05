using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class PoolSensorRepository : IPoolSensorRepository
    {
        private readonly CloudTable _poolDataTable;

        public PoolSensorRepository(IMyAppSettings appSettings)
        {
            var storageAccount = CloudStorageAccount.Parse(appSettings.AzureFileConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            _poolDataTable = tableClient.GetTableReference("pooldata");
        }

        public IEnumerable<PoolDataEntity> Get(string deviceid, DateTimeOffset? fromDate = null)
        {
            var deviceFilter = TableQuery.GenerateFilterCondition(nameof(PoolDataEntity.PartitionKey), QueryComparisons.Equal, deviceid);
            var fromDateFilter = TableQuery.GenerateFilterCondition(nameof(PoolDataEntity.RowKey), QueryComparisons.GreaterThanOrEqual, fromDate?.Ticks.ToString() ?? DateTimeOffset.MinValue.Ticks.ToString());
            var filter = TableQuery.CombineFilters(deviceFilter, TableOperators.And, fromDateFilter);

            var query = new TableQuery<PoolDataEntity>()
                .Where(filter);

            var result = _poolDataTable.ExecuteQuery(query);

            return result;
        }

        public async Task CreateOrUpdate(PoolDataEntity entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);

            await _poolDataTable.ExecuteAsync(operation);
        }
    }
}
