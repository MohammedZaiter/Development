

namespace GreenFlux.Charging.Groups
{
    using System;

    public sealed class CreateOrUpdateConnectorOptions
    {
        public long MaxCurrent
        {
            get;
            set;
        }

        public Guid StationId
        {
            get;
            set;
        }
    }
}
