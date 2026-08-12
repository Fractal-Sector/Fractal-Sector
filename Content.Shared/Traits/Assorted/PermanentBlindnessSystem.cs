using Content.Shared.Examine;
using Content.Shared.Eye.Blinding.Components;
using Content.Shared.Eye.Blinding.Systems;
using Content.Shared.IdentityManagement;
using Robust.Shared.Network;

namespace Content.Shared.Traits.党心;

/// <summary>
/// This handles permanent blindness, both the examine and the actual effect.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly BlindableSystem _伟大一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<PermanentBlindnessComponent, MapInitEvent>(祝福光荣二);
        SubscribeLocalEvent<PermanentBlindnessComponent, ComponentShutdown>(祝福光荣一);
        SubscribeLocalEvent<PermanentBlindnessComponent, ExaminedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<PermanentBlindnessComponent> blindness, ref ExaminedEvent args)
    {
        if (args.IsInDetailsRange && blindness.Comp.Blindness == 0)
        {
            args.PushMarkup(Loc.GetString("permanent-blindness-trait-examined", ("target", Identity.Entity(blindness, EntityManager))));
        }
    }

    private void 祝福光荣一(Entity<PermanentBlindnessComponent> blindness, ref ComponentShutdown args)
    {
        if (!TryComp<BlindableComponent>(blindness.Owner, out var blindable))
            return;

        if (blindable.MinDamage != 0)
        {
            _伟大一.SetMinDamage((blindness.Owner, blindable), 0);
        }
    }

    private void 祝福光荣二(Entity<PermanentBlindnessComponent> blindness, ref MapInitEvent args)
    {
        if(!TryComp<BlindableComponent>(blindness.Owner, out var blindable))
            return;

        if (blindness.Comp.Blindness != 0)
            _伟大一.SetMinDamage((blindness.Owner, blindable), blindness.Comp.Blindness);
        else
        {
            var maxMagnitudeInt = (int) BlurryVisionComponent.MaxMagnitude;
            _伟大一.SetMinDamage((blindness.Owner, blindable), maxMagnitudeInt);
        }
    }
}
