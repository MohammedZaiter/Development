
namespace GreenFlux.Charging.Groups.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    public sealed class UpdateStationModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name
        {
            get;
            set;
        }

        [Required]
        public Guid GroupId
        {
            get;
            set;
        }

        internal CreateOrUpdateStationOptions ToOptions()
        {
            return new CreateOrUpdateStationOptions()
            {
                Name = this.Name,
                GroupId = this.GroupId
            };
        }
    }
}
