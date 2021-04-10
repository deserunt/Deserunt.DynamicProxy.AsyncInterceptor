using System;
using System.Threading.Tasks;

namespace AsyncInterceptor.Tests.InterfaceProxies
{
    public class ClassWithInterfaceToProxy : IInterfaceToProxy
    {
        private readonly Logger _log;

        public ClassWithInterfaceToProxy(Logger logger)
        {
            _log = logger;
        }

        public async Task<Guid> AsynchronousResultExceptionMethod()
        {
            _log.Add($"{nameof(AsynchronousResultExceptionMethod)}:Start");
            await Task.Delay(10).ConfigureAwait(false);
            throw new TestException($"{nameof(AsynchronousResultExceptionMethod)}:Exception");
        }

        public async Task<Guid> AsynchronousResultMethod()
        {
            _log.Add($"{nameof(AsynchronousResultMethod)}:Start");
            await Task.Delay(10).ConfigureAwait(false);
            _log.Add($"{nameof(AsynchronousResultMethod)}:End");
            return Guid.NewGuid();
        }

        public async Task AsynchronousVoidExceptionMethod()
        {
            _log.Add($"{nameof(AsynchronousVoidExceptionMethod)}:Start");
            await Task.Delay(10).ConfigureAwait(false);
            throw new TestException($"{nameof(AsynchronousVoidExceptionMethod)}:Exception");
        }

        public async Task AsynchronousVoidMethod()
        {
            _log.Add($"{nameof(AsynchronousVoidMethod)}:Start");
            await Task.Delay(10).ConfigureAwait(false);
            _log.Add($"{nameof(AsynchronousVoidMethod)}:End");
        }
    }
}