
namespace GreenSystem.Charging.Groups.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Create or update model for Group.
    /// </summary>
    public sealed class CreateOrUpdateGroupModel
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
        /// Gets or sets the capacity (=<1).
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        [Required]
        [Range(1, Double.MaxValue)]
        public long Capacity
        {
            get;
            set;
        }

        /// <summary>
        /// Converts to options class.
        /// </summary>
        /// <returns></returns>
        internal CreateOrUpdateGroupOptions ToOptions()
        {
            return new CreateOrUpdateGroupOptions()
            {
                Name = this.Name,
                Capacity = this.Capacity
            };
        }
    }
}
