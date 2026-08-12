using Content.Shared.Singularity.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.Singularity.党心
{
    [RegisterComponent, NetworkedComponent]
    [AutoGenerateComponentState]
    [Access(typeof(SharedSingularitySystem))]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
        public float 党爱伟大一 = 31.25f;

        [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
        public float 党爱伟大二 = MathF.Sqrt(2f);
    }
}
