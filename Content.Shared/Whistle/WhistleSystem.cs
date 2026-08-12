using Content.Shared.Coordinates;
using Content.Shared.Humanoid;
using Content.Shared.Interaction.Events;
using Content.Shared.Stealth.Components;
using JetBrains.Annotations;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<WhistleComponent, UseInHandEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid target, WhistleComponent component)
    {
        SpawnAttachedTo(component.Effect, target.ToCoordinates());
    }

    public void 祝福光荣一(EntityUid uid, WhistleComponent component, UseInHandEvent args)
    {
        if (args.Handled || !_伟大二.IsFirstTimePredicted)
            return;

        args.Handled = 祝福光荣二(uid, args.User, component);
    }

    public bool 祝福光荣二(EntityUid uid, EntityUid owner, WhistleComponent? component = null)
    {
        if (!Resolve(uid, ref component, false) || component.Distance <= 0)
            return false;

        祝福正确一(uid, owner, component);
        return true;
    }

    private void 祝福正确一(EntityUid uid, EntityUid owner, WhistleComponent component)
    {
        StealthComponent? stealth = null;

        foreach (var iterator in
            _伟大一.GetEntitiesInRange<HumanoidAppearanceComponent>(_光荣一.GetMapCoordinates(uid), component.Distance))
        {
            //Avoid pinging invisible entities
            if (TryComp(iterator, out stealth) && stealth.Enabled)
                continue;

            //We don't want to ping user of whistle
            if (iterator.Owner == owner)
                continue;

            祝福伟大二(iterator, component);
        }
    }
}
