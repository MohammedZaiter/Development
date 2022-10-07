
namespace GreenFlux.Charging.Tests
{
    using GreenFlux.Charging.Groups;
    using Moq;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public sealed partial class ManagerTest
    {
        [Fact]
        public void GetConnectorByIdMustThrowExceptionIfStationNotFound()
        {
            var guid = Guid.NewGuid();

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync((Station)null);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object);

            var result = manager.GetConnectorById(guid, It.IsAny<int>()).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void GetConnectorByIdShouldSucceeded()
        {
            var guid = Guid.NewGuid();

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync(new Station());

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object);

            var result = manager.GetConnectorById(guid, It.IsAny<int>()).Result;

            Assert.True(result.Success);
        }

        [Fact]
        public void GetConnectorsByStationIdIdMustThrowExceptionIfStationNotFound()
        {
            var guid = Guid.NewGuid();

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync((Station)null);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object);

            var result = manager.GetConnectorsByStationId(guid).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void GetConnectorsByStationIdShouldSucceeded()
        {
            var guid = Guid.NewGuid();

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync(new Station());

            var mocConnectorsStore = new Mock<IConnectorsStore>();
            mocConnectorsStore.Setup(e => e.GetConnectorsByStationId(guid)).ReturnsAsync(new List<Connector>());

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, mocConnectorsStore.Object);

            var result = manager.GetConnectorsByStationId(guid).Result;

