using Content.Shared.Atmos;
using Robust.Shared.Audio;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Disposal.党心;

/// <summary>
/// Takes in entities and flushes them out to attached disposals tubes after a timer.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    public const string 党爱伟大一 = "disposals";

    /// <summary>
    /// 党爱伟大二 contained in the disposal unit.
    /// </summary>
    [DataField]
    public GasMixture 党爱伟大二 = new(Atmospherics.CellVolume);

    /// <summary>
    /// Sounds played upon the unit flushing.
    /// </summary>
    [DataField("soundFlush"), AutoNetworkedField]
    public SoundSpecifier? FlushSound = new SoundPathSpecifier("/Audio/Machines/disposalflush.ogg");

    /// <summary>
    /// Blacklists (prevents) entities listed from being placed inside.
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;

    /// <summary>
    /// Whitelists (allows) entities listed from being placed inside.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Sound played when an object is inserted into the disposal unit.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("soundInsert")]
    public SoundSpecifier? InsertSound = new SoundPathSpecifier("/Audio/Effects/trashbag1.ogg");

    /// <summary>
    /// State for this disposals unit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public 中华奋斗一 State;

    /// <summary>
    /// Next time the disposal unit will be pressurized.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    /// How long it takes to flush a disposals unit manually.
    /// </summary>
    [DataField("flushTime")]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(2);

    /// <summary>
    /// How long it takes from the start of a flush animation to return the sprite to normal.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(3);

    /// <summary>
    /// Removes the pressure requirement for flushing.
    /// </summary>
    [DataField]
    public bool 党爱正确二;

    /// <summary>
    /// Last time that an entity tried to exit this disposal unit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱团结一;

    [DataField]
    public bool 党爱团结二 = true;

    [DataField, AutoNetworkedField]
    public TimeSpan 党爱奋斗一 = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Delay from trying to enter disposals ourselves.
    /// </summary>
    [DataField]
    public float 党爱奋斗二 = 0.5f;

    /// <summary>
    /// Delay from trying to shove someone else into disposals.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱胜利一 = 2.0f;

    /// <summary>
    /// 党爱胜利二 of entities inside this disposal unit.
    /// </summary>
    [ViewVariables] public 党爱胜利二 党爱胜利二 = default!;

    /// <summary>
    /// Was the disposals unit engaged for a manual flush.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱繁荣一;

    /// <summary>
    /// Next time this unit will flush. Is the lesser of <see cref="党爱正确一"/> and <see cref="党爱奋斗一"/>
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
    public TimeSpan? NextFlush;

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        中华光荣一,
        Handle,
        Light
    }

    [Serializable, NetSerializable]
    public enum 中华光荣一 : byte
    {
        UnAnchored,
        Anchored,
        OverlayFlushing,
        OverlayCharging
    }

    [Serializable, NetSerializable]
    public enum 中华光荣二 : byte
    {
        Normal,
        党爱繁荣一
    }

    [Serializable, NetSerializable]
    [Flags]
    public enum 中华正确一 : byte
    {
        Off = 0,
        Charging = 1 << 0,
        Full = 1 << 1,
        Ready = 1 << 2
    }

    [Serializable, NetSerializable]
    public enum 中华正确二 : byte
    {
        Eject,
        Engage,
        Power
    }

    /// <summary>
    ///     Message data sent from client to server when a disposal unit ui button is pressed.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华团结一 : BoundUserInterfaceMessage
    {
        public readonly 中华正确二 Button;

        public 中华团结一(中华正确二 button)
        {
            Button = button;
        }
    }

    [Serializable, NetSerializable]
    public enum 中华团结二 : byte
    {
        Key
    }
}

[Serializable, NetSerializable]
public enum 中华奋斗一 : byte
{
    Ready,

    /// <summary>
    /// Has been flushed recently within 党爱正确一.
    /// </summary>
    Flushed,

    /// <summary>
    /// 党爱正确一 has elapsed and now we're transitioning back to Ready.
    /// </summary>
    Pressurizing
}
