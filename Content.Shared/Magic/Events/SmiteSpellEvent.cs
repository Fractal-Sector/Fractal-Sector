using Content.Shared.Actions;

namespace Content.Shared.Magic.党心;

public sealed partial class 中华伟大一 : EntityTargetActionEvent
{
    // TODO: Make part of gib method
    /// <summary>
    /// Should this smite delete all parts/mechanisms gibbed except for the brain?
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = true;
}
