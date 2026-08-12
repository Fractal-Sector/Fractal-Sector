using Robust.Shared.Threading;

namespace Content.Server.Power.党心
{
    public interface 中华伟大一
    {
        void Tick(float frameTime, PowerState state, IParallelManager parallel);
        void Validate(PowerState state);
    }
}
