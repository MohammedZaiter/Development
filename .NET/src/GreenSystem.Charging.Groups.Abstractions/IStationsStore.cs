
namespace GreenSystem.Charging.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Store abstraction that encapsulates database operations layer for Stations.
    /// </summary>
    public interface IStationsStore
    {
        /// <summary>
        /// Gets the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Station> GetStation(Guid id);

        /// <summary>
        /// Gets the stations by group identifier.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<Station>> GetStationsByGroupId(Guid groupId);

        /// <summary>
        /// Creates the station.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<Guid> CreateStation(CreateOrUpdateStationOptions options);

        /// <summary>
        /// Updates the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task UpdateStation(Guid id, CreateOrUpdateStationOptions options);

        /// <summary>
        /// Removes the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task RemoveStation(Guid id);
    }
}
