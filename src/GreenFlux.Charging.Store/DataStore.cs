
namespace GreenFlux.Charging.Store
{
    using System;

    /// <summary>
    /// Store class for common store operations.
    /// </summary>
    public class DataStore
    {
        protected readonly IConnectionManager connectionManager;

        protected DataStore(IConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        /// <summary>
        /// Normalizes the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected static object NormalizeValue<T>(T value) where T : class
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Safes the cast.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected static T SafeCast<T>(object value)
        {
            if (value == DBNull.Value)
            {
                return default;
            }
            else
            {
                return (T)value;
            }
        }
    }
}
