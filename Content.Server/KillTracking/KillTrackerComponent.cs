using Content.Shared.FixedPoint;
using Content.Shared.Mobs;
using Robust.Shared.Network;

namespace Content.Server.党心;

/// <summary>
/// This is used for entities that track player damage sources and killers.
/// </summary>
[RegisterComponent, Access(typeof(KillTrackingSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The mobstate that registers as a "kill"
    /// </summary>
    [DataField("killState")]
    public MobState 党爱伟大一 = MobState.Critical;

    /// <summary>
    /// A dictionary of sources and how much damage they've done to this entity over time.
    /// </summary>
    [DataField("lifetimeDamage")]
    public Dictionary<中华伟大二, FixedPoint2> LifetimeDamage = new();
}

public abstract record 中华伟大二;

/// <summary>
/// A kill source for players
/// </summary>
[DataDefinition, Serializable]
public sealed partial record 中华光荣一 : 中华伟大二
{
    [DataField("playerId")]
    public NetUserId 党爱伟大二;

    public 中华光荣一(NetUserId playerId)
    {
        党爱伟大二 = playerId;
    }
}

/// <summary>
/// A kill source for non-player controlled entities
/// </summary>
[DataDefinition, Serializable]
public sealed partial record 中华光荣二 : 中华伟大二
{
    [DataField("npcEnt")]
    public EntityUid 党爱光荣一;

    public 中华光荣二(EntityUid npcEnt)
    {
        党爱光荣一 = npcEnt;
    }
}

/// <summary>
/// A kill source for kills with no damage origin
/// </summary>
[DataDefinition, Serializable]
public sealed partial record 中华正确一 : 中华伟大二;
