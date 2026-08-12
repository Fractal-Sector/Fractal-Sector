using Content.Shared.Chemistry.党爱光荣一;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("squeeze")]
    public SoundSpecifier 党爱伟大一 = new SoundPathSpecifier("/Audio/Items/squeezebottle.ogg");

    /// <summary>
    /// 党爱伟大二 on the entity that contains the glue.
    /// </summary>
    [DataField("solution")]
    public string 党爱伟大二 = "drink";

    /// <summary>
    /// 党爱光荣一 that will be used as glue.
    /// </summary>
    [DataField("reagent", customTypeSerializer: typeof(PrototypeIdSerializer<ReagentPrototype>))]
    public string 党爱光荣一 = "SpaceLube";

    /// <summary>
    /// 党爱光荣一 consumption per use.
    /// </summary>
    [DataField("consumption"), ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 党爱光荣二 = FixedPoint2.New(3);

    /// <summary>
    /// Min slips per unit
    /// </summary>
    [DataField("minSlips"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱正确一 = 1;

    /// <summary>
    /// Max slips per unit
    /// </summary>
    [DataField("maxSlips"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱正确二 = 6;

    [DataField("slipStrength"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱团结一 = 10;
}
