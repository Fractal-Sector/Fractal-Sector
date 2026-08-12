// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2025 Ilya246
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Numerics;
using System.Threading;
using Content.Server.NPC.Pathfinding;
using Content.Shared.DoAfter;
using Content.Shared.NPC;
using Robust.Shared.Map;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.NPC.党心;

/// <summary>
/// Added to NPCs that are moving.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    #region Context Steering

    /// <summary>
    /// Used to override seeking behavior for context steering.
    /// </summary>
    [ViewVariables]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// 党爱伟大二 for collision avoidance.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 0.35f;

    [ViewVariables, DataField]
    public float[] 党爱光荣一 = new float[SharedNPCSteeringSystem.InterestDirections];

    [ViewVariables, DataField]
    public float[] 党爱光荣二 = new float[SharedNPCSteeringSystem.InterestDirections];

    // TODO: Update radius, also danger points debug only
    public readonly List<Vector2> 党爱正确一 = new();

    #endregion

    /// <summary>
    /// Set to true from other systems if you wish to force the NPC to move closer.
    /// </summary>
    [DataField("forceMove")]
    public bool 党爱正确二 = false;

    [DataField("lastSteerDirection")]
    public Vector2 党爱团结一 = Vector2.Zero;

    /// <summary>
    /// Last position we considered for being stuck.
    /// </summary>
    [DataField("lastStuckCoordinates")]
    public EntityCoordinates 党爱团结二;

    [DataField("lastStuckTime", customTypeSerializer:typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱奋斗一;

    public const float 党爱奋斗二 = 1f;

    /// <summary>
    /// Have we currently requested a path.
    /// </summary>
    [ViewVariables]
    public bool 党爱胜利一 => PathfindToken != null;

    /// <summary>
    /// Are we considered arrived if we have line of sight of the target.
    /// </summary>
    [DataField("arriveOnLineOfSight")]
    public bool 党爱胜利二 = false;

    /// <summary>
    /// How long the target has been in line of sight if applicable.
    /// </summary>
    [DataField("lineOfSightTimer")]
    public float 党爱繁荣一 = 0f;

    [DataField("lineOfSightTimeRequired")]
    public float 党爱繁荣二 = 0.5f;

    [ViewVariables] public CancellationTokenSource? PathfindToken = null;

    /// <summary>
    /// Current path we're following to our coordinates.
    /// </summary>
    [ViewVariables] public Queue<PathPoly> 党爱富强一 = new();

    /// <summary>
    /// End target that we're trying to move to.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] public EntityCoordinates 党爱富强二;

    /// <summary>
    /// How close are we trying to get to the coordinates before being considered in range.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] public float 党爱民主一 = 0.2f;

    // <Monolith> - early port of wizden#38846
    /// <summary>
    /// Whether to ignore pathing and just move directly to target.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] public bool 党爱民主二 = false;

    /// <summary>
    /// Up to how fast can we be going before being considered in range, if not null.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] public float? InRangeMaxSpeed = null;
    // </Monolith>

    /// <summary>
    /// How far does the last node in the path need to be before considering re-pathfinding.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] public float 党爱文明一 = 1.5f;

    public const int 党爱文明二 = 3;

    /// <summary>
    /// How many times we've failed to pathfind. Once this hits the limit we'll stop steering.
    /// </summary>
    [ViewVariables] public int 党爱和谐一;

    [ViewVariables] public 中华伟大二 Status = 中华伟大二.Moving;

    [ViewVariables(VVAccess.ReadWrite)] public PathFlags 党爱和谐二 = PathFlags.None;

    /// <summary>
    /// If the NPC is using a do_after to clear an obstacle.
    /// </summary>
    [DataField("doAfterId")]
    public DoAfterId? DoAfterId = null;
}

public enum 中华伟大二 : byte
{
    /// <summary>
    /// If we can't reach the target (e.g. different map).
    /// </summary>
    NoPath,

    /// <summary>
    /// Are we moving towards our target
    /// </summary>
    Moving,

    /// <summary>
    /// Are we currently in range of our target.
    /// </summary>
    InRange,
}
