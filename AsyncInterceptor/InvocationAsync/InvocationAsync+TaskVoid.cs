using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Deserunt.DynamicProxy
{
    internal sealed class InvocationAsync : InvocationAsyncBase
    {
        private readonly IInvocation _invocation;
        private readonly TaskCompletionSource<TaskVoid> _tcs;
        private object _returnValue;

        public InvocationAsync(IInvocation invocation) : base(invocation)
        {
            _tcs = new TaskCompletionSource<TaskVoid>();
            _invocation = invocation;
            _returnValue = _invocation.ReturnValue;
            _invocation.ReturnValue = Task;
        }

        public override Task Task => _tcs.Task;

        public override object ReturnValue
        { 
            get => _returnValue;
            set
            {
                _returnValue = (Task)value;
                _invocation.ReturnValue = _returnValue; 
            }
        }

        protected override async Task Capture()
        {
            _returnValue = (Task)_invocation.ReturnValue;
            _invocation.ReturnValue = Task;

            if (_returnValue != Task)
            {
                await ((Task)_returnValue).ConfigureAwait(false);
            }
        }

        protected override async Task Resolve()
        {
            await Capture().ConfigureAwait(false);
            _tcs.SetResult(new TaskVoid());
        }

        protected override void Reject(Exception ex)
        {
            _tcs.SetException(ex);
        }
    }
}
