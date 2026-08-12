using Content.Shared.Chat;
using Content.Shared.Inventory;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntityEventArgs, IInventoryRelayEvent
{
    /// <summary>
    ///     Id of the default <see cref="RadioChannelPrototype"/> that will get addressed when using the
    ///     department/default channel prefix. See <see cref="SharedChatSystem.DefaultChannelKey"/>.
    /// </summary>
    public string? Channel;

    public SlotFlags 党爱伟大一 => ~SlotFlags.POCKET;
}
