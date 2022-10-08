
namespace GreenFlux.Charging.Groups.WebApi
{
    using GreenFlux.Charging.Groups.WebApi.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public sealed class StationsController : Controller
    {
        private readonly IStationsManager stationsManager;

        public StationsController(IStationsManager stationsManager)
        {
            this.stationsManager = stationsManager ?? throw new ArgumentNullException(nameof(stationsManager));
        }

        /// <summary>
        /// Gets the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("stations/{id}")]
        [ProducesResponseType(200, Type = typeof(Station))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStation([FromRoute] Guid id)
        {
            var station = await this.stationsManager.GetStation(id);

            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        /// <summary>
        /// Updates the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="updateModel">The update model.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">updateModel</exception>
        [HttpPut]
        [Route("stations/{id}")]
        [ProducesResponseType(200, Type = typeof(void))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateStation([FromRoute] Guid id, [FromBody] UpdateStationModel updateModel)
        {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel));
            }

            var result = await this.stationsManager.UpdateStation(id, updateModel.ToOptions());

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Removes the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("stations/{id}")]
        [ProducesResponseType(200, Type = typeof(void))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveStation([FromRoute] Guid id)
        {
            var result = await this.stationsManager.RemoveStation(id);

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok();
        }
    }
}
