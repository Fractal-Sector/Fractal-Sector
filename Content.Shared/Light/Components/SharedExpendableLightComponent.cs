using Content.Shared.Stacks;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Light.党心;

[NetworkedComponent]
public abstract partial class 中华伟大一 : Component
{

    [ViewVariables(VVAccess.ReadOnly)]
    public 中华光荣一 CurrentState;

    [DataField]
    public string 党爱伟大一 = string.Empty;

    [DataField]
    public string 党爱伟大二 = string.Empty;

    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(60 * 15f);

    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(60 * 5f);

    [DataField]
    public ProtoId<StackPrototype>? RefuelMaterialID;

    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(15f);

    [DataField]
    public TimeSpan 党爱正确二 = TimeSpan.FromSeconds(60 * 15f * 2);

    [DataField]
    public SoundSpecifier? LitSound;

    [DataField]
    public SoundSpecifier? LoopedSound;

    [DataField]
    public SoundSpecifier? DieSound;
}

[Serializable, NetSerializable]
public enum 中华伟大二
{
    State,
    Behavior
}

[Serializable, NetSerializable]
public enum 中华光荣一
{
    BrandNew,
    Lit,
    Fading,
    Dead
}