            Assert.True(result.Success);
        }

        [Fact]
        public void CreateConnectorMustThrowExceptionIfOptionsIsNull()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object);

            Assert.ThrowsAsync<ArgumentNullException>(() => manager.CreateConnector(null));
        }

        [Fact]
        public void CreateConnectorsMustFailIfStationIsNotFound()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateConnectorOptions()
            {
                StationId = guid,
                MaxCurrent = 10
            };

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync((Station)null);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object);

            var result = manager.CreateConnector(options).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void CreateConnectorsMustFailIfSectionConnectorsIsMoreThanFive()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateConnectorOptions()
            {
                StationId = guid,
                MaxCurrent = 10
            };

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync((Station)null);

            var mocConnectorsStore = new Mock<IConnectorsStore>();
            mocConnectorsStore.Setup(e => e.GetConnectorsByStationId(guid)).ReturnsAsync(new List<Connector>()
            {
                new Connector(),
                new Connector(),
                new Connector(),
                new Connector(),
                new Connector(),
                new Connector()
            });

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, mocConnectorsStore.Object);

            var result = manager.CreateConnector(options).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void CreateConnectorsMustFailIfNoEnoughGroupCapacity()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateConnectorOptions()
            {
                StationId = guid,
                MaxCurrent = 2
            };

            var mockGroupsStore = new Mock<IGroupsStore>();
            mockGroupsStore.Setup(e => e.GetGroup(guid)).ReturnsAsync(new Group()
            {
                Id = Guid.NewGuid(),
                Capacity = 10,
                ConsumedCapacity = 10,
                Name = "group1"
            });

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync(new Station() { GroupId = guid });

            var mocConnectorsStore = new Mock<IConnectorsStore>();
            mocConnectorsStore.Setup(e => e.GetConnectorsByStationId(guid)).ReturnsAsync(new List<Connector>());

            var manager = new Manager(mockGroupsStore.Object, mockStationsStore.Object, mocConnectorsStore.Object);

            var result = manager.CreateConnector(options).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void CreateConnectorsShouldSucceeded()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateConnectorOptions()
            {
                StationId = guid,
                MaxCurrent = 2
            };

            var mockGroupsStore = new Mock<IGroupsStore>();
            mockGroupsStore.Setup(e => e.GetGroup(guid)).ReturnsAsync(new Group()
            {
                Id = Guid.NewGuid(),
                Capacity = 10,
                ConsumedCapacity = 8,
                Name = "group1"
            });

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync(new Station() { GroupId = guid });

            var mocConnectorsStore = new Mock<IConnectorsStore>();
            mocConnectorsStore.Setup(e => e.GetConnectorsByStationId(guid)).ReturnsAsync(new List<Connector>());

            var manager = new Manager(mockGroupsStore.Object, mockStationsStore.Object, mocConnectorsStore.Object);

            var result = manager.CreateConnector(options).Result;

            Assert.True(result.Success);
        }

        [Fact]
        public void UpdateConnectorMustThrowExceptionIfOptionsIsNull()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object);

            Assert.ThrowsAsync<ArgumentNullException>(() => manager.UpdateConnector(It.IsAny<int>(), null));
        }

        [Fact]
        public void UpdateConnectorsMustFailIfIdLessThanOne()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateConnectorOptions()
            {
                StationId = guid,
                MaxCurrent = 10
            };

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync((Station)null);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object);

            var result = manager.UpdateConnector(0, options).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void UpdateConnectorsMustFailIfStationIsNotFound()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateConnectorOptions()
            {
                StationId = guid,
                MaxCurrent = 10
            };

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync((Station)null);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object);

            var result = manager.UpdateConnector(1, options).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void UpdateConnectorsMustFailIfConnectorIsNotFound()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateConnectorOptions()
            {
                StationId = guid,
                MaxCurrent = 10
            };

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync(new Station());

            var mockConnectorsStore = new Mock<IConnectorsStore>();
            mockConnectorsStore.Setup(e => e.GetConnector(guid, 1)).ReturnsAsync((Connector)null);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, mockConnectorsStore.Object);

            var result = manager.UpdateConnector(1, options).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void UpdateConnectorsMustFailIfNoEnoughGroupCapacity()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateConnectorOptions()
            {
                StationId = guid,
                MaxCurrent = 4
            };

            var mockGroupsStore = new Mock<IGroupsStore>();
            mockGroupsStore.Setup(e => e.GetGroup(guid)).ReturnsAsync(new Group()
            {
                Id = Guid.NewGuid(),
                Capacity = 10,
                ConsumedCapacity = 10,
                Name = "group1"
            });

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync(new Station() { GroupId = guid });

            var mockConnectorsStore = new Mock<IConnectorsStore>();
            mockConnectorsStore.Setup(e => e.GetConnector(guid, 1)).ReturnsAsync(new Connector()
            {
                Id = 1,
                StationId = guid,
                MaxCurrent = 2
            });

            var manager = new Manager(mockGroupsStore.Object, mockStationsStore.Object, mockConnectorsStore.Object);

            var result = manager.UpdateConnector(1, options).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void UpdateConnectorsShouldSucceeded()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateConnectorOptions()
            {
                StationId = guid,
                MaxCurrent = 1
            };

            var mockGroupsStore = new Mock<IGroupsStore>();
            mockGroupsStore.Setup(e => e.GetGroup(guid)).ReturnsAsync(new Group()
            {
                Id = Guid.NewGuid(),
                Capacity = 10,
                ConsumedCapacity = 10,
                Name = "group1"
            });

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync(new Station() { GroupId = guid });

            var mockConnectorsStore = new Mock<IConnectorsStore>();
            mockConnectorsStore.Setup(e => e.GetConnector(guid, 1)).ReturnsAsync(new Connector()
            {
                Id = 1,
                StationId = guid,
                MaxCurrent = 2
            });

            var manager = new Manager(mockGroupsStore.Object, mockStationsStore.Object, mockConnectorsStore.Object);

            var result = manager.UpdateConnector(1, options).Result;

            Assert.True(result.Success);
        }

        [Fact]
        public void RemoveConnectorsMustFailIfIdLessThanOne()
        {
            var guid = Guid.NewGuid();

            var manager = new Manager(new Mock<IGroupsStore>().Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object);

            var result = manager.RemoveConnector(guid, 0).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void RemoveConnectorsMustFailIfStationIsNotFound()
        {
            var guid = Guid.NewGuid();

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync((Station)null);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object);

            var result = manager.RemoveConnector(guid, 1).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void RemoveConnectorsMustFailIfConnectorIsNotFound()
        {
            var guid = Guid.NewGuid();

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync(new Station());

            var mockConnectorsStore = new Mock<IConnectorsStore>();
            mockConnectorsStore.Setup(e => e.GetConnector(guid, 1)).ReturnsAsync((Connector)null);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, mockConnectorsStore.Object);

            var result = manager.RemoveConnector(guid, 1).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void RemoveConnectorsShoulSucceeded()
        {
            var guid = Guid.NewGuid();

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid)).ReturnsAsync(new Station());

            var mockConnectorsStore = new Mock<IConnectorsStore>();
            mockConnectorsStore.Setup(e => e.GetConnector(guid, 1)).ReturnsAsync(new Connector());

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, mockConnectorsStore.Object);

            var result = manager.RemoveConnector(guid, 1).Result;

            Assert.True(result.Success);
        }
    }
}
