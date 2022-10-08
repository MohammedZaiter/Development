
namespace GreenFlux.Charging.Groups.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Update model for Station.
    /// </summary>
    public sealed class UpdateStationModel
    {
        /// <summary>
        /// Gets or sets the name (3=<length<=30).
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        [Required]
        public Guid GroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Converts to options class.
        /// </summary>
        /// <returns></returns>
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
