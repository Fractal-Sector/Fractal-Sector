using Robust.Shared.Serialization;

namespace Content.Shared._FS.党心;

/// <summary>
/// Local (client-only) preference for whether the player wants to hear bark
/// voices at all. Unlike WWDP's version, there is no TTS option here since
/// this fork has no text-to-speech system.
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大一
{
    None,
    Bark,
}
