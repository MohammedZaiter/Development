
namespace GreenFlux.Charging.Groups.Store
{
    using GreenFlux.Charging.Store;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    /// <summary>
    /// Store class that encapsulates database operations layer for Connectors.
    /// </summary>
    /// <seealso cref="GreenFlux.Charging.Store.DataStore" />
    /// <seealso cref="GreenFlux.Charging.Groups.IConnectorsStore" />
    /// <seealso cref="GreenFlux.Charging.Groups.IGroupsStore" />
    /// <seealso cref="GreenFlux.Charging.Groups.IStationsStore" />
    public partial class Store : DataStore, IConnectorsStore
    {
        /// <summary>
        /// Gets the connector.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        public async Task<Connector> GetConnector(Guid stationId, int identifier)
        {
            var con = await this.connectionManager.GetConnection();

            using var getConnectorCmd = new SqlCommand("usp_GetConnectorByIdentifier", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = getConnectorCmd.Parameters.AddWithValue("@id", identifier);
            _ = getConnectorCmd.Parameters.AddWithValue("@stationId", stationId);

            try
            {
                using var reader = await getConnectorCmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return this.ConnectorFromReader(reader);
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing GetConnector");

                throw;
            }
        }

        /// <summary>
        /// Gets the connectors by station identifier.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Connector>> GetConnectorsByStationId(Guid stationId)
        {
            var con = await this.connectionManager.GetConnection();

            using var getConnectorByStationIdGroupCmd = new SqlCommand("usp_GetConnectorsByStationId", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = getConnectorByStationIdGroupCmd.Parameters.AddWithValue("@stationId", stationId);

            try
            {
                using var reader = await getConnectorByStationIdGroupCmd.ExecuteReaderAsync();

                var connectors = new List<Connector>();

                while (reader.Read())
                {
                    connectors.Add(
                         this.ConnectorFromReader(reader)
                        );
                }

                return connectors;
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing GetConnectorsByStationId");

                throw;
            }
        }

        /// <summary>
        /// Creates the connector.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="identifier"></param>
        /// <param name="options">The options.</param>
        public async Task CreateConnector(Guid groupId, int identifier, CreateOrUpdateConnectorOptions options)
        {
            var con = await this.connectionManager.GetConnection();

            using var createConnectorCmd = new SqlCommand("usp_CreateConnector", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = createConnectorCmd.Parameters.AddWithValue("@id", identifier);
            _ = createConnectorCmd.Parameters.AddWithValue("@stationId", options.StationId);
            _ = createConnectorCmd.Parameters.AddWithValue("@maxCurrent", options.MaxCurrent);

            try
            {
                await createConnectorCmd.ExecuteNonQueryAsync();
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing CreateConnector");

                throw;
            }
        }

        /// <summary>
        /// Updates the connector current.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="oldCurrent">The old current.</param>
        /// <param name="options">The options.</param>
        public async Task UpdateConnectorCurrent(Guid groupId, int id, long oldCurrent, CreateOrUpdateConnectorOptions options)
        {
            var con = await this.connectionManager.GetConnection();

            using var updateConnectorCmd = new SqlCommand("usp_UpdateConnectorCurrent", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = updateConnectorCmd.Parameters.AddWithValue("@id", id);
            _ = updateConnectorCmd.Parameters.AddWithValue("@stationId", options.StationId);
            _ = updateConnectorCmd.Parameters.AddWithValue("@oldCurrent", oldCurrent);
            _ = updateConnectorCmd.Parameters.AddWithValue("@newCurrent", options.MaxCurrent);

            try
            {
                await updateConnectorCmd.ExecuteNonQueryAsync();
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing UpdateConnectorCurrent");

                throw;
            }
        }

        /// <summary>
        /// Removes the connector.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="oldCurrent">The old current.</param>
        public async Task RemoveConnector(Guid groupId, Guid stationId, int id, long oldCurrent)
        {
            var con = await this.connectionManager.GetConnection();

            using var removeConnectorCmd = new SqlCommand("usp_RemoveConnector", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = removeConnectorCmd.Parameters.AddWithValue("@id", id);
            _ = removeConnectorCmd.Parameters.AddWithValue("@stationId", stationId);

            try
            {
                await removeConnectorCmd.ExecuteNonQueryAsync();
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing RemoveConnector");

                throw;
            }
        }

        /// <summary>
        /// Read connector from reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private Connector ConnectorFromReader(SqlDataReader reader)
        {
            return new Connector()
            {
                Id = SafeCast<int>(reader["Id"]),
                StationId = SafeCast<Guid>(reader["StationId"]),
                MaxCurrent = SafeCast<long>(reader["MaxCurrent"])
            };
        }
    }
}
