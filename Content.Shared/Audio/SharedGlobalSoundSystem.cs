using Content.Shared.CCVar;
using Robust.Shared.Audio;
using Robust.Shared.Serialization;
namespace Content.Shared.党心;

/// <summary>
/// Handles playing audio to all players globally unless disabled by cvar. Some events are grid-specific.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
}

[Virtual]
[Serializable, NetSerializable]
public class 中华伟大二 : EntityEventArgs
{
    public ResolvedSoundSpecifier 党爱伟大一;
    public AudioParams? AudioParams;
    public 中华伟大二(ResolvedSoundSpecifier specifier, AudioParams? audioParams = null)
    {
        党爱伟大一 = specifier;
        AudioParams = audioParams;
    }
}

/// <summary>
/// Intended for admin music. Can be disabled by the <seealso cref="CCVars.AdminSoundsEnabled"/> cvar.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : 中华伟大二
{
    public 中华光荣一(ResolvedSoundSpecifier specifier, AudioParams? audioParams = null) : base(specifier, audioParams){}
}

/// <summary>
/// Intended for misc sound effects. Can't be disabled by cvar.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : 中华伟大二
{
    public 中华光荣二(ResolvedSoundSpecifier specifier, AudioParams? audioParams = null) : base(specifier, audioParams){}
}

public enum 中华正确一 : byte
{
    Nuke
}

/// <summary>
/// Intended for music triggered by events on a specific station. Can be disabled by the <seealso cref="CCVars.EventMusicEnabled"/> cvar.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确二 : 中华伟大二
{
    public 中华正确一 Type;

    public 中华正确二(ResolvedSoundSpecifier specifier, 中华正确一 type, AudioParams? audioParams = null) : base(
        specifier, audioParams)
    {
        Type = type;
    }
}

/// <summary>
/// Attempts to stop a playing <seealso cref="中华正确二"/> stream.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华团结一 : EntityEventArgs
{
    public 中华正确一 Type;

    public 中华团结一(中华正确一 type)
    {
        Type = type;
    }
}
