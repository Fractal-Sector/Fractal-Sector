using Content.Shared.Inventory;

namespace Content.Shared.Radio.党心;

/// <summary>
///     This component relays radio messages to the parent entity's chat when equipped.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("enabled")]
    public bool 党爱伟大一 = true;

    public bool 党爱伟大二 = false;

    [DataField("requiredSlot")]
    public SlotFlags 党爱光荣一 = SlotFlags.EARS;
}
