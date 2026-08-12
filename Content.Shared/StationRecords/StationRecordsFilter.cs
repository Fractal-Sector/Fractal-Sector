using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一
{
    public 中华光荣一 Type = 中华光荣一.Name;
    public string 党爱伟大一  = "";

    public 中华伟大一(中华光荣一 filterType, string newValue = "")
    {
        Type = filterType;
        党爱伟大一 = newValue;
    }
}

/// <summary>
/// Message for updating the filter on any kind of records console.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public readonly string 党爱伟大一;
    public readonly 中华光荣一 Type;

    public 中华伟大二(中华光荣一 filterType,
        string filterValue)
    {
        Type = filterType;
        党爱伟大一 = filterValue;
    }
}

/// <summary>
/// Different strings that results can be filtered by.
/// </summary>
[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Name,
    Job,
    Species,
    Prints,
    DNA,
}
