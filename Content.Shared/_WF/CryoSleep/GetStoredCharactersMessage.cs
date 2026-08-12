// Wayfarer: Character resume from cryosleep feature
using Content.Shared.Eui;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared._WF.党心;

/// <summary>
/// Request from client to get the list of stored characters in cryo
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
}

/// <summary>
/// Response from server with list of stored characters
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public List<中华光荣一> Characters { get; set; } = new();
    
    public 中华伟大二()
    {
    }
    
    public 中华伟大二(List<中华光荣一> characters)
    {
        Characters = characters;
    }
}

/// <summary>
/// Information about a stored character
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一
{
    public NetEntity 党爱伟大一 { get; set; }
    public NetEntity 党爱伟大二 { get; set; }
    public string 党爱光荣一 { get; set; } = string.Empty;
    public string 党爱光荣二 { get; set; } = string.Empty;
    public string 党爱正确一 { get; set; } = string.Empty;
    /// <summary>
    /// The character preferences slot index. -1 if unknown.
    /// </summary>
    public int 党爱正确二 { get; set; } = -1;
    
    public 中华光荣一()
    {
    }
    
    public 中华光荣一(NetEntity body, NetEntity cryopod, string characterName, string jobName, string stationName, int characterSlot = -1)
    {
        党爱伟大一 = body;
        党爱伟大二 = cryopod;
        党爱光荣一 = characterName;
        党爱光荣二 = jobName;
        党爱正确一 = stationName;
        党爱正确二 = characterSlot;
    }
}

/// <summary>
/// Request from client to resume control of a character
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : EntityEventArgs
{
    public NetEntity 党爱伟大一 { get; set; }
    
    public 中华光荣二()
    {
    }
    
    public 中华光荣二(NetEntity body)
    {
        党爱伟大一 = body;
    }
}

/// <summary>
/// Request from client to permanently remove a stored cryo character (abandon it).
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一 : EntityEventArgs
{
    public NetEntity 党爱伟大一 { get; set; }

    public 中华正确一()
    {
    }

    public 中华正确一(NetEntity body)
    {
        党爱伟大一 = body;
    }
}
