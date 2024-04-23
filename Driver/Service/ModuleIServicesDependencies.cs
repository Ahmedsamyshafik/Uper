using Driver.Service.IServices;
using Driver.Service.Services;

namespace Driver.Service
{
    public static class ModuleIServicesDependencies
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRequestDriveService, RequestDriveService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
