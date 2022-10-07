
namespace GreenFlux.Charging.Store
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public interface IConnectionManager : IDisposable
    {
        Task<SqlConnection> GetConnection();
    }
}
