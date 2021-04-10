# AsyncInterceptor

Provides a simple [InterceptorAsync](AsyncInterceptor/InterceptorAsync.cs) base
class to facilate asynchronous interception with [Castle DynamicProxy](http://www.castleproject.org/projects/dynamicproxy/).

# Motivation

Provide a simple solution to asynchronous interception that is similar to
the `IInterceptor.cs` interface.

# Usage

```csharp
// calling code:
var result = await serviceProxy.GetAsync(41);
Assert.Equal(42, result);
```

```csharp
// Interception
using System.Threading.Tasks;
using Deserunt.DynamicProxy;

public class TaskResultInterceptor : InterceptorAsync
{
    public override async Task Intercept(IInvocationAsync invocation)
    {
        // do some async work
        await Task.Yield();

        // send the proceed
        await invocation.Proceed();

        // override the return value
        invocation.ReturnValue = (int)invocation.Arguments[0] + 1;
    }    
}
```

## Influenced By:

* [Castle.Core.AsyncInterceptor](https://github.com/JSkimming/Castle.Core.AsyncInterceptor)
* [stakx/DynamicProxy.AsyncInterceptor](https://github.com/stakx/DynamicProxy.AsyncInterceptor)
