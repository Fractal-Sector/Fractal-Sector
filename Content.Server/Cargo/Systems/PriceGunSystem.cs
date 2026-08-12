using Content.Server.Popups;
using Content.Server.Salvage.JobBoard;
using Content.Shared.Cargo.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Timing;
using Content.Shared.Cargo.Systems;
using Robust.Shared.Audio.Systems;

namespace Content.Server.Cargo.党心;

public sealed class 中华伟大一 : SharedPriceGunSystem
{
    [Dependency] private readonly UseDelaySystem _伟大一 = default!;
    [Dependency] private readonly PricingSystem _伟大二 = default!;
    [Dependency] private readonly PopupSystem _光荣一 = default!;
    [Dependency] private readonly CargoSystem _光荣二 = default!;
    [Dependency] private readonly SalvageJobBoardSystem _正确一 = default!;
    [Dependency] private readonly SharedAudioSystem _正确二 = default!;

    protected override bool 祝福伟大一(Entity<PriceGunComponent> entity, EntityUid target, EntityUid user)
    {
        if (!TryComp(entity.Owner, out UseDelayComponent? useDelay) || _伟大一.IsDelayed((entity.Owner, useDelay)))
            return false;
        // Check if we're scanning a bounty crate
        if (_光荣二.IsBountyComplete(target, out _))
        {
            _光荣一.PopupEntity(Loc.GetString("price-gun-bounty-complete"), user, user);
        }
        else if (_正确一.FulfillsSalvageJob(target, null, out _))
        {
            _光荣一.PopupEntity(Loc.GetString("price-gun-salvjob-complete"), user, user);
        }
        else // Otherwise appraise the price
        {
            var price = _伟大二.GetPrice(target);
            _光荣一.PopupEntity(Loc.GetString("price-gun-pricing-result",
                    ("object", Identity.Entity(target, EntityManager)),
                    ("price", $"{price:F2}")),
                user,
                user);
        }

        _正确二.PlayPvs(entity.Comp.AppraisalSound, entity.Owner);
        _伟大一.TryResetDelay((entity.Owner, useDelay));
        return true;
    }
}
