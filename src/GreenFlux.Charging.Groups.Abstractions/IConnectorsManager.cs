
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConnectorsManager
    {
        Task<ReturnResult<Connector>> GetConnectorById(Guid stationId, int id);

        Task<ReturnResult<IEnumerable<Connector>>> GetConnectorsByStationId(Guid stationId);

        Task<ReturnResult> CreateConnector(CreateOrUpdateConnectorOptions options);

        Task<ReturnResult> UpdateConnector(int id, CreateOrUpdateConnectorOptions options);

        Task<ReturnResult> RemoveConnector(Guid stationId, int id);
    }
}
