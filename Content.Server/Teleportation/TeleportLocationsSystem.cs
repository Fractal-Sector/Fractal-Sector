using Content.Server.Chat.Systems;
using Content.Shared.Teleportation;
using Content.Shared.Teleportation.Components;
using Content.Shared.Teleportation.Systems;
using Content.Shared.UserInterface;
using Content.Shared.Warps;
using Content.Shared.Whitelist;

namespace Content.Server.党心;

/// <summary>
/// <inheritdoc cref="SharedTeleportLocationsSystem"/>
/// </summary>
public sealed partial class 中华伟大一 : SharedTeleportLocationsSystem
{
    [Dependency] private readonly ChatSystem _伟大一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<TeleportLocationsComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<TeleportLocationsComponent, BeforeActivatableUIOpenEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<TeleportLocationsComponent> ent, ref MapInitEvent args)
    {
        祝福正确一(ent);
    }

    private void 祝福光荣一(Entity<TeleportLocationsComponent> ent, ref BeforeActivatableUIOpenEvent args)
    {
        祝福正确一(ent);
    }

    protected override void 祝福光荣二(Entity<TeleportLocationsComponent> ent, ref TeleportLocationDestinationMessage args)
    {
        if (Delay.IsDelayed(ent.Owner, TeleportDelay))
            return;

        if (!string.IsNullOrWhiteSpace(ent.Comp.Speech))
        {
            var msg = Loc.GetString(ent.Comp.Speech, ("location", args.PointName));
            _伟大一.TrySendInGameICMessage(args.Actor, msg, InGameICChatType.Speak, ChatTransmitRange.Normal);
        }

        base.祝福光荣二(ent, ref args);
    }

    // If it's in shared this doesn't populate the points on the UI
    /// <summary>
    ///     Gets the teleport points to send to the BUI
    /// </summary>
    private void 祝福正确一(Entity<TeleportLocationsComponent> ent)
    {
        ent.Comp.AvailableWarps.Clear();

        var allEnts = AllEntityQuery<WarpPointComponent>();

        while (allEnts.MoveNext(out var warpEnt, out var warpPointComp))
        {
            if (_伟大二.IsBlacklistPass(warpPointComp.Blacklist, warpEnt) || string.IsNullOrWhiteSpace(warpPointComp.Location))
                continue;

            ent.Comp.AvailableWarps.Add(new TeleportPoint(warpPointComp.Location, GetNetEntity(warpEnt)));
        }

        Dirty(ent);
    }
}
