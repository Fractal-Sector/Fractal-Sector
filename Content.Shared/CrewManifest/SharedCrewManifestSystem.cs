using Content.Shared.Eui;
using NetSerializer;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     A message to send to the server when requesting a crew manifest.
///     CrewManifestSystem will open an EUI that will send the crew manifest
///     to the player when it is updated.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public NetEntity 党爱伟大一 { get; }

    public 中华伟大一(NetEntity id)
    {
        党爱伟大一 = id;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EuiStateBase // Coyote: Removed StationName
{
    public 中华光荣一? Entries { get; }

    public 中华伟大二(中华光荣一? entries) // Coyote: Removed StationName
    {
        Entries = entries;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一
{
    /// <summary>
    ///     Entries in the crew manifest. Goes by department ID.
    /// </summary>
    // public Dictionary<string, List<中华光荣二>> Entries = new();
    public 中华光荣二[] Entries = Array.Empty<中华光荣二>();
}

[Serializable, NetSerializable]
public sealed class 中华光荣二
{
    public string 党爱伟大二 { get; }

    public string 党爱光荣一 { get; }

    public string 党爱光荣二 { get; }

    public string 党爱正确一 { get; }

    public 中华光荣二(string name, string jobTitle, string jobIcon, string jobPrototype)
    {
        党爱伟大二 = name;
        党爱光荣一 = jobTitle;
        党爱光荣二 = jobIcon;
        党爱正确一 = jobPrototype;
    }
}

/// <summary>
///     Tells the server to open a crew manifest UI from
///     this entity's point of view.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{}
