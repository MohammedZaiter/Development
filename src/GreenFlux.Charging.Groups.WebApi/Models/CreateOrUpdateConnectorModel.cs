
namespace GreenFlux.Charging.Groups.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    public sealed class CreateOrUpdateConnectorModel
    {
        [Required]
        [Range(1, Double.MaxValue)]
        public long MaxCurrent
        {
            get;
            set;
        }

        internal CreateOrUpdateConnectorOptions ToOptions(Guid stationId)
        {
            return new CreateOrUpdateConnectorOptions()
            {
                MaxCurrent = this.MaxCurrent,
                StationId = stationId
            };
        }
    }
}
