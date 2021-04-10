using System;
using System.Threading.Tasks;
using Deserunt.DynamicProxy;

namespace AsyncInterceptor.Tests.InterfaceProxies
{
    public class TestInterceptorAsync : InterceptorAsync
    {
        private readonly Logger _log;
        private readonly Delays _delays;

        public TestInterceptorAsync(Logger logger, Delays delays)
        {
            _delays = delays;
            _log = logger;
        }

        public override async Task Intercept(IInvocationAsync invocation)
        {
            try
            {
                LogInterceptStart(invocation);

                await _delays.BeforeProceed().ConfigureAwait(false);

                await invocation.Proceed().ConfigureAwait(false);

                await _delays.AfterProceed().ConfigureAwait(false);

                await LogReturnValue((dynamic)invocation.ReturnValue, invocation).ConfigureAwait(false);

                LogInterceptEnd(invocation);
            }
            catch (Exception ex)
            {
                _log.Add($"{invocation.Method.Name}:InterceptException:{ex.Message}");
                throw;
            }
            finally
            {
                _log.Freeze();
            }
        }

        private async Task LogReturnValue(Task returnValue, IInvocationAsync invocation)
        {
            await returnValue;
            _log.Add($"{invocation.Method.Name}:Void");
        }

        private async Task LogReturnValue<TResult>(Task<TResult> returnValue, IInvocationAsync invocation)
        {
            var result = await returnValue;
            _log.Add($"{invocation.Method.Name}:{result}");
        }

        private void LogInterceptStart(IInvocationAsync invocation)
        {
            _log.Add($"{invocation.Method.Name}:InterceptStart");
        }

        private void LogInterceptEnd(IInvocationAsync invocation)
        {
            _log.Add($"{invocation.Method.Name}:InterceptEnd");
        }
    }
}