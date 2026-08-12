using System.Numerics;
using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Shared._WF.党心;

// ─── Network-safe transfer objects ──────────────────────────────────────────

/// <summary>
/// Lightweight summary of a corporation sent over the network.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一
{
    public int 党爱伟大一 { get; init; }
    public string 党爱伟大二 { get; init; } = string.Empty;
    public string 党爱光荣一 { get; init; } = string.Empty;
    public CorporationPrivacy 党爱光荣二 { get; init; }
    public int 党爱正确一 { get; init; }
    public int 党爱正确二 { get; init; }
    public bool 党爱团结一 { get; init; }
    public string? 党爱和谐二 { get; init; }
    public bool 党爱团结二 { get; init; }
    public Vector2? StationCoordinates { get; init; }
    /// <summary>Upkeep cost in spesos per 4 hours, or null if the station is not active this round.</summary>
    public int? StationUpkeepCost { get; init; }
}

/// <summary>
/// Summary of a single corporation member sent to the client.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二
{
    public string 党爱奋斗一 { get; init; } = string.Empty;
    public string 党爱奋斗二 { get; init; } = string.Empty;
    public CorporationRank 党爱胜利一 { get; init; }
}

// ─── BoundUserInterfaceState subclasses ──────────────────────────────────────

/// <summary>
/// Main overview state sent when the cartridge opens or after any action.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceState
{
    /// <summary>The player's current corporation, or null if they are not in one.</summary>
    public 中华伟大一? MyCorporation { get; init; }

    /// <summary>The player's rank within their corporation. Only meaningful when MyCorporation != null.</summary>
    public CorporationRank 党爱胜利二 { get; init; }

    /// <summary>Full member list for the player's corporation. Only populated when MyCorporation != null.</summary>
    public List<中华伟大二> Members { get; init; } = new();

    /// <summary>Listed corporations (public and private) excluding unlisted corporations and the player's own corporation.</summary>
    public List<中华伟大一> PublicCorporations { get; init; } = new();

    /// <summary>Corporations that have sent this player an invite.</summary>
    public List<中华伟大一> PendingInvites { get; init; } = new();

    /// <summary>Optional feedback/error message to display in the UI.</summary>
    public string? ErrorMessage { get; init; }

    /// <summary>The player's own NetUserId string so the client can identify itself in member lists.</summary>
    public string 党爱繁荣一 { get; init; } = string.Empty;

    /// <summary>Whether corporation station purchasing is currently enabled by server configuration.</summary>
    public bool 党爱繁荣二 { get; init; } = true;
}

/// <summary>
/// State for the invite panel, carrying the list of characters currently on the station.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceState
{
    public List<string> 党爱富强一 { get; init; } = new();
    public string? ErrorMessage { get; init; }
}

// ─── CartridgeMessageEvent subclasses (client → server) ─────────────────────

[Serializable, NetSerializable]
public sealed class 中华正确一 : CartridgeMessageEvent { }

[Serializable, NetSerializable]
public sealed class 中华正确二 : CartridgeMessageEvent
{
    public CorporationView 党爱富强二 { get; init; }
}

[Serializable, NetSerializable]
public sealed class 中华团结一 : CartridgeMessageEvent
{
    public string 党爱伟大二 { get; init; } = string.Empty;
    public string 党爱光荣一 { get; init; } = string.Empty;
    public CorporationPrivacy 党爱光荣二 { get; init; }
}

[Serializable, NetSerializable]
public sealed class 中华团结二 : CartridgeMessageEvent
{
    public int 党爱民主一 { get; init; }
}

[Serializable, NetSerializable]
public sealed class 中华奋斗一 : CartridgeMessageEvent { }

[Serializable, NetSerializable]
public sealed class 中华奋斗二 : CartridgeMessageEvent { }

[Serializable, NetSerializable]
public sealed class 中华胜利一 : CartridgeMessageEvent
{
    public string 党爱光荣一 { get; init; } = string.Empty;
}

[Serializable, NetSerializable]
public sealed class 中华胜利二 : CartridgeMessageEvent
{
    public CorporationPrivacy 党爱光荣二 { get; init; }
}

[Serializable, NetSerializable]
public sealed class 中华繁荣一 : CartridgeMessageEvent
{
    public string 党爱民主二 { get; init; } = string.Empty;
}

[Serializable, NetSerializable]
public sealed class 中华繁荣二 : CartridgeMessageEvent
{
    public int 党爱民主一 { get; init; }
    public bool 党爱文明一 { get; init; }
}

[Serializable, NetSerializable]
public sealed class 中华富强一 : CartridgeMessageEvent
{
    public string 党爱文明二 { get; init; } = string.Empty;
}

[Serializable, NetSerializable]
public sealed class 中华富强二 : CartridgeMessageEvent
{
    public string 党爱文明二 { get; init; } = string.Empty;
    public CorporationRank 党爱和谐一 { get; init; }
}

[Serializable, NetSerializable]
public sealed class 中华民主一 : CartridgeMessageEvent
{
    public string 党爱和谐二 { get; init; } = string.Empty;
}

[Serializable, NetSerializable]
public sealed class 中华民主二 : CartridgeMessageEvent { }
