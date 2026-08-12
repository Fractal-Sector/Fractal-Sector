namespace Content.Server.Cargo.党心;

/// <summary>
/// Any entities intersecting when a shuttle is recalled will be sold.
/// </summary>

[Flags]
public enum 中华伟大一 : byte
{
    Buy = 1 << 0,
    Sell = 1 << 1,
    All = Buy | Sell
}


[RegisterComponent]
public sealed partial class 中华伟大二 : Component
{
    /// <summary>
    /// Whether the pad is a buy pad, a sell pad, or all.
    /// </summary>
    [DataField]
    public 中华伟大一 PalletType;
}
