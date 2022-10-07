
namespace GreenFlux.Charging.Groups.Store
{
    using GreenFlux.Charging.Store;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public partial class Store : DataStore, IStationsStore
    {
        public async Task<Station> GetStation(Guid id)
        {
            var con = await this.connectionManager.GetConnection();

            using var getStationCmd = new SqlCommand("usp_GetStationById", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = getStationCmd.Parameters.AddWithValue("@id", id);

            try
            {
                using var reader = await getStationCmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return this.StationFromReader(reader);
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing GetStation");

                throw;
            }
        }

        public async Task<IEnumerable<Station>> GetStationsByGroupId(Guid groupId)
        {
            var con = await this.connectionManager.GetConnection();

            using var getStationByGroupIdCmd = new SqlCommand("usp_GetStationsByGroupId", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = getStationByGroupIdCmd.Parameters.AddWithValue("@groupId", groupId);

            try
            {
                using var reader = await getStationByGroupIdCmd.ExecuteReaderAsync();

                var stations = new List<Station>();

                while (reader.Read())
                {
                    stations.Add(
                        this.StationFromReader(reader)
                        );
                }

                return stations;
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing GetStationsByGroupId");

                throw;
            }
        }

        public async Task<Guid> CreateStation(CreateOrUpdateStationOptions options)
        {
            var con = await this.connectionManager.GetConnection();

            var stationId = Guid.NewGuid();

            using var createStationCmd = new SqlCommand("usp_CreateStation", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = createStationCmd.Parameters.AddWithValue("@id", stationId);
            _ = createStationCmd.Parameters.AddWithValue("@name", NormalizeValue(options.Name));
            _ = createStationCmd.Parameters.AddWithValue("@groupId", options.GroupId);

            try
            {
                await createStationCmd.ExecuteNonQueryAsync();

                return stationId;
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing CreateStation");

                throw;
            }
        }

        public async Task UpdateStation(Guid id, Guid oldGroupId, long stationCurrent, CreateOrUpdateStationOptions options)
        {
            var con = await this.connectionManager.GetConnection();

            using var transaction = await con.BeginTransactionAsync(IsolationLevel.Serializable);

            using var updateStationCmd = new SqlCommand("usp_UpdateStation", con, (SqlTransaction)transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = updateStationCmd.Parameters.AddWithValue("@id", id);
            _ = updateStationCmd.Parameters.AddWithValue("@name", NormalizeValue(options.Name));
            _ = updateStationCmd.Parameters.AddWithValue("@groupId", options.GroupId);

            try
            {
                await updateStationCmd.ExecuteNonQueryAsync();

                if (options.GroupId != oldGroupId)
                {
                    await this.UpdateGroupCurrent(oldGroupId, 0, stationCurrent, (SqlTransaction)transaction, con);

                    await this.UpdateGroupCurrent(options.GroupId, stationCurrent, 0, (SqlTransaction)transaction, con);
                }

                await transaction.CommitAsync();
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                this.logger.LogError(ex, "An error occured while executing UpdateStation");

                throw;
            }
        }

        public async Task RemoveStation(Guid groupId, Guid id, long stationCurrent)
        {
            var con = await this.connectionManager.GetConnection();

            using var transaction = await con.BeginTransactionAsync(IsolationLevel.Serializable);

            using var removeStationCmd = new SqlCommand("usp_RemoveStation", con, (SqlTransaction)transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = removeStationCmd.Parameters.AddWithValue("@id", id);

            try
            {
                await removeStationCmd.ExecuteNonQueryAsync();

                await this.UpdateGroupCurrent(groupId, 0, stationCurrent, (SqlTransaction)transaction, con);

                await transaction.CommitAsync();
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                this.logger.LogError(ex, "An error occured while executing RemoveStation");

                throw;
            }
        }

        private async Task UpdateStationCurrent(Guid stationId,
            long addedCurrent,
            long subtractedCurrent,
            SqlTransaction transaction,
            SqlConnection con)
        {
            using var updateStationCurrentCmd = new SqlCommand("usp_UpdateStationCurrent", con, (SqlTransaction)transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = updateStationCurrentCmd.Parameters.AddWithValue("@id", stationId);
            _ = updateStationCurrentCmd.Parameters.AddWithValue("@addedCurrent", addedCurrent);
            _ = updateStationCurrentCmd.Parameters.AddWithValue("@subtractedCurrent", subtractedCurrent);

            await updateStationCurrentCmd.ExecuteNonQueryAsync();
        }

        private Station StationFromReader(SqlDataReader reader)
        {
            return new Station()
            {
                Id = SafeCast<Guid>(reader["Id"]),
                Name = SafeCast<string>(reader["Name"]),
                GroupId = SafeCast<Guid>(reader["GroupId"]),
                ConsumedCurrent = SafeCast<long>(reader["ConsumedCurrent"])
            };
        }
    }
}
