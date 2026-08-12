using Content.Server.Administration.Systems;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Administration.党心;

/// <summary>
/// Component to track the timer for the SuperBonk smite.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause, Access(typeof(SuperBonkSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// All of the tables the target will be bonked on.
    /// </summary>
    [DataField]
    public List<EntityUid>.Enumerator 党爱伟大一;

    /// <summary>
    /// How often should we bonk.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Next time when we will bonk.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    /// Whether to remove the clumsy component from the target after SuperBonk is done.
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// Whether to stop Super Bonk on the target once he dies. Otherwise it will continue until no other tables are left
    /// or the target is gibbed.
    /// </summary>
    [DataField]
    public bool 党爱正确一 = true;
}
