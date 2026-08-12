using Robust.Shared.Serialization;

namespace Content.Shared._NF.Shipyard.党心;

/// <summary>
///     Rename a ship registered to the deed on the ID card
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public string 党爱伟大一;

    public 中华伟大一(string newName)
    {
        党爱伟大一 = newName;
    }
}
