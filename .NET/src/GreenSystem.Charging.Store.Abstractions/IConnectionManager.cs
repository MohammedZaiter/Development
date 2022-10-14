
namespace GreenSystem.Charging.Store
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    /// <summary>
    /// Connection manager abstraction to establish and maintain store connection.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IConnectionManager : IDisposable
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        Task<SqlConnection> GetConnection();
    }
}
