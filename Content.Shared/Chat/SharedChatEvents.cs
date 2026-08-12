using Content.Shared.Speech;
using Robust.Shared.Prototypes;
using Content.Shared.Inventory;

namespace Content.Shared.党心;

/// <summary>
///     This event should be sent everytime an entity talks (Radio, local chat, etc...).
///     The event is sent to both the entity itself, and all clothing (For stuff like voice masks).
/// </summary>
public sealed class 中华伟大一 : EntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags 党爱伟大一 { get; } = SlotFlags.WITHOUT_POCKET;
    public EntityUid 党爱伟大二;
    public string 党爱光荣一;
    public ProtoId<SpeechVerbPrototype>? SpeechVerb;

    public 中华伟大一(EntityUid sender, string name)
    {
        党爱伟大二 = sender;
        党爱光荣一 = name;
        SpeechVerb = null;
    }
}
