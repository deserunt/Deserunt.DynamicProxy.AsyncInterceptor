using System;
using System.Threading.Tasks;
using AsyncInterceptor.Tests.InterfaceProxies;
using Castle.DynamicProxy;
using Xunit;

namespace AsyncInterceptor.Tests
{
    public static class ProxyFactory 
    {
        public static (IInterfaceToProxy, Logger) Factory(Timing beforeProceedTiming = Timing.Sync, Timing afterProceedTiming = Timing.Sync)
        {
            var logger = new Logger();
            var target = new ClassWithInterfaceToProxy(logger);
            var interceptor = new TestInterceptorAsync(logger, new Delays(beforeProceedTiming, afterProceedTiming));

            var proxyGenerator = new ProxyGenerator();
            var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<IInterfaceToProxy>(target, interceptor);

            return (proxy, logger);
        }
    }

    public class InterceptorAsync_AsynchronousResultMethodShould
    {
        [Theory]
        [InlineData(Timing.Sync, Timing.Sync)]
        [InlineData(Timing.Sync, Timing.Async)]
        [InlineData(Timing.Async, Timing.Async)]
        [InlineData(Timing.Async, Timing.Sync)]
        public async Task LogFiveEntries(Timing beforeProceedTiming, Timing afterProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming, afterProceedTiming);
            var guid = await proxy.AsynchronousResultMethod();

            Assert.NotEqual(Guid.Empty, guid);
            Assert.Equal(5, logger.Count);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowInterceptionPriorToInvocation(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);
            await proxy.AsynchronousResultMethod().ConfigureAwait(false);

            Assert.Equal($"{nameof(IInterfaceToProxy.AsynchronousResultMethod)}:InterceptStart", logger[0]);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowInterceptionAfterProceed(Timing afterProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(afterProceedTiming: afterProceedTiming);

            await proxy.AsynchronousResultMethod();

            Assert.Equal($"{nameof(IInterfaceToProxy.AsynchronousResultMethod)}:InterceptEnd", logger[4]);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowResultToBeResolvedAfterProceed(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);

            var result = await proxy.AsynchronousResultMethod();

            Assert.Equal($"{nameof(IInterfaceToProxy.AsynchronousResultMethod)}:{result}", logger[3]);
        }
    }

    public class InterceptorAsync_AsynchronousVoidMethodShould
    {
        [Theory]
        [InlineData(Timing.Sync, Timing.Sync)]
        [InlineData(Timing.Sync, Timing.Async)]
        [InlineData(Timing.Async, Timing.Async)]
        [InlineData(Timing.Async, Timing.Sync)]
        public async Task LogFiveEntries(Timing beforeProceedTiming, Timing afterProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming, afterProceedTiming);
            await proxy.AsynchronousVoidMethod();

            Assert.Equal(5, logger.Count);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowInterceptionPriorToInvocation(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);
            await proxy.AsynchronousVoidMethod().ConfigureAwait(false);

            Assert.Equal($"{nameof(IInterfaceToProxy.AsynchronousVoidMethod)}:InterceptStart", logger[0]);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowInterceptionAfterProceed(Timing afterProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(afterProceedTiming: afterProceedTiming);

            await proxy.AsynchronousVoidMethod();

            Assert.Equal($"{nameof(IInterfaceToProxy.AsynchronousVoidMethod)}:InterceptEnd", logger[4]);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowResultToBeResolvedAfterProceed(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);

            await proxy.AsynchronousVoidMethod();

            Assert.Equal($"{nameof(IInterfaceToProxy.AsynchronousVoidMethod)}:Void", logger[3]);
        }
    }

    public class InterceptorAsync_AsynchronousVoidExceptionMethodShould
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidExceptionMethod);

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task LogThreeEntries(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);
            var ex = await Assert.ThrowsAsync<TestException>(proxy.AsynchronousVoidExceptionMethod);

            Assert.Equal(3, logger.Count);

            Assert.NotNull(ex);
            Assert.Equal($"{MethodName}:Start", logger[1]);
            Assert.Equal($"{MethodName}:Exception", ex.Message);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowProcessingPriorToInvocation(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);
            await Assert.ThrowsAsync<TestException>(proxy.AsynchronousVoidExceptionMethod);

            Assert.Equal($"{MethodName}:InterceptStart", logger[0]);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowExceptionHandling(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);
            var ex = await Assert.ThrowsAsync<TestException>(proxy.AsynchronousVoidExceptionMethod);

            Assert.Equal($"{MethodName}:InterceptException:{ex.Message}", logger[2]);
        }
    }

    public class InterceptorAsync_AsynchronousResultExceptionMethodShould
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultExceptionMethod);

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task LogThreeEntries(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);
            var ex = await Assert.ThrowsAsync<TestException>(proxy.AsynchronousResultExceptionMethod);

            Assert.Equal(3, logger.Count);

            Assert.NotNull(ex);
            Assert.Equal($"{MethodName}:Start", logger[1]);
            Assert.Equal($"{MethodName}:Exception", ex.Message);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowProcessingPriorToInvocation(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);

            var ex = await Assert.ThrowsAsync<TestException>(proxy.AsynchronousResultExceptionMethod);

            Assert.Equal($"{MethodName}:InterceptStart", logger[0]);
        }

        [Theory]
        [InlineData(Timing.Sync)]
        [InlineData(Timing.Async)]
        public async Task AllowExceptionHandling(Timing beforeProceedTiming)
        {
            var (proxy, logger) = ProxyFactory.Factory(beforeProceedTiming);
            var ex = await Assert.ThrowsAsync<TestException>(proxy.AsynchronousResultExceptionMethod);

            Assert.Equal($"{MethodName}:InterceptException:{ex.Message}", logger[2]);
        }
    }
}
