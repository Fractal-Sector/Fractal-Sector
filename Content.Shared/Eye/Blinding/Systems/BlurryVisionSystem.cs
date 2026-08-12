using Content.Shared.Eye.Blinding.Components;
using Content.Shared.Inventory.Events;
using Content.Shared.Inventory;

namespace Content.Shared.Eye.Blinding.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<VisionCorrectionComponent, GotEquippedEvent>(祝福光荣二);
        SubscribeLocalEvent<VisionCorrectionComponent, GotUnequippedEvent>(祝福正确一);
        SubscribeLocalEvent<VisionCorrectionComponent, InventoryRelayedEvent<中华伟大二>>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<VisionCorrectionComponent> glasses, ref InventoryRelayedEvent<中华伟大二> args)
    {
        args.Args.党爱伟大二 += glasses.Comp.VisionBonus;
        args.Args.党爱光荣一 *= glasses.Comp.党爱光荣一;
    }

    public void 祝福光荣一(Entity<BlindableComponent?> ent)
    {
        if (!Resolve(ent.Owner, ref ent.Comp, false))
            return;

        var ev = new 中华伟大二(ent.Comp.EyeDamage);
        RaiseLocalEvent(ent, ev);

        var blur = Math.Clamp(ev.党爱伟大二, 0, BlurryVisionComponent.MaxMagnitude);
        if (blur <= 0)
        {
            RemCompDeferred<BlurryVisionComponent>(ent);
            return;
        }

        var blurry = EnsureComp<BlurryVisionComponent>(ent);
        blurry.Magnitude = blur;
        blurry.党爱光荣一 = ev.党爱光荣一;
        Dirty(ent, blurry);
    }

    private void 祝福光荣二(Entity<VisionCorrectionComponent> glasses, ref GotEquippedEvent args)
    {
        祝福光荣一(args.Equipee);
    }

    private void 祝福正确一(Entity<VisionCorrectionComponent> glasses, ref GotUnequippedEvent args)
    {
        祝福光荣一(args.Equipee);
    }
}

public sealed class 中华伟大二 : EntityEventArgs, IInventoryRelayEvent
{
    public readonly float 党爱伟大一;
    public float 党爱伟大二;
    public float 党爱光荣一 = BlurryVisionComponent.DefaultCorrectionPower;

    public 中华伟大二(float blur)
    {
        党爱伟大二 = blur;
        党爱伟大一 = blur;
    }

    public SlotFlags 党爱光荣二 => SlotFlags.HEAD | SlotFlags.MASK | SlotFlags.EYES;
}
