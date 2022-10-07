﻿
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Threading.Tasks;

    public sealed partial class Manager : IGroupsManager
    {
        private readonly IGroupsStore groupsStore;
        private readonly IStationsStore stationsStore;
        private readonly IConnectorsStore connectorsStore;

        public Manager(
            IGroupsStore groupsStore,
            IStationsStore stationsStore,
            IConnectorsStore connectorsStore)
        {
            this.groupsStore = groupsStore ?? throw new ArgumentNullException(nameof(groupsStore));
            this.connectorsStore = connectorsStore ?? throw new ArgumentNullException(nameof(stationsStore));
            this.stationsStore = stationsStore ?? throw new ArgumentNullException(nameof(stationsStore));
        }

        public async Task<ReturnResult<Group>> GetGroupById(Guid id)
        {
            return ReturnResult<Group>.SuccessResult(await this.groupsStore.GetGroup(id));
        }

        public Task<Guid> CreateGroup(CreateOrUpdateGroupOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return this.groupsStore.CreateGroup(options);
        }

        public async Task<ReturnResult> UpdateGroup(Guid id, CreateOrUpdateGroupOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var group = await this.groupsStore.GetGroup(id);

            if (group == null)
            {
                return ReturnResult.ErrorResult("GROUP_NOT_FOUND", $"Group matching id {id} is not found.");
            }

            await this.groupsStore.UpdateGroup(id, options);

            return ReturnResult.SuccessResult;
        }

        public async Task<ReturnResult> RemoveGroup(Guid id)
        {
            var group = await this.groupsStore.GetGroup(id);

            if (group == null)
            {
                return ReturnResult.ErrorResult("GROUP_NOT_FOUND", $"Group matching id {id} is not found.");
            }

            await this.groupsStore.RemoveGroup(id);

            return ReturnResult.SuccessResult;
        }
    }
}
