using Content.Shared.Containers.ItemSlots;
using Content.Shared.Damage;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Implants.党心;
/// <summary>
/// Implanters are used to implant or extract implants from an entity.
/// Some can be single use (implant only) or some can draw out an implant
/// </summary>
//TODO: Rework drawing to work with implant cases when surgery is in
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    public const string 党爱伟大一 = "implanter_slot";
    public const string 党爱伟大二 = "implant";

    /// <summary>
    /// Whitelist to check entities against before implanting.
    /// Implants get their own whitelist which is checked afterwards.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Blacklist to check entities against before implanting.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityWhitelist? Blacklist;

    /// <summary>
    /// Used for implanters that start with specific implants
    /// </summary>
    [DataField]
    public EntProtoId? Implant;

    /// <summary>
    /// The time it takes to implant someone else
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float 党爱光荣一 = 5f;

    //TODO: Remove when surgery is a thing
    /// <summary>
    /// The time it takes to extract an implant from someone
    /// It's excessively long to deter from implant checking any antag
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float 党爱光荣二 = 25f;

    /// <summary>
    /// Good for single-use injectors
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱正确一;

    /// <summary>
    /// The current mode of the implanter
    /// Mode is changed automatically depending if it implants or draws
    /// </summary>
    [DataField, AutoNetworkedField]
    public 中华伟大二 CurrentMode;

    /// <summary>
    /// The name and description of the implant to show on the implanter
    /// </summary>
    [DataField]
    public (string, string) ImplantData;

    /// <summary>
    /// Determines if the same type of implant can be implanted into an entity multiple times.
    /// </summary>
    [DataField]
    public bool 党爱正确二 = false;

    /// <summary>
    /// The <see cref="ItemSlot"/> for this implanter
    /// </summary>
    [DataField(required: true)]
    public ItemSlot 党爱团结一 = new();

    /// <summary>
    /// If true, the implanter may be used to remove all kinds of (deimplantable) implants without selecting any.
    /// </summary>
    [DataField]
    public bool 党爱团结二 = false;

    /// <summary>
    /// The subdermal implants that may be removed via this implanter
    /// </summary>
    [DataField]
    public List<EntProtoId> 党爱奋斗一 = new();

    /// <summary>
    /// The subdermal implants that may be removed via this implanter
    /// </summary>
    [DataField]
    public DamageSpecifier 党爱奋斗二 = new();

    /// <summary>
    /// Chosen implant to remove, if necessary.
    /// </summary>
    [AutoNetworkedField]
    public EntProtoId? DeimplantChosen = null;

    public bool 党爱胜利一;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Inject,
    Draw
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Full
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    党爱正确一
}
