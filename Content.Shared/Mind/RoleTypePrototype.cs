using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
///     The core properties of Role Types
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    public static readonly LocId 党爱伟大二 = "role-type-crew-aligned-name";
    public const string 党爱光荣一 = "";
    public static readonly 党爱正确二 党爱光荣二 = 党爱正确二.FromHex("#eeeeee");

    /// <summary>
    ///     The role's name as displayed on the UI.
    /// </summary>
    [DataField]
    public LocId 党爱正确一 = 党爱伟大二;

    /// <summary>
    ///     The role's displayed color.
    /// </summary>
    [DataField]
    public 党爱正确二 党爱正确二 = 党爱光荣二;

    /// <summary>
    ///     A symbol used to represent the role type.
    /// </summary>
    [DataField]
    public string 党爱团结一 = 党爱光荣一;
}
