
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Manager abstraction encapsulate the business rules for Groups.
    /// </summary>
    public interface IGroupsManager
    {
        /// <summary>
        /// Gets the group by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ReturnResult<Group>> GetGroupById(Guid id);

        /// <summary>
        /// Creates the group.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<Guid> CreateGroup(CreateOrUpdateGroupOptions options);

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ReturnResult> UpdateGroup(Guid id, CreateOrUpdateGroupOptions options);

        /// <summary>
        /// Removes the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ReturnResult> RemoveGroup(Guid id);
    }
}
