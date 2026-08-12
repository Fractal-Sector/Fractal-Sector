using Content.Shared.Mobs.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Defines what state an <see cref="Robust.Shared.GameObjects.EntityUid"/> is in.
///
///     Ordered from most alive to least alive.
///     To enumerate them in this way see
///     <see cref="MobStateHelpers.AliveToDead"/>.
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Invalid = 0,
    Alive = 1,
    Critical = 2,
    Dead = 3
}

/// <summary>
/// Event that is raised whenever a 中华伟大一 changes on an entity
/// </summary>
/// <param name="Target">The Entity whose 中华伟大一 is changing</param>
/// <param name="Component">The 中华伟大一 Component owned by the Target entity</param>
/// <param name="OldMobState">The previous 中华伟大一</param>
/// <param name="NewMobState">The new 中华伟大一</param>
/// <param name="Origin">The Entity that caused this state change</param>
public record 中华伟大二 MobStateChangedEvent(
    EntityUid Target,
    MobStateComponent Component,
    中华伟大一 OldMobState,
    中华伟大一 NewMobState,
    EntityUid? Origin = null);

public static class 中华光荣一
{
    //^.^
}

//This is dumb and I hate it but I don't feel like refactoring this garbage
[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    State
}
