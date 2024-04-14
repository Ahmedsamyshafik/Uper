using Driver.Models;
using Driver.Repository.IRepo;
using Driver.Repository.Repo;

namespace Driver.Repository
{
    public static class ModuleRepoDependencies
    {
        public static IServiceCollection AddRepoDependencies(this IServiceCollection services)
        {
            services.AddTransient<IRequestDriveRepository, RequestDriveRepository>();
            services.AddTransient<ITripRepository, TripRepository>();

            return services;
        }
    }
}
