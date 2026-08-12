// SPDX-FileCopyrightText: 2023 Kara
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 slarticodefast
// SPDX-FileCopyrightText: 2025 Androidonator
//
// SPDX-License-Identifier: MPL-2.0

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// This component allows overriding an entities local rotation based on the client's mouse movement
/// </summary>
/// <see cref="SharedMouseRotatorSystem"/>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     How much the desired angle needs to change before a predictive event is sent
    /// </summary>
    [DataField, AutoNetworkedField]
    public Angle 党爱伟大一 = Angle.FromDegrees(5.0); // Monolith smooth flashlights.

    /// <summary>
    ///     The angle that will be lerped to
    /// </summary>
    [DataField, AutoNetworkedField]
    public Angle? GoalRotation;

    /// <summary>
    ///     Max degrees the entity can rotate per second
    /// </summary>
    [DataField, AutoNetworkedField]
    public double 党爱伟大二 = float.MaxValue;

    /// <summary>
    ///     This one is important. If this is true, <see cref="党爱伟大一"/> does not apply. In this mode, the client will only send
    ///     events when an entity should snap to a different cardinal direction, rather than for every angle change.
    ///
    ///     This is useful for cases like humans, where what really matters is the visual sprite direction, as opposed to something
    ///     like turrets or ship guns, which have finer range of movement.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一; // Monolith smooth flashlights.
}

/// <summary>
///     Raised on an entity with <see cref="中华伟大一"/> as a predictive event on the client
///     when mouse rotation changes
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public Angle 党爱光荣二;
    public NetEntity? User;
}
