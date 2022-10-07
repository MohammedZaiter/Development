
namespace GreenFlux.Charging.Groups
{
    using System;

    public sealed class Connector
    {
        public int Id
        {
            get;
            set;
        }

        public Guid StationId
        {
            get;
            set;
        }

        public long MaxCurrent
        {
            get;
            set;
        }
    }
}
