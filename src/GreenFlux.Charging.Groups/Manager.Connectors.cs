
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class Manager : IConnectorsManager
    {
        public async Task<ReturnResult<Connector>> GetConnectorById(Guid stationId, int id)
        {
            var station = await this.stationsStore.GetStation(stationId);

            if (station == null)
            {
                return ReturnResult<Connector>.ErrorResult("STATION_NOT_FOUND", $"Station matching id {stationId} is not found.");
            }

            var connector = await this.connectorsStore.GetConnector(stationId, id);

            return ReturnResult<Connector>.SuccessResult(connector);
        }

        public async Task<ReturnResult<IEnumerable<Connector>>> GetConnectorsByStationId(Guid stationId)
        {
            var station = await this.stationsStore.GetStation(stationId);

            if (station == null)
            {
                return ReturnResult<IEnumerable<Connector>>.ErrorResult("STATION_NOT_FOUND", $"Station matching id {stationId} is not found.");
            }

            var connectors = await this.connectorsStore.GetConnectorsByStationId(stationId);

            return ReturnResult<IEnumerable<Connector>>.SuccessResult(connectors);
        }

        public async Task<ReturnResult> CreateConnector(CreateOrUpdateConnectorOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var station = await this.stationsStore.GetStation(options.StationId);

            if (station == null)
            {
                return ReturnResult.ErrorResult("STATION_NOT_FOUND", $"Station matching id {options.StationId} is not found.");
            }

            var connectors = await this.connectorsStore.GetConnectorsByStationId(options.StationId);

            if (connectors.Count() >= 5)
            {
                return ReturnResult.ErrorResult("STATION_EXCEEDS_CONNECTERS", $"Station matching id {options.StationId} exceeds the allowed connectors (5).");
            }

            var availableSlotNumber = this.GetAvailableSlotNumber(connectors);

            var group = await this.groupsStore.GetGroup(station.GroupId);

            if (group.Capacity < group.ConsumedCapacity + options.MaxCurrent)
            {
                return ReturnResult.ErrorResult("INVALID_CONNECTOR", $"Connector with {options.MaxCurrent} exceeds group capacity.");
            }

            await this.connectorsStore.CreateConnector(station.GroupId, availableSlotNumber, options);

            return ReturnResult.SuccessResult;
        }

        public async Task<ReturnResult> UpdateConnector(int id, CreateOrUpdateConnectorOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (id < 1)
            {
                return ReturnResult.ErrorResult("INVALID_CONNECTOR_ID", $"Connector should be with id greater than or equal 1.");
            }

            var station = await this.stationsStore.GetStation(options.StationId);

            if (station == null)
            {
                return ReturnResult.ErrorResult("STATION_NOT_FOUND", $"Station matching id {options.StationId} is not found.");
            }

            var connector = await this.connectorsStore.GetConnector(options.StationId, id);

            if (connector == null)
            {
                return ReturnResult.ErrorResult("CONNECTOR_NOT_FOUND", $"Connector matching id {id} is not found.");
            }

            var group = await this.groupsStore.GetGroup(station.GroupId);

            if (group.Capacity < (group.ConsumedCapacity - connector.MaxCurrent) + options.MaxCurrent)
            {
                return ReturnResult.ErrorResult("INVALID_CONNECTOR", $"Connector with {options.MaxCurrent} exceeds group capacity.");
            }

            await this.connectorsStore.UpdateConnectorCurrent(station.GroupId, id, connector.MaxCurrent, options);

            return ReturnResult.SuccessResult;
        }

        public async Task<ReturnResult> RemoveConnector(Guid stationId, int id)
        {
            if (id < 1)
            {
                return ReturnResult.ErrorResult("INVALID_CONNECTOR_ID", $"Connector should be with id greater than or equal 1.");
            }

            var station = await this.stationsStore.GetStation(stationId);

            if (station == null)
            {
                return ReturnResult.ErrorResult("STATION_NOT_FOUND", $"Station matching id {stationId} is not found.");
            }

            var connector = await this.connectorsStore.GetConnector(stationId, id);

            if (connector == null)
            {
                return ReturnResult.ErrorResult("CONNECTOR_NOT_FOUND", $"Connector matching id {id} is not found.");
            }

            await this.connectorsStore.RemoveConnector(station.GroupId, stationId, id, connector.MaxCurrent);

            return ReturnResult.SuccessResult;
        }

        private int GetAvailableSlotNumber(IEnumerable<Connector> connectors)
        {
            if (connectors.Any() == false)
            {
                return 1;
            }

            if (connectors.Count() >= 5)
            {
                throw new InvalidOperationException("Unavailable slots for the specified section.");
            }

            var sortedSlots = connectors
                .Select(ct => ct.Id)
                .OrderBy(e => e)
                .ToArray();

            for (int i = 1; i < 6; i++)
            {
                if (sortedSlots.Length < i)
                {
                    return i;
                }

                if (sortedSlots[i - 1] != i)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}
