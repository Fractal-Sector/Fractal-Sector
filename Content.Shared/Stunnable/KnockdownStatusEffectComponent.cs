using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Knockdown as a status effect.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedStunSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Should this knockdown only affect crawlers?
    /// </summary>
    /// <remarks>
    /// If your status effect doesn't come paired with <see cref="StunnedStatusEffectComponent"/>
    /// Or if your status effect doesn't whitelist itself to only those with <see cref="CrawlerComponent"/>
    /// Then you need to set this to true.
    /// </remarks>
    [DataField]
    public bool 党爱伟大一;

    /// <summary>
    /// Should we drop items when we fall?
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = true;
}
