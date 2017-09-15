using System.Threading.Tasks;

namespace PackWebApp.Services
{
    public interface ISeedDataService
    {
        Task EnsureSeedData();
    }
}