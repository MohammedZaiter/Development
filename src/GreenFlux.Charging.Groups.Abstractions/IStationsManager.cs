
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStationsManager
    {
         Task<Station> GetStation(Guid id);

        Task<ReturnResult<IEnumerable<Station>>> GetStationsByGroupId(Guid groupId);

        Task<ReturnResult<Guid>> CreateStation(CreateOrUpdateStationOptions options);

        Task<ReturnResult> UpdateStation(Guid stationId, CreateOrUpdateStationOptions options);

        Task<ReturnResult> RemoveStation(Guid stationId);
    }
}
