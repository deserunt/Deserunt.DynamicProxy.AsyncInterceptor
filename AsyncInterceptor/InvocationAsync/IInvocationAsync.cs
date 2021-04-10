using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Deserunt.DynamicProxy
{
    public interface IInvocationAsync
    {
        IReadOnlyList<object> Arguments { get; }

        IReadOnlyList<Type> GenericArguments { get; }

        object InvocationTarget { get; }

        MethodInfo Method { get; }

        MethodInfo MethodInvocationTarget { get; }

        object Proxy { get; }

        object ReturnValue { get; set; }

        Type TargetType { get; }

        object GetArgumentValue(int index);

        MethodInfo GetConcreteMethod();

        MethodInfo GetConcreteMethodInvocationTarget();

        Task Proceed();

        void SetArgumentValue(int index, object value);
    }
}
