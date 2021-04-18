using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Repository;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DataLayerTests
{
    public class PoolSensorRepositoryTest
    {
        private Mock<ITableStorage> _tableStorageMock;
        private IPoolSensorRepository _sut;

        [SetUp]
        public void Setup()
        {
            _tableStorageMock = new Mock<ITableStorage>();
            _sut = new PoolSensorRepository(_tableStorageMock.Object);
        }

        [Test]
        public async Task PoolSensorRepository_Get_ShouldCallTableStorage()
        {
            var result = await _sut.Get("TestDevice");

            _tableStorageMock.Verify(x => x.GetAsync<PoolDataEntity>("pooldata", It.IsAny<string>()));
        }
    }
}