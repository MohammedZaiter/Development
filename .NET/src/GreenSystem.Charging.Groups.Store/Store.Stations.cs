
namespace GreenSystem.Charging.Groups.Store
{
    using GreenSystem.Charging.Store;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    /// <summary>
    /// Store class that encapsulates database operations layer for Stations.
    /// </summary>
    /// <seealso cref="GreenSystem.Charging.Store.DataStore" />
    /// <seealso cref="GreenSystem.Charging.Groups.IConnectorsStore" />
    /// <seealso cref="GreenSystem.Charging.Groups.IGroupsStore" />
    /// <seealso cref="GreenSystem.Charging.Groups.IStationsStore" />
    public partial class Store : DataStore, IStationsStore
    {
        /// <summary>
        /// Gets the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the stations by group identifier.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates the station.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="options">The options.</param>
        public async Task UpdateStation(Guid id, CreateOrUpdateStationOptions options)
        {
            var con = await this.connectionManager.GetConnection();

            using var updateStationCmd = new SqlCommand("usp_UpdateStation", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = updateStationCmd.Parameters.AddWithValue("@id", id);
            _ = updateStationCmd.Parameters.AddWithValue("@name", NormalizeValue(options.Name));
            _ = updateStationCmd.Parameters.AddWithValue("@groupId", options.GroupId);

            try
            {
                await updateStationCmd.ExecuteNonQueryAsync();
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing UpdateStation");

                throw;
            }
        }

        /// <summary>
        /// Removes the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task RemoveStation(Guid id)
        {
            var con = await this.connectionManager.GetConnection();

            using var removeStationCmd = new SqlCommand("usp_RemoveStation", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = removeStationCmd.Parameters.AddWithValue("@id", id);

            try
            {
                await removeStationCmd.ExecuteNonQueryAsync();
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing RemoveStation");

                throw;
            }
        }

        /// <summary>
        /// Read Station from reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private Station StationFromReader(SqlDataReader reader)
        {
            return new Station()
            {
                Id = SafeCast<Guid>(reader["Id"]),
                Name = SafeCast<string>(reader["Name"]),
                GroupId = SafeCast<Guid>(reader["GroupId"])
            };
        }
    }
}
