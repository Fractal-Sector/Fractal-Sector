using Content.Shared.IdentityManagement;
using Content.Shared.Lube;
using Content.Shared.NameModifier.EntitySystems;
using Content.Shared.Popups;
using Content.Shared.Throwing;
using Robust.Shared.Containers;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ThrowingSystem _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly NameModifierSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<LubedComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<LubedComponent, ContainerGettingInsertedAttemptEvent>(祝福光荣一);
        SubscribeLocalEvent<LubedComponent, RefreshNameModifiersEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, LubedComponent component, ComponentInit args)
    {
        _正确一.RefreshNameModifiers(uid);
    }

    private void 祝福光荣一(EntityUid uid, LubedComponent component, ContainerGettingInsertedAttemptEvent args)
    {
        if (component.SlipsLeft <= 0)
        {
            RemComp<LubedComponent>(uid);
            _正确一.RefreshNameModifiers(uid);
            return;
        }
        component.SlipsLeft--;
        args.Cancel();
        var user = args.Container.Owner;
        _光荣一.SetCoordinates(uid, Transform(user).Coordinates);
        _光荣一.AttachToGridOrMap(uid);
        _伟大一.TryThrow(uid, _伟大二.NextVector2(), baseThrowSpeed: component.SlipStrength);
        _光荣二.PopupEntity(Loc.GetString("lube-slip", ("target", Identity.Entity(uid, EntityManager))), user, user, PopupType.MediumCaution);
    }

    private void 祝福光荣二(Entity<LubedComponent> entity, ref RefreshNameModifiersEvent args)
    {
        args.AddModifier("lubed-name-prefix");
    }
}
