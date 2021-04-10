using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;

namespace Deserunt.DynamicProxy
{
    internal static class InvocationAsyncCreateExtension
    {
        public static InvocationAsyncBase CreateInvocationAsync(this Type type, IInvocation invocation)
        {
            if (type.IsGenericType == false)
            {
                return new InvocationAsync(invocation);
            }

            var genericParamType = type.GetGenericArguments()[0];
            var invocationAsyncType = typeof(InvocationAsync<>).MakeGenericType(genericParamType);
            var invocationAsync = Activator.CreateInstance(invocationAsyncType, invocation);
            return (InvocationAsyncBase)invocationAsync;
        }
    }
}
