using System;
using System.Threading.Tasks;
using Deserunt.DynamicProxy;

namespace AsyncInterceptor.Samples
{
    public class TaskVoidInterceptor : InterceptorAsync
    {
        public TaskVoidInterceptor()
        {
        }

        public override async Task Intercept(IInvocationAsync invocation)
        {
            // do some async work before proceed
            await Task.Yield();

            await invocation.Proceed();
        }
    }
}
