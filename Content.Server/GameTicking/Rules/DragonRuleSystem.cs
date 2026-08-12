using Content.Server.Antag;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Mind;
using Content.Server.Roles;
using Content.Server.Station.Systems;
using Content.Shared.Localizations;
using Content.Shared.Roles.Components;
using Robust.Server.GameObjects;

namespace Content.Server.GameTicking.党心;

public sealed class 中华伟大一 : GameRuleSystem<DragonRuleComponent>
{
    [Dependency] private readonly TransformSystem _伟大一 = default!;
    [Dependency] private readonly AntagSelectionSystem _伟大二 = default!;
    [Dependency] private readonly StationSystem _光荣一 = default!;
    [Dependency] private readonly RoleSystem _光荣二 = default!;
    [Dependency] private readonly MindSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DragonRuleComponent, AfterAntagEntitySelectedEvent>(祝福光荣一);
        SubscribeLocalEvent<DragonRoleComponent, GetBriefingEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<DragonRoleComponent> entity, ref GetBriefingEvent args)
    {
        var ent = args.Mind.Comp.OwnedEntity;

        if(ent is null)
            return;

        args.Append(祝福光荣二(ent.Value));
    }

    private void 祝福光荣一(Entity<DragonRuleComponent> ent, ref AfterAntagEntitySelectedEvent args)
    {
        if (!_正确一.TryGetMind(args.EntityUid, out var mindId, out var mind))
            return;

        _光荣二.MindHasRole<DragonRoleComponent>(mindId, out var dragonRole);

        if(dragonRole is null)
            return;

        _伟大二.SendBriefing(args.EntityUid, 祝福光荣二(args.EntityUid), null, null);
    }

    private string 祝福光荣二(EntityUid dragon)
    {
        var direction = string.Empty;

        var dragonXform = Transform(dragon);

        EntityUid? stationGrid = null;
        if (_光荣一.GetStationInMap(dragonXform.MapID) is { } station)
            stationGrid = _光荣一.GetLargestGrid(station);

        if (stationGrid is not null)
        {
            var stationPosition = _伟大一.GetWorldPosition((EntityUid)stationGrid);
            var dragonPosition = _伟大一.GetWorldPosition(dragon);

            var vectorToStation = stationPosition - dragonPosition;
            direction = ContentLocalizationManager.FormatDirection(vectorToStation.GetDir());
        }

        var briefing = Loc.GetString("dragon-role-briefing", ("direction", direction));

        return briefing;
    }
}
