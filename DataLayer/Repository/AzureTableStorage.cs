using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class AzureTableStorage : ITableStorage
    {
        private readonly CloudTableClient _tableClient;
        private readonly Dictionary<string, CloudTable> _tables;

        public AzureTableStorage(IMyAppSettings appSettings)
        {
            var storageAccount = CloudStorageAccount.Parse(appSettings.AzureFileConnectionString);

            _tableClient = storageAccount.CreateCloudTableClient();

            _tables = new Dictionary<string, CloudTable>();
        }

        public async Task<IEnumerable<T>> GetAsync<T>(string tableName, string filterCondition) where T : ITableEntity, new()
        {
            var cloudTable = await TryGetTable(tableName);

            var query = new TableQuery<T>().Where(filterCondition);

            var result = cloudTable.ExecuteQuery(query);

            return result;
        }

        public async Task<object> AddOrUpdateAsync<T>(string tableName, T entity) where T : ITableEntity, new()
        {
            var cloudTable = await TryGetTable(tableName);

            var operation = TableOperation.InsertOrReplace(entity);

            var result = await cloudTable.ExecuteAsync(operation);

            return result.Result;
        }

        private async Task<CloudTable> TryGetTable(string tableName)
        {
            if (!_tables.TryGetValue(tableName, out var cloudTable))
            {
                cloudTable = _tableClient.GetTableReference(tableName);
                await cloudTable.CreateIfNotExistsAsync();
                _tables.Add(tableName, cloudTable);
            }

            return cloudTable;
        }
    }
}
