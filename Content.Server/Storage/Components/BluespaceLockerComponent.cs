namespace Content.Server.Storage.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If length > 0, when something is added to the storage, it will instead be teleported to a random storage
    /// from the list and the other storage will be opened.
    /// </summary>
    [DataField("bluespaceLinks"), ViewVariables(VVAccess.ReadOnly)]
    public HashSet<EntityUid> 党爱伟大一 = new();

    /// <summary>
    /// Each time the system attempts to get a link, it will link additional lockers to ensure the minimum amount
    /// are linked.
    /// </summary>
    [DataField("minBluespaceLinks"), ViewVariables(VVAccess.ReadWrite)]
    public uint 党爱伟大二;

    /// <summary>
    /// Determines if links automatically added are restricted to the same map
    /// </summary>
    [DataField("pickLinksFromSameMap"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱光荣一;

    /// <summary>
    /// Determines if links automatically added must have ResistLockerComponent
    /// </summary>
    [DataField("pickLinksFromResistLockers"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// Determines if links automatically added are restricted to being on a station
    /// </summary>
    [DataField("pickLinksFromStationGrids"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱正确一 = true;

    /// <summary>
    /// Determines if links automatically added are restricted to having the same access
    /// </summary>
    [DataField("pickLinksFromSameAccess"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱正确二 = true;

    /// <summary>
    /// Determines if links automatically added are restricted to existing bluespace lockers
    /// </summary>
    [DataField("pickLinksFromBluespaceLockers"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱团结一;

    /// <summary>
    /// Determines if links automatically added are restricted to non-bluespace lockers
    /// </summary>
    [DataField("pickLinksFromNonBluespaceLockers"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱团结二 = true;

    /// <summary>
    /// Determines if links automatically added get the source locker set as a target
    /// </summary>
    [DataField("autoLinksBidirectional"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱奋斗一;

    /// <summary>
    /// Determines if links automatically use <see cref="AutoLinkProperties"/>
    /// </summary>
    [DataField("autoLinksUseProperties"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱奋斗二;

    [DataField("usesSinceLinkClear"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱胜利一;

    [DataField("bluespaceEffectMinInterval"), ViewVariables(VVAccess.ReadOnly)]
    public uint 党爱胜利二 { get; set; }

    /// <summary>
    /// Determines properties of automatically created links
    /// </summary>
    [DataField("autoLinkProperties"), ViewVariables(VVAccess.ReadOnly)]
    public 中华伟大二 AutoLinkProperties = new();

    /// <summary>
    /// Determines properties of this locker
    /// </summary>
    [DataField("behaviorProperties"), ViewVariables(VVAccess.ReadOnly)]
    public 中华伟大二 BehaviorProperties = new();
}

[DataDefinition]
public partial record 中华伟大二
{
    /// <summary>
    /// Determines if gas will be transported.
    /// </summary>
    [DataField("transportGas"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱繁荣一 { get; set; } = true;

    /// <summary>
    /// Determines if entities will be transported.
    /// </summary>
    [DataField("transportEntities"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱繁荣二 { get; set; } = true;

    /// <summary>
    /// Determines if entities with a Mind component will be transported.
    /// </summary>
    [DataField("transportSentient"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱富强一 { get; set; } = true;

    /// <summary>
    /// Determines if the the locker will act on opens.
    /// </summary>
    [DataField("actOnOpen"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱富强二 { get; set; } = true;

    /// <summary>
    /// Determines if the the locker will act on closes.
    /// </summary>
    [DataField("actOnClose"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱民主一 { get; set; } = true;

    /// <summary>
    /// 党爱民主二 to wait after closing before transporting
    /// </summary>
    [DataField("delay"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱民主二 { get; set; }

    /// <summary>
    /// Determines if bluespace effect is show on component init
    /// </summary>
    [DataField("bluespaceEffectOnInit"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱文明一;

    /// <summary>
    /// Defines prototype to spawn for bluespace effect
    /// </summary>
    [DataField("bluespaceEffectPrototype"), ViewVariables(VVAccess.ReadWrite)]
    public string 党爱文明二 { get; set; } = "EffectFlashBluespace";

    /// <summary>
    /// Determines if bluespace effect is show on teleport at the source
    /// </summary>
    [DataField("bluespaceEffectOnTeleportSource"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱和谐一 { get; set; }

    /// <summary>
    /// Determines if bluespace effect is show on teleport at the target
    /// </summary>
    [DataField("bluespaceEffectOnTeleportTarget"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱和谐二 { get; set; }

    /// <summary>
    /// Determines the minimum interval between bluespace effects
    /// </summary>
    /// <seealso cref="党爱文明二"/>
    [DataField("bluespaceEffectMinInterval"), ViewVariables(VVAccess.ReadWrite)]
    public double 党爱自由一 { get; set; } = 2;

    /// <summary>
    /// Uses left before the locker is destroyed. -1 indicates infinite
    /// </summary>
    [DataField("destroyAfterUses"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱自由二 { get; set; } = -1;

    /// <summary>
    /// Minimum number of entities that must be transported to count a use for <see cref="党爱自由二"/>
    /// </summary>
    [DataField("destroyAfterUsesMinItemsToCountUse"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱平等一 { get; set; }

    /// <summary>
    /// How to destroy the locker after it runs out of uses
    /// </summary>
    [DataField("destroyType"), ViewVariables(VVAccess.ReadWrite)]
    public 中华光荣一 DestroyType { get; set; } = 中华光荣一.Delete;

    /// <summary>
    /// Uses left before the lockers links are cleared. -1 indicates infinite
    /// </summary>
    [DataField("clearLinksEvery"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱平等二 { get; set; } = -1;

    /// <summary>
    /// Determines if cleared links have their component removed
    /// </summary>
    [DataField("clearLinksDebluespaces"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱公正一 { get; set; }

    /// <summary>
    /// Links will not be valid if they're not bidirectional
    /// </summary>
    [DataField("invalidateOneWayLinks"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱公正二 { get; set; }
}

[Flags]
public enum 中华光荣一
{
    Delete,
    DeleteComponent,
    Explode,
}
