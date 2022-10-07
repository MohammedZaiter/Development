
namespace GreenFlux.Charging.Store
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public sealed class ConnectionManager : IConnectionManager
    {
        private readonly string connectionString;
        private SqlConnection connection;
        private bool disposed;

        public ConnectionManager(DataStoreOptions dataStoreOptions)
        {
            this.connectionString = dataStoreOptions.ConnectionString;
        }

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
