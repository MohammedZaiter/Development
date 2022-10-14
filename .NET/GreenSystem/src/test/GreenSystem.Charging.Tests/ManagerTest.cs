
namespace GreenSystem.Charging.Tests
{
    using GreenSystem.Charging.Abstractions;
    using GreenSystem.Charging.Groups;
    using Moq;
    using System;
    using Xunit;

    public sealed partial class ManagerTest
    {
        [Fact]
        public void ConstructorMustThrowExceptionIfGroupsStoreIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Manager(null, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object));
        }

        [Fact]
        public void ConstructorMustThrowExceptionIfStationsStoreIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Manager(new Mock<IGroupsStore>().Object, null, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object));
        }

        [Fact]
        public void ConstructorMustThrowExceptionIfConnectorsStoreIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Manager(new Mock<IGroupsStore>().Object, new Mock<IStationsStore>().Object, null, new Mock<ICachingService>().Object));
        }

        [Fact]
        public void GetGroupByIdMustReturnGroupType()
        {
            var guid = Guid.NewGuid();

            var actualGroup = new Group()
            {
                Id = guid,
                Name = "group1",
                Capacity = 10
            };

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.GetGroup(guid))
                    .ReturnsAsync(actualGroup);

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var group = manager.GetGroupById(guid).Result.Result;

            Assert.True(group.Equals(actualGroup));
        }

        [Fact]
        public void CreateGroupMustThrowExceptionIfOptionsIsNull()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            Assert.ThrowsAsync<ArgumentNullException>(() => manager.CreateGroup(null));
        }

        [Fact]
        public void CreateGroupShouldBeSucceeded()
        {
            var createOrUpdateOptions = new CreateOrUpdateGroupOptions()
            {
                Capacity = 10,
                Name = "group1"
            };

            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.CreateGroup(createOrUpdateOptions))
                    .ReturnsAsync(guid);

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var groupId = manager.CreateGroup(createOrUpdateOptions).Result;

            Assert.Equal(guid, groupId);
        }

        [Fact]
        public void UpdateGroupMustThrowExceptionIfOptionsIsNull()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            Assert.ThrowsAsync<ArgumentNullException>(() => manager.UpdateGroup(guid, null));
        }

        [Fact]
        public void UpdateGroupMustFailIfGroupNotFound()
        {
            var createOrUpdateOptions = new CreateOrUpdateGroupOptions()
            {
                Capacity = 10,
                Name = "group1"
            };

            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.GetGroup(guid)).ReturnsAsync((Group)null);

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.UpdateGroup(guid, createOrUpdateOptions).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void UpdateGroupShouldBeSucceeded()
        {
            var createOrUpdateOptions = new CreateOrUpdateGroupOptions()
            {
                Capacity = 10,
                Name = "group1"
            };

            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.UpdateGroup(guid, createOrUpdateOptions));
            mockGraphStore.Setup(e => e.GetGroup(guid)).ReturnsAsync(new Group());

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.UpdateGroup(guid, createOrUpdateOptions).Result;

            Assert.True(result.Success);
        }

        [Fact]
        public void RemoveGroupShouldBeFailIfGroupNotFound()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.RemoveGroup(guid));
            mockGraphStore.Setup(e => e.GetGroup(guid)).ReturnsAsync((Group)null);

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.RemoveGroup(guid).Result;

            Assert.False(result.Success);
        }

        [Fact]
        public void RemoveGroupShouldBeSucceeded()
        {
            var guid = Guid.NewGuid();

            var mockGraphStore = new Mock<IGroupsStore>();
            mockGraphStore.Setup(e => e.RemoveGroup(guid));
            mockGraphStore.Setup(e => e.GetGroup(guid)).ReturnsAsync(new Group());

            var manager = new Manager(mockGraphStore.Object, new Mock<IStationsStore>().Object, new Mock<IConnectorsStore>().Object, new Mock<ICachingService>().Object);

            var result = manager.RemoveGroup(guid).Result;

            Assert.True(result.Success);
        }
    }
}