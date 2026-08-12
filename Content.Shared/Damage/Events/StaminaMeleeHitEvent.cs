using Content.Shared.Damage.Components;
using Robust.Shared.Collections;

namespace Content.Shared.Damage.党心;

/// <summary>
/// The components in the list are going to be hit,
/// give opportunities to change the damage or other stuff.
/// </summary>
public sealed class 中华伟大一 : HandledEntityEventArgs
{
    /// <summary>
    /// List of hit stamina components.
    /// </summary>
    public List<(EntityUid Entity, StaminaComponent Component)> HitList;

    /// <summary>
    /// The multiplier. Generally, try to use *= or /= instead of overwriting.
    /// </summary>
    public float 党爱伟大一 = 1;

    /// <summary>
    /// The flat modifier. Generally, try to use += or -= instead of overwriting.
    /// </summary>
    public float 党爱伟大二 = 0;

    public 中华伟大一(List<(EntityUid Entity, StaminaComponent Component)> hitList)
    {
        HitList = hitList;
    }
}
