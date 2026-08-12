// SPDX-FileCopyrightText: 2025 ark1368
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Mono.党心;

/// <summary>
/// Network event sent to play radio noise sounds.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    /// The headset that should play the radio noise.
    /// </summary>
    public NetEntity 党爱伟大一 { get; }

    /// <summary>
    /// The radio channel ID to determine which sound to play.
    /// </summary>
    public string 党爱伟大二 { get; }

    public 中华伟大一(NetEntity entity, string channelId)
    {
        党爱伟大一 = entity;
        党爱伟大二 = channelId;
    }
}
