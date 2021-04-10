using System;
using System.Threading.Tasks;

namespace AsyncInterceptor.Tests.InterfaceProxies
{
    public interface IInterfaceToProxy
    {
        Task AsynchronousVoidMethod();

        Task AsynchronousVoidExceptionMethod();

        Task<Guid> AsynchronousResultMethod();

        Task<Guid> AsynchronousResultExceptionMethod();

    }
}