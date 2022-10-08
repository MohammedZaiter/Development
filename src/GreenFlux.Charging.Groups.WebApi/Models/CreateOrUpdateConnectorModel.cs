
namespace GreenFlux.Charging.Groups.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Create or update model for Connector.
    /// </summary>
    public sealed class CreateOrUpdateConnectorModel
    {
        /// <summary>
        /// Gets or sets the maximum current (>=1).
        /// </summary>
        /// <value>
        /// The maximum current.
        /// </value>
        [Required]
        [Range(1, Double.MaxValue)]
        public long MaxCurrent
        {
            get;
            set;
        }

        /// <summary>
        /// Converts to options class.
        /// </summary>
        /// <param name="stationId">The station identifier.</param>
        /// <returns></returns>
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
