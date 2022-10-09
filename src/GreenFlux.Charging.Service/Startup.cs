
namespace Charging.Group.Service
{
    using GreenFlux.Charging.Caching.Redis;
    using GreenFlux.Charging.Store;
    using Microsoft.OpenApi.Models;

    public sealed class Startup
    {
        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            WebHostEnvironment = webHostEnvironment;
            Configuration = configuration;
        }

        public IWebHostEnvironment WebHostEnvironment
        {
            get;
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();

            var storeOptions = new DataStoreOptions
            {
                ConnectionString = this.Configuration["GREENFLUX_CONNECTIONSTRING"]
            };
            services
                .AddServices()
                .AddStore(storeOptions);

            var cacheConnectionString = this.Configuration["REDIS_CONNECTIONSTRING"];

            services
                .AddDistributedMemoryCache()
                .AddCaching(cacheConnectionString);

            if (this.WebHostEnvironment.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Charging Service", Version = "v1" });
                });
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Charging Service (v1)"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (this.WebHostEnvironment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        "/swagger/v1/swagger.json",
                        $"Charging Service (v1) - ({this.WebHostEnvironment.EnvironmentName})");
                });
            }

        }
    }
}
