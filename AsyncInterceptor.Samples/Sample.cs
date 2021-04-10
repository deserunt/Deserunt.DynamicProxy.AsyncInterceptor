using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Moq;
using Xunit;

namespace AsyncInterceptor.Samples
{
    public class Sample
    {
        [Fact]
        public async Task InterceptTaskResult() 
        {
            var target = Mock.Of<IService>();

            var proxyGenerator = new ProxyGenerator();

            var interceptor = new TaskResultInterceptor();

            var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<IService>(target, interceptor);

            var result = await proxy.GetAsync(41);

            Assert.Equal(42, result);
        }

        [Fact]
        public async Task InterceptTaskVoid()
        {
            var target = Mock.Of<IService>();

            var targetMock = Mock.Get(target);
            targetMock.Setup(x => x.DoAsync()).Returns(() => Task.Delay(100));

            var proxyGenerator = new ProxyGenerator();
            var interceptor = new TaskVoidInterceptor();

            var proxy = proxyGenerator.CreateInterfaceProxyWithTarget(target, interceptor);

            // invoke the method

            await proxy.DoAsync();

            // verify method was called
            targetMock.Verify(x => x.DoAsync(), Times.Once());
        }
    }
}