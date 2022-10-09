using Charging.Group.Service;
using GreenFlux.Charging.Setup;

if (IsSetupMode(args))
{
    DatabaseSetup.Setup();
}
else
{
    CreateHostBuilder(args).Build().Run();
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(builder =>
        {
            builder.AddEnvironmentVariables("GREENFLUX_");
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup(context => new Startup(context.HostingEnvironment, context.Configuration));
        });
}

static bool IsSetupMode(string[] args)
{
    for (int i = 0; i < args.Length; i++)
    {
        if (string.Equals(args[i], "-m", StringComparison.OrdinalIgnoreCase))
        {
            // check that i + i value is equal to 'setup'
            if (i + 1 < args.Length)
            {
                return string.Equals(args[i + 1], "setup", StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    return false;
}