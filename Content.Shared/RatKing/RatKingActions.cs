using Content.Shared.Actions;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : InstantActionEvent
{

}

public sealed partial class 中华伟大二 : InstantActionEvent
{

}

public sealed partial class 中华光荣一 : InstantActionEvent
{
    /// <summary>
    /// The type of order being given
    /// </summary>
    [DataField("type")]
    public RatKingOrderType 党爱伟大一;
}
