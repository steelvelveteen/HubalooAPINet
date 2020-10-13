using HubalooAPI.BLL;
using HubalooAPI.Dal.Auth;
using HubalooAPI.Dal.Database;
using HubalooAPI.Injection;
using HubalooAPI.Interfaces.BLL;
using HubalooAPI.Interfaces.Dal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HubalooAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // services.AddSingleton<IDatabase, DapperDatabase>();
            // services.AddSingleton<IAuthManager, AuthManager>();
            // services.AddSingleton<IAuthRepository, AuthRepository>();

            SetUpServiceProviders(services);
            var service = services.BuildServiceProvider();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SetUpServiceProviders(IServiceCollection services)
        {
            var productionService = new ProductionService(services);
        }
    }
}
