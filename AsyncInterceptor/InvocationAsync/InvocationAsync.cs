using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Deserunt.DynamicProxy
{
    internal sealed class InvocationAsync<TResult> : InvocationAsyncBase
    {
        private TResult _result;
        private readonly IInvocation _invocation;
        private readonly TaskCompletionSource<TResult> _tcs;
        private object _returnValue;

        public InvocationAsync(IInvocation invocation) : base(invocation)
        {
            _tcs = new TaskCompletionSource<TResult>();
            _invocation = invocation;
            _returnValue = _invocation.ReturnValue;
            _invocation.ReturnValue = _tcs.Task;
        }

        public override Task Task => _tcs.Task;

        public override object ReturnValue
        {
            get => _returnValue;
            set
            {
                if (value.GetType() == typeof(TResult))
                {
                    _returnValue = Task.FromResult((TResult)value);
                }
                else
                {
                    _returnValue = (Task<TResult>)value;
                }

                _invocation.ReturnValue = _returnValue;
            }
        }

        protected override async Task Capture()
        {
            _returnValue = (Task<TResult>)_invocation.ReturnValue;
            _invocation.ReturnValue = Task;

            if (_returnValue != Task)
            {
                _result = await ((Task<TResult>)_returnValue).ConfigureAwait(false);
            }
        }

        protected override void Reject(Exception ex)
        {
            _tcs.SetException(ex);
        }

        protected override async Task Resolve()
        {
            await Capture().ConfigureAwait(false);
            _tcs.SetResult(_result);
        }
    }
}
