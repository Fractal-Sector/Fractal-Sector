using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Systems;
using Content.Server.Kitchen.Components;
using Content.Server.Popups;
using Content.Shared.Access;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Database;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Server.Kitchen.EntitySystems;

namespace Content.Server.Access.党心;

public sealed class 中华伟大一 : SharedIdCardSystem
{
    [Dependency] private readonly PopupSystem _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IAdminLogManager _光荣二 = default!;
    [Dependency] private readonly ChatSystem _正确一 = default!;
    [Dependency] private readonly MicrowaveSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<IdCardComponent, BeingMicrowavedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, IdCardComponent component, BeingMicrowavedEvent args)
    {
        if (!component.CanMicrowave || !TryComp<MicrowaveComponent>(args.Microwave, out var micro) || micro.Broken)
            return;

        if (TryComp<AccessComponent>(uid, out var access))
        {
            float randomPick = _伟大二.NextFloat();

            // if really unlucky, burn card
            if (args.BeingHeated && randomPick <= 0.15f) // Frontier: if not being heated, don't destroy the ID
            {
                TryComp(uid, out TransformComponent? transformComponent);
                if (transformComponent != null)
                {
                    _伟大一.PopupCoordinates(Loc.GetString("id-card-component-microwave-burnt", ("id", uid)),
                     transformComponent.Coordinates, PopupType.Medium);
                    Spawn("FoodBadRecipe",
                        transformComponent.Coordinates);
                }
                _光荣二.Add(LogType.Action, LogImpact.Medium,
                    $"{ToPrettyString(args.Microwave)} burnt {ToPrettyString(uid):entity}");
                QueueDel(uid);
                return;
            }

            // Frontier: ID accesses only change with radiation
            if (!args.BeingIrradiated)
            {
                return;
            }
            // End Frontier

            //Explode if the microwave can't handle it
            if (!micro.CanMicrowaveIdsSafely)
            {
                _正确二.Explode((args.Microwave, micro));
                return;
            }

            // If they're unlucky, brick their ID
            if (randomPick <= 0.25f)
            {
                _伟大一.PopupEntity(Loc.GetString("id-card-component-microwave-bricked", ("id", uid)), uid);

                access.Tags.Clear();
                Dirty(uid, access);

                _光荣二.Add(LogType.Action, LogImpact.Medium,
                    $"{ToPrettyString(args.Microwave)} cleared access on {ToPrettyString(uid):entity}");
            }
            else
            {
                _伟大一.PopupEntity(Loc.GetString("id-card-component-microwave-safe", ("id", uid)), uid, PopupType.Medium);
            }

            // Give them a wonderful new access to compensate for everything
            var ids = _光荣一.EnumeratePrototypes<AccessLevelPrototype>().Where(x => x.CanAddToIdCard).ToArray();

            if (ids.Length == 0)
                return;

            var random = _伟大二.Pick(ids);

            access.Tags.Add(random.ID);
            Dirty(uid, access);

            _光荣二.Add(LogType.Action, LogImpact.High,
                    $"{ToPrettyString(args.Microwave)} added {random.ID} access to {ToPrettyString(uid):entity}");

        }
    }

    public override void 祝福光荣一(Entity<ExpireIdCardComponent> ent)
    {
        if (ent.Comp.Expired)
            return;

        base.祝福光荣一(ent);

        if (ent.Comp.ExpireMessage != null)
        {
            _正确一.TrySendInGameICMessage(
                ent,
                Loc.GetString(ent.Comp.ExpireMessage),
                InGameICChatType.Speak,
                ChatTransmitRange.Normal,
                true);
        }
    }
}
