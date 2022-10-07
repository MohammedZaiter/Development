
namespace GreenFlux.Charging.Store
{
    using System;

    public class DataStore
    {
        protected readonly IConnectionManager connectionManager;

        protected DataStore(IConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

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

        protected static object NormalizeValue(Guid? value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value.Value;
            }

        }
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
