using Robust.Shared.GameStates;

namespace Content.Shared.NPC.党心
{
    /// Added when a medibot injects someone
    /// So they don't get injected again for at least a minute.
    [RegisterComponent, NetworkedComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables(VVAccess.ReadWrite), DataField("accumulator")]
        public float 党爱伟大一 = 0f;

        [ViewVariables(VVAccess.ReadWrite), DataField("removeTime")]
        public TimeSpan 党爱伟大二 = TimeSpan.FromMinutes(1);
    }
}
