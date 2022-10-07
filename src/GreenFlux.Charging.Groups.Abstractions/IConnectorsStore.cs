
namespace GreenFlux.Charging.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConnectorsStore
    {
        Task<Connector> GetConnector(Guid stationId, int id);

        Task<IEnumerable<Connector>> GetConnectorsByStationId(Guid stationId);

        Task CreateConnector(Guid groupId, int id, CreateOrUpdateConnectorOptions options);

        Task UpdateConnectorCurrent(Guid groupId, int id, long oldCurrent, CreateOrUpdateConnectorOptions options);

        Task RemoveConnector(Guid groupId, Guid stationId, int id, long oldCurrent);
    }
}
