
namespace GreenFlux.Charging.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStationsStore
    {
        Task<Station> GetStation(Guid id);

        Task<IEnumerable<Station>> GetStationsByGroupId(Guid groupId);

        Task<Guid> CreateStation(CreateOrUpdateStationOptions options);

        Task UpdateStation(Guid id, Guid oldGroupId, long stationCurrent, CreateOrUpdateStationOptions options);

        Task RemoveStation(Guid groupId, Guid id, long stationCurrent);
    }
}
