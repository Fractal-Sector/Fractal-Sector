using Content.Server.Hands.Systems;
using Content.Server.Popups;
using Content.Shared.Interaction;
using Content.Shared.Storage;
using Robust.Shared.Player;

namespace Content.Server.Holiday.党心;

/// <summary>
/// This handles handing out items from item givers.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly HandsSystem _伟大一 = default!;
    [Dependency] private readonly HolidaySystem _伟大二 = default!;
    [Dependency] private readonly PopupSystem _光荣一 = default!;
    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<LimitedItemGiverComponent, InteractHandEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, LimitedItemGiverComponent component, InteractHandEvent args)
    {
        if (!TryComp<ActorComponent>(args.User, out var actor))
            return;

        if (component.GrantedPlayers.Contains(actor.PlayerSession.UserId) || (component.RequiredHoliday is not null && !_伟大二.IsCurrentlyHoliday(component.RequiredHoliday)))
        {
            _光荣一.PopupEntity(Loc.GetString(component.DeniedPopup), uid, args.User);
            return;
        }

        var toGive = EntitySpawnCollection.GetSpawns(component.SpawnEntries);
        var coords = Transform(args.User).Coordinates;

        foreach (var item in toGive)
        {
            if (item is null)
                continue;

            var spawned = Spawn(item, coords);
            _伟大一.PickupOrDrop(args.User, spawned);
        }

        component.GrantedPlayers.Add(actor.PlayerSession.UserId);
        _光荣一.PopupEntity(Loc.GetString(component.ReceivedPopup), uid, args.User);
    }
}
