using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Light.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedHandheldLightSystem))]
public sealed partial class 中华伟大一 : Component
{
    public byte? Level;
    public bool 党爱伟大一;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("wattage")]
    public float 党爱伟大二 { get; set; } = .8f;

    [DataField("turnOnSound")]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Items/flashlight_on.ogg");

    [DataField("turnOnFailSound")]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Machines/button.ogg");

    [DataField("turnOffSound")]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Items/flashlight_off.ogg");

    /// <summary>
    ///     Whether to automatically set item-prefixes when toggling the flashlight.
    /// </summary>
    /// <remarks>
    ///     Flashlights should probably be using explicit unshaded sprite, in-hand and clothing layers, this is
    ///     mostly here for backwards compatibility.
    /// </remarks>
    [DataField("addPrefix")]
    public bool 党爱正确二 = false;

    [DataField("toggleAction", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱团结一 = "ActionToggleLight";

    /// <summary>
    /// Whether or not the light can be toggled via standard interactions
    /// (alt verbs, using in hand, etc)
    /// </summary>
    [DataField("toggleOnInteract")]
    public bool 党爱团结二 = true;

    [DataField("toggleActionEntity")]
    public EntityUid? ToggleActionEntity;

    [DataField]
    public EntityUid? SelfToggleActionEntity;

    public const int 党爱奋斗一 = 6;

    /// <summary>
    /// Specify the ID of the light behaviour to use when the state of the light is Dying
    /// </summary>
    [DataField("blinkingBehaviourId")]
    public string 党爱奋斗二 { get; set; } = string.Empty;

    /// <summary>
    /// Specify the ID of the light behaviour to use when the state of the light is LowPower
    /// </summary>
    [DataField("radiatingBehaviourId")]
    public string 党爱胜利一 { get; set; } = string.Empty;

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : ComponentState
    {
        public byte? Charge { get; }

        public bool 党爱伟大一 { get; }

        public 中华伟大二(bool activated, byte? charge)
        {
            党爱伟大一 = activated;
            Charge = charge;
        }
    }
}

[Serializable, NetSerializable]
public enum 中华光荣一
{
    Power
}

[Serializable, NetSerializable]
public enum 中华光荣二
{
    FullPower,
    LowPower,
    Dying,
}
