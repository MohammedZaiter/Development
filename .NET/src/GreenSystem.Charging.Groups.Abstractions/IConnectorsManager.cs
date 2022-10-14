
namespace GreenSystem.Charging.Groups
{
    using GreenSystem.Charging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Manager abstraction encapsulate the business rules for Connectors.
    /// </summary>
    public interface IConnectorsManager
    {
        /// <summary>
        /// Gets the connector by id.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ReturnResult<Connector>> GetConnectorById(Guid stationId, int id);

        /// <summary>
        /// Gets the connectors by station id.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <returns></returns>
        Task<ReturnResult<IEnumerable<Connector>>> GetConnectorsByStationId(Guid stationId);

        /// <summary>
        /// Creates the connector.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ReturnResult> CreateConnector(CreateOrUpdateConnectorOptions options);

        /// <summary>
        /// Updates the connector.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ReturnResult> UpdateConnector(int id, CreateOrUpdateConnectorOptions options);

        /// <summary>
        /// Removes the connector.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ReturnResult> RemoveConnector(Guid stationId, int id);
    }
}
