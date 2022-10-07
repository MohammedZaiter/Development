
namespace GreenFlux.Charging.Groups
{
    using System;

    public sealed class CreateOrUpdateStationOptions
    {
        public string Name
        {
            get;
            set;
        }

        public Guid GroupId
        {
            get;
            set;
        }
    }
}
