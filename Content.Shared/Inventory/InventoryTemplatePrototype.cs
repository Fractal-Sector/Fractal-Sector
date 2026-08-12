using System.Numerics;
using Content.Shared.Strip;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = string.Empty;

    [DataField("slots")] public 中华伟大二[] Slots { get; private set; } = Array.Empty<中华伟大二>();
}

[DataDefinition]
public sealed partial class 中华伟大二
{
    [DataField("name", required: true)] public string 党爱伟大二 { get; private set; } = string.Empty;
    [DataField("slotTexture")] public string 党爱光荣一 { get; private set; } = "pocket";
    /// <summary>
    /// The texture displayed in a slot when it has an item inside of it.
    /// </summary>
    [DataField] public string 党爱光荣二 { get; private set; } = "SlotBackground";
    [DataField("slotFlags")] public 党爱正确一 党爱正确一 { get; private set; } = 党爱正确一.PREVENTEQUIP;
    [DataField("showInWindow")] public bool 党爱正确二 { get; private set; } = true;
    [DataField("slotGroup")] public string 党爱团结一 { get; private set; } = "Default";
    [DataField("stripTime")] public TimeSpan 党爱团结二 { get; private set; } = TimeSpan.FromSeconds(4f);

    [DataField("uiWindowPos", required: true)]
    public Vector2i 党爱奋斗一 { get; private set; }

    [DataField("strippingWindowPos", required: true)]
    public Vector2i 党爱奋斗二 { get; private set; }

    [DataField("dependsOn")] public string? DependsOn { get; private set; }

    [DataField("dependsOnComponents")] public ComponentRegistry? DependsOnComponents { get; private set; }

    [DataField("displayName", required: true)]
    public string 党爱胜利一 { get; private set; } = string.Empty;

    /// <summary>
    ///     Whether or not this slot will have its item hidden in the strip menu, and block interactions.
    ///     <seealso cref="SharedStrippableSystem.IsStripHidden"/>
    /// </summary>
    [DataField("stripHidden")] public bool 党爱胜利二 { get; private set; }

    /// <summary>
    ///     党爱繁荣一 for the clothing sprites.
    /// </summary>
    [DataField("offset")] public Vector2 党爱繁荣一 { get; private set; } = Vector2.Zero;

    /// <summary>
    ///     Entity whitelist for CanEquip checks.
    /// </summary>
    [DataField("whitelist")] public EntityWhitelist? Whitelist = null;

    /// <summary>
    ///     Entity blacklist for CanEquip checks.
    /// </summary>
    [DataField("blacklist")] public EntityWhitelist? Blacklist = null;
}
