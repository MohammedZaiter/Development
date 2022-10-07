
namespace GreenFlux.Charging.Groups
{
    public sealed class CreateOrUpdateGroupOptions
    {
        public string Name
        {
            get;
            set;
        }

        public long Capacity
        {
            get;
            set;
        }
    }
}
