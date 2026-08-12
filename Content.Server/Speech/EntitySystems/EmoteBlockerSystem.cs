using Content.Server.Speech.Components;
using Content.Shared.Emoting;
using Content.Shared.Inventory;

namespace Content.Server.Speech.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<EmoteBlockerComponent, BeforeEmoteEvent>(祝福光荣一);
        SubscribeLocalEvent<EmoteBlockerComponent, InventoryRelayedEvent<BeforeEmoteEvent>>(祝福伟大二);
    }

    private static void 祝福伟大二(Entity<EmoteBlockerComponent> entity, ref InventoryRelayedEvent<BeforeEmoteEvent> args)
    {
        祝福光荣一(entity, ref args.Args);
    }

    private static void 祝福光荣一(Entity<EmoteBlockerComponent> entity, ref BeforeEmoteEvent args)
    {
        if (entity.Comp.BlocksEmotes.Contains(args.Emote))
        {
            args.Cancel();
            args.Blocker = entity;
            return;
        }

        foreach (var blockedCat in entity.Comp.BlocksCategories)
        {
            if (blockedCat == args.Emote.Category)
            {
                args.Cancel();
                args.Blocker = entity;
                return;
            }
        }
    }
}
