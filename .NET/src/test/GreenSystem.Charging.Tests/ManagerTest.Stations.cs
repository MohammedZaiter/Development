
namespace GreenSystem.Charging.Tests
{
    using GreenSystem.Charging.Abstractions;
    using GreenSystem.Charging.Groups;
    using Moq;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public sealed partial class ManagerTest
    {
        [Fact]
        public void GetStationByIdMustReturnStationType()
        {
            var guid = Guid.NewGuid();

            var actualStation = new Station()
            {
                Id = guid,
                Name = "station1"
            };

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid))
                    .ReturnsAsync(actualStation);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var station = manager.GetStation(guid).Result;

            Assert.True(station.Equals(actualStation));
        }

        [Fact]
        public void GetStationsByGroupIdMustSucceeded()
        {
            var guid = Guid.NewGuid();

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStationsByGroupId(guid))
                    .ReturnsAsync(new List<Station>());

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.GetGroup(guid))
                    .ReturnsAsync(new Group());

            var manager = new Manager(mockGraphStore.Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.GetStationsByGroupId(guid).Result;

            Assert.True(result.Success);
        }

        [Fact]
        public void GetStationsByGroupIdMustFailIfGroupNotFound()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.GetGroup(guid))
                    .ReturnsAsync((Group)null);

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.GetStationsByGroupId(guid).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void CreateStationMustThrowExceptionIfOptionsIsNull()
        {
            var manager = new Manager(new Mock<IGroupsStore>().Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            Assert.ThrowsAsync<ArgumentNullException>(() => manager.CreateStation(null));
        }

        [Fact]
        public void CreateStationMustFailIfGroupNotFound()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.GetGroup(guid))
                    .ReturnsAsync((Group)null);

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.CreateStation(new CreateOrUpdateStationOptions()).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void CreateStationMustSucceeded()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateStationOptions()
            {
                GroupId = guid
            };

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.GetGroup(guid))
                    .ReturnsAsync(new Group());

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.CreateStation(options).Result;

            Assert.True(result.Success);
        }

        [Fact]
        public void UpdateStationMustThrowExceptionIfOptionsIsNull()
        {
            var manager = new Manager(new Mock<IGroupsStore>().Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            Assert.ThrowsAsync<ArgumentNullException>(() => manager.UpdateStation(It.IsAny<Guid>(), null));
        }

        [Fact]
        public void UpdateStationMustFailIfStationNotFound()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateStationOptions()
            {
                GroupId = guid
            };

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid))
                    .ReturnsAsync((Station)null);

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.UpdateStation(It.IsAny<Guid>(), options).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void UpdateStationMustFailIfGroupNotFound()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.GetGroup(guid))
                    .ReturnsAsync((Group)null);

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid))
                    .ReturnsAsync(new Station());

            var manager = new Manager(mockGraphStore.Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.UpdateStation(It.IsAny<Guid>(), new CreateOrUpdateStationOptions()).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void UpdateStationShouldSucceeded()
        {
            var guid = Guid.NewGuid();

            var options = new CreateOrUpdateStationOptions()
            {
                GroupId = guid
            };

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.GetGroup(guid))
                    .ReturnsAsync(new Group());

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid))
                    .ReturnsAsync(new Station());

            var manager = new Manager(mockGraphStore.Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.UpdateStation(guid, options).Result;

            Assert.True(result.Success);
        }

        [Fact]
        public void RemoveStationMustFailIfGroupNotFound()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.GetGroup(guid))
                    .ReturnsAsync((Group)null);

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.RemoveStation(guid).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void RemoveStationShouldSucceeded()
        {
            var guid = Guid.NewGuid();

            var mockStationsStore = new Mock<IStationsStore>();
            mockStationsStore.Setup(e => e.GetStation(guid))
                    .ReturnsAsync(new Station());

            var manager = new Manager(new Mock<IGroupsStore>().Object, mockStationsStore.Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.RemoveStation(guid).Result;

            Assert.True(result.Success);
        }
    }
}
