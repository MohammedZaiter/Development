
namespace GreenFlux.Charging.Groups.WebApi
{
    using GreenFlux.Charging.Groups;
    using GreenFlux.Charging.Groups.WebApi.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public sealed class GroupsController : Controller
    {
        private readonly IGroupsManager groupsManager;
        private readonly IStationsManager stationsManager;

        public GroupsController(
            IGroupsManager groupsManager,
            IStationsManager stationsManager)
        {
            this.groupsManager = groupsManager ?? throw new ArgumentNullException(nameof(groupsManager));
            this.stationsManager = stationsManager ?? throw new ArgumentNullException(nameof(stationsManager));
        }

        /// <summary>
        /// Gets the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("groups/{id}")]
        [ProducesResponseType(200, Type = typeof(Group))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGroup([FromRoute] Guid id)
        {
            var result = await this.groupsManager.GetGroupById(id);

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Creates the group.
        /// </summary>
        /// <param name="createModel">The create model.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">createModel</exception>
        [HttpPost]
        [Route("groups")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateGroup([FromBody] CreateOrUpdateGroupModel createModel)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }

            var groupId = await this.groupsManager.CreateGroup(createModel.ToOptions());

            return Ok(groupId);
        }

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="updateModel">The update model.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">updateModel</exception>
        [HttpPut]
        [Route("groups/{id}")]
        [ProducesResponseType(200, Type = typeof(void))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateGroup([FromRoute] Guid id, [FromBody] CreateOrUpdateGroupModel updateModel)
        {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel));
            }

            var result = await this.groupsManager.UpdateGroup(id, updateModel.ToOptions());

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Removes the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("groups/{id}")]
        [ProducesResponseType(200, Type = typeof(void))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveGroup([FromRoute] Guid id)
        {
            var result = await this.groupsManager.RemoveGroup(id);

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Gets the stations by group identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("groups/{id}/stations")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Station>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStationsByGroupId([FromRoute] Guid id)
        {
            var result = await this.stationsManager.GetStationsByGroupId(id);

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Creates the station.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("groups/{id}/stations")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateStation([FromRoute] Guid id, [FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                this.ModelState.AddModelError("STATION_NAME_CANNOTBE_NULL", "Station name cannot be null or empty.");

                return BadRequest(this.ModelState);
            }

            if (name.Length > 30)
            {
                this.ModelState.AddModelError("STATION_NAME_EXCEEDS_LENGTH", "Station name cannot exeeds 30 characters.");

                return BadRequest(this.ModelState);
            }

            var result = await this.stationsManager.CreateStation(new CreateOrUpdateStationOptions()
            {
                GroupId = id,
                Name = name
            });

            if (!result.Success)
            {
                this.ModelState.AddModelError(result.Code, result.Description);

                return BadRequest(this.ModelState);
            }

            return Ok(result.Result);
        }
    }
}