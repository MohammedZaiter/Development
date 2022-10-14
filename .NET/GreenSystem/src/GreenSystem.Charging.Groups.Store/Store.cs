
namespace GreenSystem.Charging.Groups.Store
{
    using GreenSystem.Charging.Store;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    /// <summary>
    /// Store class that encapsulates database operations layer for Groups.
    /// </summary>
    /// <seealso cref="GreenSystem.Charging.Store.DataStore" />
    /// <seealso cref="GreenSystem.Charging.Groups.IConnectorsStore" />
    /// <seealso cref="GreenSystem.Charging.Groups.IGroupsStore" />
    /// <seealso cref="GreenSystem.Charging.Groups.IStationsStore" />
    public partial class Store : DataStore, IGroupsStore
    {
        private readonly ILogger<Store> logger;

        public Store(IConnectionManager connectionManager, ILogger<Store> logger) : base(connectionManager)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Group> GetGroup(Guid id)
        {
            var con = await this.connectionManager.GetConnection();

            using var getGroupCmd = new SqlCommand("usp_GetGroupById", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = getGroupCmd.Parameters.AddWithValue("@id", id);

            try
            {
                using var reader = await getGroupCmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return this.GroupFromReader(reader);
                }
                else
                {
                    return null;
                }

            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing GetGroup");

                throw;
            }
        }

        /// <summary>
        /// Creates the group.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<Guid> CreateGroup(CreateOrUpdateGroupOptions options)
        {
            var con = await this.connectionManager.GetConnection();

            var groupId = Guid.NewGuid();

            using var createGroupCmd = new SqlCommand("usp_CreateGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = createGroupCmd.Parameters.AddWithValue("@id", groupId);
            _ = createGroupCmd.Parameters.AddWithValue("@name", NormalizeValue(options.Name));
            _ = createGroupCmd.Parameters.AddWithValue("@capacity", options.Capacity);

            try
            {
                await createGroupCmd.ExecuteNonQueryAsync();

                return groupId;
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing CreateGroup");

                throw;
            }
        }

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="options">The options.</param>
        public async Task UpdateGroup(Guid groupId, CreateOrUpdateGroupOptions options)
        {
            var con = await this.connectionManager.GetConnection();

            using var updateGroupCmd = new SqlCommand("usp_UpdateGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = updateGroupCmd.Parameters.AddWithValue("@id", groupId);
            _ = updateGroupCmd.Parameters.AddWithValue("@name", NormalizeValue(options.Name));
            _ = updateGroupCmd.Parameters.AddWithValue("@capacity", options.Capacity);

            try
            {
                await updateGroupCmd.ExecuteNonQueryAsync();
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing UpdateGroup");

                throw;
            }
        }

        /// <summary>
        /// Removes the group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        public async Task RemoveGroup(Guid groupId)
        {
            var con = await this.connectionManager.GetConnection();

            using var removeGroupCmd = new SqlCommand("usp_RemoveGroup", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            _ = removeGroupCmd.Parameters.AddWithValue("@id", groupId);

            try
            {
                await removeGroupCmd.ExecuteNonQueryAsync();
            }

            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while executing RemoveGroup");

                throw;
            }
        }

        /// <summary>
        /// Read Group from reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private Group GroupFromReader(SqlDataReader reader)
        {
            return new Group()
            {
                Id = SafeCast<Guid>(reader["Id"]),
                Name = SafeCast<string>(reader["Name"]),
                Capacity = SafeCast<long>(reader["Capacity"])
            };
        }
    }
}
