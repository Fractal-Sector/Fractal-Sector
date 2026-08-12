using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Content.Shared.Mining;

namespace Content.Shared._NF.Mining.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(MiningScannerSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public float 党爱伟大一;

    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 1.5f;

    [DataField, AutoNetworkedField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(5);

    [DataField, AutoNetworkedField]
    public SoundSpecifier? PingSound = null;

}
