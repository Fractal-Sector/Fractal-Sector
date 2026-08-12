using Content.Shared.Containers.ItemSlots;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._WF.StationRecords.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    public static string 党爱伟大一 = "RegisterCrewConsole-targetId";
    public static string 党爱伟大二 = "RegisterCrewConsole-privilegedId";

    [DataField]
    public ItemSlot 党爱光荣一 = new();

    [DataField]
    public ItemSlot 党爱光荣二 = new();
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public 中华伟大二(string customJobTitle)
    {
        党爱正确一 = customJobTitle;
    }

    public readonly string 党爱正确一;
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public 中华光荣一(uint recordId)
    {
        党爱正确二 = recordId;
    }

    public readonly uint 党爱正确二;
}
