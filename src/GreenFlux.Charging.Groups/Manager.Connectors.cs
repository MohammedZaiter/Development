
namespace GreenFlux.Charging.Groups
{
    using GreenFlux.Charging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Manager class encapsulate the business rules for Connectors.
    /// </summary>
    /// <seealso cref="GreenFlux.Charging.Groups.IConnectorsManager" />
    /// <seealso cref="GreenFlux.Charging.Groups.IGroupsManager" />
    /// <seealso cref="GreenFlux.Charging.Groups.IStationsManager" />
    public partial class Manager : IConnectorsManager
    {
        /// <summary>
        /// Gets the connector by id.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the connectors by station id.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates the connector.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">options</exception>
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

            var groupConsumedCapacity = await this.cachingService.Get<long>(this.GetGroupConsumedCurrentKey(group.Id));

            if (group.Capacity < groupConsumedCapacity + options.MaxCurrent)
            {
                return ReturnResult.ErrorResult("INVALID_CONNECTOR", $"Connector with {options.MaxCurrent} exceeds group capacity.");
            }

            await Task.WhenAll(new Task[]
            {
                this.connectorsStore.CreateConnector(station.GroupId, availableSlotNumber, options),
                this.cachingService.Increment(this.GetStationConsumedCurrentKey(station.Id), options.MaxCurrent),
                this.cachingService.Increment(this.GetGroupConsumedCurrentKey(station.GroupId), options.MaxCurrent)
            });

            return ReturnResult.SuccessResult;
        }

        /// <summary>
        /// Updates the connector.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">options</exception>
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

            var groupConsumedCapacity = await this.cachingService.Get<long>(this.GetGroupConsumedCurrentKey(group.Id));

            if (group.Capacity < (groupConsumedCapacity - connector.MaxCurrent) + options.MaxCurrent)
            {
                return ReturnResult.ErrorResult("INVALID_CONNECTOR", $"Connector with {options.MaxCurrent} exceeds group capacity.");
            }

            if (connector.MaxCurrent == options.MaxCurrent)
            {
                return ReturnResult.SuccessResult;
            }

            var tasks = new List<Task>()
            {
                this.connectorsStore.UpdateConnectorCurrent(station.GroupId, id, connector.MaxCurrent, options)
            };

            //If oldCurrent = 10; newCurrent = 6 ==> 10-6 = 4 ==> decrement by 4
            //If oldCurrent = 6; newCurrent = 10 ==> 10-6 = 4 ==> increment by 4
            if (connector.MaxCurrent > options.MaxCurrent)
            {
                tasks.AddRange(new Task[]
                {
                    this.cachingService.Decrement(this.GetStationConsumedCurrentKey(station.Id), connector.MaxCurrent - options.MaxCurrent),
                    this.cachingService.Decrement(this.GetGroupConsumedCurrentKey(station.GroupId), connector.MaxCurrent- options.MaxCurrent),
                });
            }
            else
            {
                tasks.AddRange(new Task[]
                {
                    this.cachingService.Increment(this.GetStationConsumedCurrentKey(station.Id), options.MaxCurrent - connector.MaxCurrent),
                    this.cachingService.Increment(this.GetGroupConsumedCurrentKey(station.GroupId), options.MaxCurrent - connector.MaxCurrent)
                });
            }

            await Task.WhenAll(tasks);

            return ReturnResult.SuccessResult;
        }

        /// <summary>
        /// Removes the connector.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

            await Task.WhenAll(new Task[]
            {
                this.connectorsStore.RemoveConnector(station.GroupId, stationId, id, connector.MaxCurrent),
                this.cachingService.Decrement(this.GetStationConsumedCurrentKey(station.Id), connector.MaxCurrent),
                this.cachingService.Decrement(this.GetGroupConsumedCurrentKey(station.GroupId), connector.MaxCurrent),
            });

            return ReturnResult.SuccessResult;
        }

        /// <summary>
        /// Gets the available slot number (1-5).
        /// </summary>
        /// <param name="connectors">The connectors.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Unavailable slots for the specified section.</exception>
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
