
namespace GreenFlux.Charging.Group.WebApi
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