using Driver.Models;
using Driver.Repository.GenericRepo;

namespace Driver.Repository.IRepo
{
    public interface ITripRepository : IGenericRepositoryAsync<Trip>
    {
    }
}
