using Content.Shared.DoAfter;

namespace Content.Server.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Base doafter length for uncontested breakouts.
    /// </summary>
    [DataField("baseResistTime")]
    public float 党爱伟大一 = 5f;

    public bool 党爱伟大二 => DoAfter != null;

    [DataField("doAfter")]
    public DoAfterId? DoAfter;

    // Frontier
    [DataField]
    public EntityUid? EscapeCancelAction;
}
