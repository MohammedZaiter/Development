
namespace GreenSystem.Charging.Groups
{
    using System;

    /// <summary>
    /// Connector domain class.
    /// </summary>
    public sealed class Connector
    {
        /// <summary>
        /// Gets or sets the id of the connector.
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id of the station.
        /// </summary>
        public Guid StationId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the max current of the connector.
        /// </summary>
        public long MaxCurrent
        {
            get;
            set;
        }
    }
}
