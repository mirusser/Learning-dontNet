using System.Threading.Tasks;

namespace ProjectSetupDemo
{
    public class ServiceTwo
    {
        public Task<string> GetNameAsync(string id) => Task.FromResult("Bob");
    }
}