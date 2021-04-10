using System;
using System.Threading.Tasks;

namespace AsyncInterceptor.Tests.InterfaceProxies
{
    public static class Wait
    {
        public static async Task For(Timing timing)
        {
            if (timing == Timing.Async)
            {
                await Task.Yield();
                await Task.Delay(10);
            }
        }  
    }
}