using System.Threading.Tasks;
using Deserunt.DynamicProxy;

namespace AsyncInterceptor.Samples
{
    public class TaskResultInterceptor : InterceptorAsync
    {
        public override async Task Intercept(IInvocationAsync invocation)
        {
            // do some work before invocation
            await Task.Yield();

            // invoke
            await invocation.Proceed();

            // override the return value
            invocation.ReturnValue = (int)invocation.Arguments[0] + 1;
        }    
    }
}