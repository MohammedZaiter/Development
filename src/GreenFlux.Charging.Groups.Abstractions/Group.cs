
namespace GreenFlux.Charging.Groups
{
    using System;

    public sealed class Group : IEquatable<Group>
    {
        public Guid Id
        {
            get;
            set;
        }

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

        public long ConsumedCapacity
        {
            get;
            set;
        }

        public bool Equals(Group other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Id != other.Id)
            {
                return false;
            }

            if (!string.Equals(this.Name, other.Name, StringComparison.Ordinal))
            {
                return false;
            }

            if (this.Capacity != other.Capacity)
            {
                return false;
            }

            if (this.ConsumedCapacity != other.ConsumedCapacity)
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Group);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
