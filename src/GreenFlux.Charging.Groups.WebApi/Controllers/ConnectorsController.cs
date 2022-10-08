
namespace GreenFlux.Charging.Group.WebApi
{
    using GreenFlux.Charging.Groups;
    using GreenFlux.Charging.Groups.WebApi.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public sealed class ConnectorsController : Controller
    {
        private readonly IConnectorsManager connectorsManager;

        public ConnectorsController(IConnectorsManager connectorsManager)
        {
            this.connectorsManager = connectorsManager ?? throw new ArgumentNullException(nameof(connectorsManager));
        }

        /// <summary>
        /// Gets the connectors by station identifier.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("stations/{stationId}/connectors")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Connector>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetConnectorsByStationId([FromRoute] Guid stationId)
        {
            var result = await this.connectorsManager.GetConnectorsByStationId(stationId);

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Gets the connector.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("stations/{stationId}/connectors/{id}")]
        [ProducesResponseType(200, Type = typeof(Connector))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetConnector([FromRoute] Guid stationId, [FromRoute] int id)
        {
            var result = await this.connectorsManager.GetConnectorById(stationId, id);

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Creates the connector.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="createModel">The create model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("stations/{stationId}/connectors")]
        [ProducesResponseType(200, Type = typeof(Connector))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateConnector([FromRoute] Guid stationId, [FromQuery] CreateOrUpdateConnectorModel createModel)
        {
            var result = await this.connectorsManager.CreateConnector(createModel.ToOptions(stationId));

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Updates the connector.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="updateModel">The update model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("stations/{stationId}/connectors/{id}")]
        [ProducesResponseType(200, Type = typeof(void))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateConnector(
            [FromRoute] Guid stationId,
            [FromRoute] int id,
            [FromBody] CreateOrUpdateConnectorModel updateModel)
        {
            var result = await this.connectorsManager.UpdateConnector(id, updateModel.ToOptions(stationId));

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Removes the connector.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("stations/{stationId}/connectors/{id}")]
        [ProducesResponseType(200, Type = typeof(void))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveConnector([FromRoute] Guid stationId, [FromRoute] int id)
        {
            var result = await this.connectorsManager.RemoveConnector(stationId, id);

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok();
        }
    }
}