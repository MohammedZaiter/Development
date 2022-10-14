
namespace GreenSystem.Charging.Groups
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Store abstraction that encapsulates database operations layer for Groups.
    /// </summary>
    public interface IGroupsStore
    {
        /// <summary>
        /// Gets the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Group> GetGroup(Guid id);

        /// <summary>
        /// Creates the group.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<Guid> CreateGroup(CreateOrUpdateGroupOptions options);

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task UpdateGroup(Guid groupId, CreateOrUpdateGroupOptions options);

        /// <summary>
        /// Removes the group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task RemoveGroup(Guid groupId);
    }
}
