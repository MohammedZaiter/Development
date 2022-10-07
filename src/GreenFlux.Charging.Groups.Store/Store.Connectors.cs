
namespace GreenFlux.Charging.Groups.Store
{
    using GreenFlux.Charging.Store;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public partial class Store : DataStore, IConnectorsStore
    {
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

        public async Task CreateConnector(Guid groupId, int identifier, CreateOrUpdateConnectorOptions options)
        {
            var con = await this.connectionManager.GetConnection();

            using var transaction = await con.BeginTransactionAsync(IsolationLevel.Serializable);

            using var createConnectorCmd = new SqlCommand("usp_CreateConnector", con, (SqlTransaction)transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = createConnectorCmd.Parameters.AddWithValue("@id", identifier);
            _ = createConnectorCmd.Parameters.AddWithValue("@stationId", options.StationId);
            _ = createConnectorCmd.Parameters.AddWithValue("@maxCurrent", options.MaxCurrent);

            try
            {
                await createConnectorCmd.ExecuteNonQueryAsync();

                await this.UpdateStationCurrent(options.StationId, options.MaxCurrent, 0, (SqlTransaction)transaction, con);

                await this.UpdateGroupCurrent(groupId, options.MaxCurrent, 0, (SqlTransaction)transaction, con);

                await transaction.CommitAsync();
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                this.logger.LogError(ex, "An error occured while executing CreateConnector");

                throw;
            }
        }

        public async Task UpdateConnectorCurrent(Guid groupId, int id, long oldCurrent, CreateOrUpdateConnectorOptions options)
        {
            var con = await this.connectionManager.GetConnection();

            using var transaction = await con.BeginTransactionAsync(IsolationLevel.Serializable);

            using var updateConnectorCmd = new SqlCommand("usp_UpdateConnectorCurrent", con, (SqlTransaction)transaction)
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

                await this.UpdateStationCurrent(options.StationId, options.MaxCurrent, oldCurrent, (SqlTransaction)transaction, con);

                await this.UpdateGroupCurrent(groupId, options.MaxCurrent, oldCurrent, (SqlTransaction)transaction, con);

                await transaction.CommitAsync();
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                this.logger.LogError(ex, "An error occured while executing UpdateConnectorCurrent");

                throw;
            }
        }

        public async Task RemoveConnector(Guid groupId, Guid stationId, int id, long oldCurrent)
        {
            var con = await this.connectionManager.GetConnection();

            using var transaction = await con.BeginTransactionAsync(IsolationLevel.Serializable);

            using var removeConnectorCmd = new SqlCommand("usp_RemoveConnector", con, (SqlTransaction)transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = removeConnectorCmd.Parameters.AddWithValue("@id", id);
            _ = removeConnectorCmd.Parameters.AddWithValue("@stationId", stationId);

            try
            {
                await removeConnectorCmd.ExecuteNonQueryAsync();

                await this.UpdateStationCurrent(stationId, 0, oldCurrent, (SqlTransaction)transaction, con);

                await this.UpdateGroupCurrent(groupId, 0, oldCurrent, (SqlTransaction)transaction, con);

                await transaction.CommitAsync();
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                this.logger.LogError(ex, "An error occured while executing RemoveConnector");

                throw;
            }
        }

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
