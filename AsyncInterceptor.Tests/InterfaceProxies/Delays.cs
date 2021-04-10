using System.Threading.Tasks;

namespace AsyncInterceptor.Tests.InterfaceProxies
{
    public class Delays
    {
        private readonly Timing _beforeProceedTiming;
        private readonly Timing _afterProceedTiming;
        public Task BeforeProceed() => Wait.For(_beforeProceedTiming);
        public Task AfterProceed() => Wait.For(_afterProceedTiming);

        public Delays(Timing beforeProceedTiming, Timing afterProceedTiming)
        {
            _beforeProceedTiming = beforeProceedTiming;
            _afterProceedTiming = afterProceedTiming;
        }
    }
}