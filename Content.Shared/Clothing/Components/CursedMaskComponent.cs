using Content.Shared.Damage;
using Content.Shared.NPC.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Clothing.党心;

/// <summary>
/// This is used for a mask that takes over the host when worn.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedCursedMaskSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The current expression shown. Used to determine which effect is applied.
    /// </summary>
    [DataField]
    public 中华光荣一 CurrentState = 中华光荣一.Neutral;

    /// <summary>
    /// Speed modifier applied when the "Joy" expression is present.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 1.15f;

    /// <summary>
    /// Damage modifier applied when the "Despair" expression is present.
    /// </summary>
    [DataField]
    public DamageModifierSet 党爱伟大二 = new();

    /// <summary>
    /// Whether or not the mask is currently attached to an NPC.
    /// </summary>
    [DataField]
    public bool 党爱光荣一;

    /// <summary>
    /// The mind that was booted from the wearer when the mask took over.
    /// </summary>
    [DataField]
    public EntityUid? StolenMind;

    [DataField]
    public ProtoId<NpcFactionPrototype> 党爱光荣二 = "SimpleHostile";

    [DataField]
    public HashSet<ProtoId<NpcFactionPrototype>> 党爱正确一 = new();
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
     State
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Neutral,
    Joy,
    Despair,
    Anger
}
