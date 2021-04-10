using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Deserunt.DynamicProxy
{
    public abstract class InterceptorAsync : IInterceptor
    {
        void IInterceptor.Intercept(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;

            if (!typeof(Task).IsAssignableFrom(returnType))
            {
                Intercept(invocation);
            }
            else
            {
                InvokeIntercept(invocation).ConfigureAwait(false);
            }
       }

        /// <summary>
        /// Intercepts a synchronous method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public virtual void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }

        /// <summary>
        /// Intercepts a asynchronous method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public virtual Task Intercept(IInvocationAsync invocation) 
        {
            return invocation.Proceed();
        }

        private async Task InvokeIntercept(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            var invocationAsync = returnType.CreateInvocationAsync(invocation);

            try
            {
                await Intercept(invocationAsync);
                await invocationAsync.TryResolve();
            }
            catch (Exception ex)
            {
                if (invocationAsync.TryReject(ex) == false)
                {
                    throw;
                }
            }
        }
    }
}
