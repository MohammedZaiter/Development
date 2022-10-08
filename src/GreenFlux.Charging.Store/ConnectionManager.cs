
namespace GreenFlux.Charging.Store
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    /// <summary>
    /// Connection manager class to establish and maintain store connection.
    /// </summary>
    /// <seealso cref="GreenFlux.Charging.Store.IConnectionManager" />
    public sealed class ConnectionManager : IConnectionManager
    {
        private readonly string connectionString;
        private SqlConnection connection;
        private bool disposed;

        public ConnectionManager(DataStoreOptions dataStoreOptions)
        {
            this.connectionString = dataStoreOptions.ConnectionString;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ObjectDisposedException">ConnectionManager</exception>
        public async Task<SqlConnection> GetConnection()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(nameof(ConnectionManager));
            }

            if (this.connection == null)
            {
                this.connection = new SqlConnection(this.connectionString);

                await this.connection.OpenAsync();
            }

            return this.connection;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.disposed = true;

            if (this.connection != null)
            {
                this.connection.Dispose();
            }
        }
    }
}
