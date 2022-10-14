

namespace GreenSystem.Charging.Groups
{
    using System;

    /// <summary>
    /// Create or update connector request options.
    /// </summary>
    public sealed class CreateOrUpdateConnectorOptions
    {
        /// <summary>
        /// Gets or sets the maximum current.
        /// </summary>
        public long MaxCurrent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the station id.
        /// </summary>
        public Guid StationId
        {
            get;
            set;
        }
    }
}
