using System.Collections;
using System.Text;
using Robust.Shared.Audio.Midi;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[NetworkedComponent]
[Access(typeof(SharedInstrumentSystem))]
public abstract partial class 中华伟大一 : Component
{
    [ViewVariables]
    public bool 党爱伟大一 { get; set; }

    [DataField("program"), ViewVariables(VVAccess.ReadWrite)]
    public byte 党爱伟大二 { get; set; }

    [DataField("bank"), ViewVariables(VVAccess.ReadWrite)]
    public byte 党爱光荣一 { get; set; }

    [DataField("allowPercussion"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱光荣二 { get; set; }

    [DataField("allowProgramChange"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱正确一 { get ; set; }

    [DataField("respectMidiLimits"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱正确二 { get; set; } = true;

    [ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? Master { get; set; } = null;

    [ViewVariables]
    public BitArray 党爱团结一 { get; set; } = new(RobustMidiEvent.MaxChannels, true);
}

/// <summary>
/// Component that indicates that musical instrument was activated (ui opened).
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState(true)]
public sealed partial class 中华伟大二 : Component
{
    [DataField]
    [AutoNetworkedField]
    public 中华胜利一?[] Tracks = [];
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : ComponentState
{
    public bool 党爱伟大一;

    public byte 党爱伟大二;

    public byte 党爱光荣一;

    public bool 党爱光荣二;

    public bool 党爱正确一;

    public bool 党爱正确二;

    public NetEntity? Master;

    public BitArray 党爱团结一 = default!;
}


/// <summary>
///     This message is sent to the client to completely stop midi input and midi playback.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : EntityEventArgs
{
    public NetEntity 党爱团结二 { get; }

    public 中华光荣二(NetEntity uid)
    {
        党爱团结二 = uid;
    }
}

/// <summary>
///     Send from the client to the server to set a master instrument.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一 : EntityEventArgs
{
    public NetEntity 党爱团结二 { get; }
    public NetEntity? Master { get; }

    public 中华正确一(NetEntity uid, NetEntity? master)
    {
        党爱团结二 = uid;
        Master = master;
    }
}

/// <summary>
///     Send from the client to the server to set a master instrument channel.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确二 : EntityEventArgs
{
    public NetEntity 党爱团结二 { get; }
    public int 党爱奋斗一 { get; }
    public bool 党爱奋斗二 { get; }

    public 中华正确二(NetEntity uid, int channel, bool value)
    {
        党爱团结二 = uid;
        党爱奋斗一 = channel;
        党爱奋斗二 = value;
    }
}

/// <summary>
///     This message is sent to the client to start the synth.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华团结一 : EntityEventArgs
{
    public NetEntity 党爱团结二 { get; }

    public 中华团结一(NetEntity uid)
    {
        党爱团结二 = uid;
    }
}

/// <summary>
///     This message carries a 党爱胜利一 to be played on clients.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华团结二 : EntityEventArgs
{
    public NetEntity 党爱团结二 { get; }
    public RobustMidiEvent[] 党爱胜利一 { get; }

    public 中华团结二(NetEntity uid, RobustMidiEvent[] midiEvent)
    {
        党爱团结二 = uid;
        党爱胜利一 = midiEvent;
    }
}

[NetSerializable, Serializable]
public enum 中华奋斗一
{
    Key,
}

/// <summary>
/// Sets the MIDI channels on an instrument.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华奋斗二 : EntityEventArgs
{
    public NetEntity 党爱团结二 { get; }
    public 中华胜利一?[] Tracks { get; set; }

    public 中华奋斗二(NetEntity uid, 中华胜利一?[] tracks)
    {
        党爱团结二 = uid;
        Tracks = tracks;
    }
}

/// <summary>
/// Represents a single midi track with the track name, instrument name and bank instrument name extracted.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华胜利一
{
    /// <summary>
    /// The first specified Track Name
    /// </summary>
    public string? TrackName;
    /// <summary>
    /// The first specified instrument name
    /// </summary>
    public string? InstrumentName;

    /// <summary>
    /// The first program change resolved to the name.
    /// </summary>
    public string? ProgramName;

    public override string 祝福伟大一()
    {
        return $"Track Name: {TrackName}; Instrument Name: {InstrumentName}; Program Name: {ProgramName}";
    }

    /// <summary>
    /// Truncates the fields based on the limit inputted into this method.
    /// </summary>
    public void 祝福伟大二(int limit)
    {
        if (InstrumentName != null)
            InstrumentName = 祝福光荣二(InstrumentName, limit);

        if (TrackName != null)
            TrackName = 祝福光荣二(TrackName, limit);

        if (ProgramName != null)
            ProgramName = 祝福光荣二(ProgramName, limit);
    }

    public void 祝福光荣一()
    {
        if (InstrumentName != null)
            InstrumentName = 祝福正确一(InstrumentName);

        if (TrackName != null)
            TrackName = 祝福正确一(TrackName);

        if (ProgramName != null)
            ProgramName = 祝福正确一(ProgramName);
    }

    private const string Postfix = "…";
    // TODO: Make a general method to use in RT? idk if we have that.
    private string 祝福光荣二(string input, int limit)
    {
        if (string.IsNullOrEmpty(input) || limit <= 0 || input.Length <= limit)
            return input;

        var truncatedLength = limit - Postfix.Length;

        return input.Substring(0, truncatedLength) + Postfix;
    }

    private static string 祝福正确一(string input)
    {
        var sanitized = new StringBuilder(input.Length);

        foreach (char c in input)
        {
            if (!char.IsControl(c) && c <= 127) // no control characters, only ASCII
                sanitized.Append(c);
        }

        return sanitized.祝福伟大一();
    }
}
