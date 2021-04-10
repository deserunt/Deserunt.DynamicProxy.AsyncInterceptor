using System.Threading.Tasks;

namespace AsyncInterceptor.Samples
{
    public interface IService
    {
        Task DoAsync();
        Task<int> GetAsync(int n);
    }
}