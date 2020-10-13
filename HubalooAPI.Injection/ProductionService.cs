using HubalooAPI.Dal.Auth;
using HubalooAPI.Dal.Database;
using HubalooAPI.Interfaces.BLL;
using HubalooAPI.Interfaces.Dal;
using Microsoft.Extensions.DependencyInjection;

namespace HubalooAPI.Injection
{
    public class ProductionService
    {
        public ProductionService(IServiceCollection services)
        {
            // services.AddSingleton<IDatabase, DapperDatabase>();
            // services.AddSingleton<IAuthManager, AuthManager>();
            // services.AddSingleton<IAuthRepository, AuthRepository>();
        }
    }
}