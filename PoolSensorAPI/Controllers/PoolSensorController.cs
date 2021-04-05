using DataLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PoolSensorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoolSensorController : ControllerBase
    {
        private readonly ILogger<PoolSensorController> _logger;
        private readonly IPoolSensorRepository _poolSensorRepository;

        public PoolSensorController(ILogger<PoolSensorController> logger, IPoolSensorRepository poolSensorRepository)
        {
            _logger = logger;
            _poolSensorRepository = poolSensorRepository;
        }

        // GET: api/<PoolSensorController>
        [HttpGet("{deviceid}")]
        public IEnumerable<PoolSensorData> Get(string deviceid, [FromQuery(Name = "fromDate")] DateTime? fromDate = null)
        {
            var data = _poolSensorRepository.Get(deviceid, fromDate);

            return data.Select(d => new PoolSensorData
            {
                DeviceId = d.PartitionKey,
                Temperature = d.AvgTemperature,
                TimeStamp = new DateTime(long.Parse(d.RowKey))
            });
        }

        // POST api/<PoolSensorController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PoolSensorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PoolSensorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
