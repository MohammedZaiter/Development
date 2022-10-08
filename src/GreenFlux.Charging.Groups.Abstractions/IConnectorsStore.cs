
namespace GreenFlux.Charging.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Store abstraction that encapsulates database operations layer for Connectors.
    /// </summary>
    public interface IConnectorsStore
    {
        /// <summary>
        /// Gets the connector.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Connector> GetConnector(Guid stationId, int id);

        /// <summary>
        /// Gets the connectors by station identifier.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<Connector>> GetConnectorsByStationId(Guid stationId);

        /// <summary>
        /// Creates the connector.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task CreateConnector(Guid groupId, int id, CreateOrUpdateConnectorOptions options);

        /// <summary>
        /// Updates the connector current.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="oldCurrent">The old current.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task UpdateConnectorCurrent(Guid groupId, int id, long oldCurrent, CreateOrUpdateConnectorOptions options);

        /// <summary>
        /// Removes the connector.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="oldCurrent">The old current.</param>
        /// <returns></returns>
        Task RemoveConnector(Guid groupId, Guid stationId, int id, long oldCurrent);
    }
}
