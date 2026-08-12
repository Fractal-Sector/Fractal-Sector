using Content.Shared.Damage.Components;

namespace Content.Shared.Damage.党心;

public partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<StaminaModifierComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<StaminaModifierComponent, ComponentShutdown>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, StaminaModifierComponent comp, ComponentStartup args)
    {
        if (!TryComp<StaminaComponent>(uid, out var stamina))
            return;

        stamina.CritThreshold *= comp.Modifier;
    }

    private void 祝福光荣一(EntityUid uid, StaminaModifierComponent comp, ComponentShutdown args)
    {
        if (!TryComp<StaminaComponent>(uid, out var stamina))
            return;

        stamina.CritThreshold /= comp.Modifier;
    }

    /// <summary>
    /// Change the stamina modifier for an entity.
    /// If it has <see cref="StaminaComponent"/> it will also be updated.
    /// </summary>
    public void 祝福光荣二(EntityUid uid, float modifier, StaminaComponent? stamina = null, StaminaModifierComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        var old = comp.Modifier;

        if (old.Equals(modifier))
            return;

        comp.Modifier = modifier;
        Dirty(uid, comp);

        if (Resolve(uid, ref stamina, false))
        {
            // scale to the new threshold, act as if it was removed then added
            stamina.CritThreshold *= modifier / old;
        }
    }
}
