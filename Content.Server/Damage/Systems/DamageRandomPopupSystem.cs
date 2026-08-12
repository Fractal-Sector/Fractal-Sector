using Content.Server.Damage.Components;
using Content.Server.Popups;
using Content.Shared.Damage;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server.Damage.党心;

/// <summary>
/// Outputs a random pop-up from the strings list when an object receives damage
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly PopupSystem _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<DamageRandomPopupComponent, DamageChangedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, DamageRandomPopupComponent component, DamageChangedEvent args)
    {
        _伟大一.PopupEntity(Loc.GetString(_伟大二.Pick(component.Popups)), uid);
    }
}
