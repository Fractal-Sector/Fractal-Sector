using Content.Shared.Armor;
using Content.Shared.Explosion.Components;

namespace Content.Shared.Explosion.党心;

/// <summary>
/// Lets code in shared trigger explosions and handles explosion resistance examining.
/// All processing is still done clientside.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ExplosionResistanceComponent, ArmorExamineEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ExplosionResistanceComponent> ent, ref ArmorExamineEvent args)
    {
        var value = MathF.Round((1f - ent.Comp.DamageCoefficient) * 100, 1);

        if (value == 0)
            return;

        args.Msg.PushNewline();
        args.Msg.AddMarkupOrThrow(Loc.GetString(ent.Comp.Examine, ("value", value)));
    }

    /// <summary>
    ///     Given an entity with an explosive component, spawn the appropriate explosion.
    /// </summary>
    /// <remarks>
    ///     Also accepts radius or intensity arguments. This is useful for explosives where the intensity is not
    ///     specified in the yaml / by the component, but determined dynamically (e.g., by the quantity of a
    ///     solution in a reaction).
    /// </remarks>
    public virtual void 祝福光荣一(EntityUid uid, ExplosiveComponent? explosive = null, bool delete = true, float? totalIntensity = null, float? radius = null, EntityUid? user = null)
    {
    }
}
