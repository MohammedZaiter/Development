
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed partial class Manager : IStationsManager
    {
        public Task<Station> GetStation(Guid id)
        {
            return this.stationsStore.GetStation(id);
        }

        public async Task<ReturnResult<IEnumerable<Station>>> GetStationsByGroupId(Guid groupId)
        {
            var group = await this.groupsStore.GetGroup(groupId);

            if (group == null)
            {
                return ReturnResult<IEnumerable<Station>>.ErrorResult("GROUP_NOT_FOUND", $"Group matching id {groupId} is not found.");
            }

            var stations = await this.stationsStore.GetStationsByGroupId(groupId);

            return ReturnResult<IEnumerable<Station>>.SuccessResult(stations);
        }

        public async Task<ReturnResult<Guid>> CreateStation(CreateOrUpdateStationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var group = await this.groupsStore.GetGroup(options.GroupId);

            if (group == null)
            {
                return ReturnResult<Guid>.ErrorResult("GROUP_NOT_FOUND", $"Group matching id {options.GroupId} is not found.");
            }

            return ReturnResult<Guid>.SuccessResult(await this.stationsStore.CreateStation(options));
        }

        public async Task<ReturnResult> UpdateStation(Guid stationId, CreateOrUpdateStationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var station = await this.stationsStore.GetStation(stationId);

            if (station == null)
            {
                return ReturnResult.ErrorResult("STATION_NOT_FOUND", $"Station matching id {stationId} is not found.");
            }

            var group = await this.groupsStore.GetGroup(options.GroupId);

            if (group == null)
            {
                return ReturnResult.ErrorResult("GROUP_NOT_FOUND", $"Group matching id {options.GroupId} is not found.");
            }

            await this.stationsStore.UpdateStation(stationId, station.GroupId, station.ConsumedCurrent, options);

            if (station.GroupId != options.GroupId)
            {
                var stationCureent = await this.cachingService.Get<long>(this.GetStationConsumedCurrentKey(stationId));

                await Task.WhenAll(new Task[]
                {
                     this.cachingService.Increment(this.GetGroupConsumedCurrentKey(options.GroupId), stationCureent),
                     this.cachingService.Decrement(this.GetGroupConsumedCurrentKey(station.GroupId), stationCureent)
                });
            }

            return ReturnResult.SuccessResult;
        }

        public async Task<ReturnResult> RemoveStation(Guid stationId)
        {
            var station = await this.stationsStore.GetStation(stationId);

            if (station == null)
            {
                return ReturnResult.ErrorResult("STATION_NOT_FOUND", $"Station matching id {stationId} is not found.");
            }


            var stationCureent = await this.cachingService.Get<long>(this.GetStationConsumedCurrentKey(stationId));

            await Task.WhenAll(new Task[]
            {
                this.stationsStore.RemoveStation(station.GroupId, stationId, station.ConsumedCurrent),
                this.cachingService.Decrement(this.GetGroupConsumedCurrentKey(station.GroupId), stationCureent),
                this.cachingService.Delete(this.GetStationConsumedCurrentKey(stationId))
            });

            return ReturnResult.SuccessResult;
        }

        private string GetStationConsumedCurrentKey(Guid stationId)
        {
            return $"Stations:{stationId}:ConsumedCurrent";
        }
    }
}
