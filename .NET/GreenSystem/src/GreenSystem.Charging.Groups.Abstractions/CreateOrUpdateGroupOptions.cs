
namespace GreenSystem.Charging.Groups
{
    /// <summary>
    /// Create or update Group request options.
    /// </summary>
    public sealed class CreateOrUpdateGroupOptions
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
        /// Gets or sets the capacity.
        /// </summary>
        public long Capacity
        {
            get;
            set;
        }
    }
}
