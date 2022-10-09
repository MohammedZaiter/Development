
namespace GreenFlux.Charging.Setup
{
    using Microsoft.SqlServer.Dac;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class DatabaseSetup
    {
        public static void Setup()
        {
            var dacOptions = new DacDeployOptions
            {
                CreateNewDatabase = true
            };

            var assembly = Assembly.GetExecutingAssembly();

            var resourceStream = assembly.GetManifestResourceStream("GreenFlux.Charging.Setup.Sql.Create_1_0.dacpac");

            using var dacPac = DacPackage.Load(resourceStream);

            var connectionString = PretifyConnectionString(
                Environment.GetEnvironmentVariable("GREENFLUX_CONNECTIONSTRING"));

            var dacServices = new DacServices(connectionString);
            dacServices.Deploy(dacPac, "greenflux", true, dacOptions);
        }

        private static string PretifyConnectionString(string connectionString)
        {
            var parts = connectionString.Split(";").ToList();

            parts.Remove("Initial Catalog");

            return string.Join(";", parts);
        }
    }
}
