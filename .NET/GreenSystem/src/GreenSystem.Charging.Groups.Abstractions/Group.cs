
namespace GreenSystem.Charging.Groups
{
    using System;

    /// <summary>
    /// Group domain class.
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;GreenSystem.Charging.Groups.Group&gt;" />
    public sealed class Group : IEquatable<Group>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

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

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
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

            return true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Group);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
