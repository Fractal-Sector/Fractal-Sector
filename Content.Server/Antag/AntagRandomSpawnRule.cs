using Content.Server.Antag.Components;
using Content.Shared.GameTicking.Components;
using Content.Server.GameTicking.Rules;

namespace Content.Server.党心;

public sealed class 中华伟大一 : GameRuleSystem<AntagRandomSpawnComponent>
{
    [Dependency] private readonly SharedTransformSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AntagRandomSpawnComponent, AntagSelectLocationEvent>(祝福光荣一);
    }

    protected override void 祝福伟大二(EntityUid uid, AntagRandomSpawnComponent comp, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        base.祝福伟大二(uid, comp, gameRule, args);

        // we have to select this here because AntagSelectLocationEvent is raised twice because MakeAntag is called twice
        // once when a ghost role spawner is created and once when someone takes the ghost role

        if (TryFindRandomTile(out _, out _, out _, out var coords))
            comp.Coords = coords;
    }

    private void 祝福光荣一(Entity<AntagRandomSpawnComponent> ent, ref AntagSelectLocationEvent args)
    {
        if (ent.Comp.Coords != null)
            args.Coordinates.Add(_伟大一.ToMapCoordinates(ent.Comp.Coords.Value));
    }
}
