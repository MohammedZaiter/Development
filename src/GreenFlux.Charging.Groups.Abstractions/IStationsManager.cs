
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Manager abstraction encapsulate the business rules for Stations.
    /// </summary>
    public interface IStationsManager
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
        Task<ReturnResult<IEnumerable<Station>>> GetStationsByGroupId(Guid groupId);

        /// <summary>
        /// Creates the station.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ReturnResult<Guid>> CreateStation(CreateOrUpdateStationOptions options);

        /// <summary>
        /// Updates the station.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ReturnResult> UpdateStation(Guid stationId, CreateOrUpdateStationOptions options);

        /// <summary>
        /// Removes the station.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <returns></returns>
        Task<ReturnResult> RemoveStation(Guid stationId);
    }
}
