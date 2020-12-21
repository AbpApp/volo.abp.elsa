using NodaTime;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Elsa
{
    public class ElsaClock: IClock, ISingletonDependency
    {
        private readonly Volo.Abp.Timing.IClock _clock;
        public ElsaClock(Timing.IClock clock)
        {
            _clock = clock;
        }
        public Instant GetCurrentInstant()=> NodaConstants.BclEpoch.PlusTicks(_clock.Now.Ticks);
    }
}
