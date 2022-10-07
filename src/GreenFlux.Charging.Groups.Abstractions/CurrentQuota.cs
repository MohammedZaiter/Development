
namespace GreenFlux.Charging.Groups
{
    public sealed class CurrentQuota
    {
        public long GroupCapacity
        {
            get;
            set;
        }

        public long ConsumedCapacity
        {
            get;
            set;
        }
    }
}
