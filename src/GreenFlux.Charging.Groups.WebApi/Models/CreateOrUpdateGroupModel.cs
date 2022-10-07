
namespace GreenFlux.Charging.Groups.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    public sealed class CreateOrUpdateGroupModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name
        {
            get;
            set;
        }

        [Required]
        [Range(1, Double.MaxValue)]
        public long Capacity
        {
            get;
            set;
        }

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
