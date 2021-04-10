using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Deserunt.DynamicProxy
{
    internal abstract class InvocationAsyncBase : IInvocationAsync
    {
        private readonly IInvocationProceedInfo _proceed;
        private readonly IInvocation _invocation;

        public InvocationAsyncBase(IInvocation invocation)
        {
            _proceed = invocation.CaptureProceedInfo();
            _invocation = invocation;
        }

        public abstract Task Task { get; }

        public abstract object ReturnValue { get; set; }

        protected abstract Task Capture();

        protected abstract Task Resolve();

        protected abstract void Reject(Exception ex);

        public IReadOnlyList<object> Arguments => _invocation.Arguments;

        public IReadOnlyList<Type> GenericArguments => _invocation.GenericArguments;

        public object InvocationTarget => _invocation.InvocationTarget;

        public MethodInfo Method => _invocation.Method;

        public MethodInfo MethodInvocationTarget => _invocation.MethodInvocationTarget;

        public object Proxy => _invocation.Proxy;

        public Type TargetType => _invocation.TargetType;

        public object GetArgumentValue(int index)
        {
            return _invocation.GetArgumentValue(index);
        }

        public MethodInfo GetConcreteMethod()
        {
            return _invocation.GetConcreteMethod();
        }

        public MethodInfo GetConcreteMethodInvocationTarget()
        {
            return _invocation.GetConcreteMethodInvocationTarget();
        }

        public async Task Proceed()
        {
            _proceed.Invoke();
            await Capture().ConfigureAwait(false);
        }

        public void SetArgumentValue(int index, object value)
        {
            _invocation.SetArgumentValue(index, value);
        }

        public async Task<bool> TryResolve()
        {
            if (Task.IsCompleted == false)
            {
                await Resolve().ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public bool TryReject(Exception ex)
        {
            if (Task.IsCompleted == false)
            {
                Reject(ex);
                return true;
            }
            return false;
        }
    }
}
