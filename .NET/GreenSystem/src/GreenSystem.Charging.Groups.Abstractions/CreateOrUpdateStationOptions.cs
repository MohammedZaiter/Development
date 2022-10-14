
namespace GreenSystem.Charging.Groups
{
    using System;

    /// <summary>
    /// Create or update station request options.
    /// </summary>
    public sealed class CreateOrUpdateStationOptions
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        public Guid GroupId
        {
            get;
            set;
        }
    }
}
