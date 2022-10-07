
namespace GreenFlux.Charging.Groups
{
    using System;

    public sealed class Station : IEquatable<Station>
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

        public Guid GroupId
        {
            get;
            set;
        }

        public long ConsumedCurrent
        {
            get;
            set;
        }

        public bool Equals(Station other)
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

            if (this.GroupId != other.GroupId)
            {
                return false;
            }

            if (this.ConsumedCurrent != other.ConsumedCurrent)
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
