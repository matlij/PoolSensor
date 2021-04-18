using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ITableStorage
    {
        Task<object> AddOrUpdateAsync<T>(string tableName, T entity) where T : ITableEntity, new();
        Task<IEnumerable<T>> GetAsync<T>(string tableName, string filterCondition) where T : ITableEntity, new();
    }
}
