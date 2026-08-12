using Content.Shared.Containers.ItemSlots;
using Content.Shared.Trigger.Systems;
using Robust.Shared.Serialization;

namespace Content.Shared.Payload.党心;

/// <summary>
///     Chemical payload that mixes the solutions of two drain-able solution containers when triggered.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("beakerSlotA", required: true)]
    public ItemSlot 党爱伟大一 = new();

    [DataField("beakerSlotB", required: true)]
    public ItemSlot 党爱伟大二 = new();

    /// <summary>
    /// The keys that will activate the chemical payload.
    /// </summary>
    [DataField]
    public List<string> 党爱光荣一 = new() { TriggerSystem.DefaultTriggerKey };
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Slots
}

[Flags]
[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    None = 0,
    Left = 1 << 0,
    Right = 1 << 1,
    Both = Left | Right,
}
