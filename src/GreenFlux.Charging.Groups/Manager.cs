
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Manager class encapsulate the business rules for Groups.
    /// </summary>
    /// <seealso cref="GreenFlux.Charging.Groups.IConnectorsManager" />
    /// <seealso cref="GreenFlux.Charging.Groups.IGroupsManager" />
    /// <seealso cref="GreenFlux.Charging.Groups.IStationsManager" />
    public sealed partial class Manager : IGroupsManager
    {
        private readonly IGroupsStore groupsStore;
        private readonly IStationsStore stationsStore;
        private readonly IConnectorsStore connectorsStore;
        private readonly ICachingService cachingService;

        public Manager(
            IGroupsStore groupsStore,
            IStationsStore stationsStore,
            IConnectorsStore connectorsStore,
            ICachingService cachingService)
        {
            this.groupsStore = groupsStore ?? throw new ArgumentNullException(nameof(groupsStore));
            this.connectorsStore = connectorsStore ?? throw new ArgumentNullException(nameof(stationsStore));
            this.cachingService = cachingService ?? throw new ArgumentNullException(nameof(cachingService));
            this.stationsStore = stationsStore ?? throw new ArgumentNullException(nameof(stationsStore));
        }

        /// <summary>
        /// Gets the group by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ReturnResult<Group>> GetGroupById(Guid id)
        {
            return ReturnResult<Group>.SuccessResult(await this.groupsStore.GetGroup(id));
        }

        /// <summary>
        /// Creates the group.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">options</exception>
        public Task<Guid> CreateGroup(CreateOrUpdateGroupOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return this.groupsStore.CreateGroup(options);
        }

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">options</exception>
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

        /// <summary>
        /// Removes the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the group cache consumed current key.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        private string GetGroupConsumedCurrentKey(Guid groupId)
        {
            return $"Groups:{groupId}:ConsumedCurrent";
        }
    }
}
