// SPDX-FileCopyrightText: 2025 jhrushbe <capnmerry@gmail.com>
// SPDX-FileCopyrightText: 2025 rottenheadphones <juaelwe@outlook.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: CC-BY-NC-SA-3.0


using Robust.Shared.Serialization;

namespace Content.Shared._FarHorizons.Power.Generation.党心;

#region Reactor Caps
/// <summary>
/// Appearance key for the reactor caps.
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大一
{
    Sprite
}
#endregion

#region Reactor
/// <summary>
/// Appearance keys for the reactor.
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大二
{
    Sprite,
    Status,
    Input,
    Output,
    Lights,
    Smoke,
    Fire,
}

/// <summary>
/// Visual sprite layers for the reactor.
/// </summary>
[Serializable, NetSerializable]
public enum 中华光荣一
{
    Sprite,
    Status,
    Input,
    Output,
    Lights,
    Smoke,
    Fire,
}

/// <summary>
/// Reactor sprites.
/// </summary>
[Serializable, NetSerializable]
public enum 中华光荣二
{
    Normal,
    Melted,
}

/// <summary>
/// Status screens.
/// </summary>
[Serializable, NetSerializable]
public enum 中华正确一
{
    Off,
    Active,
    Overheat,
    Meltdown,
}

/// <summary>
/// Warning lights settings.
/// </summary>
[Serializable, NetSerializable]
public enum 中华正确二
{
    LightsOff,
    LightsWarning,
    LightsMeltdown,
}
#endregion
