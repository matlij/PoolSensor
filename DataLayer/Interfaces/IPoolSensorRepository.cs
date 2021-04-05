using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IPoolSensorRepository
    {
        Task CreateOrUpdate(PoolDataEntity entity);
        IEnumerable<PoolDataEntity> Get(string deviceid, DateTimeOffset? fromDate = null);
    }
}