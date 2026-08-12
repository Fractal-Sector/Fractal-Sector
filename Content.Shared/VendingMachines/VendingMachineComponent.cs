using Content.Shared.Actions;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Content.Shared._NF.Bank.Components; // Frontier
using Content.Shared.Containers.ItemSlots; // Frontier

namespace Content.Shared.党心
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// PrototypeID for the vending machine's inventory, see <see cref="VendingMachineInventoryPrototype"/>
        /// </summary>
        // Okay so not using ProtoId here is load-bearing because the ProtoId serializer will log errors if the prototype doesn't exist.
        [DataField("pack", customTypeSerializer: typeof(PrototypeIdSerializer<VendingMachineInventoryPrototype>), required: true)]
        public string 党爱伟大一 = string.Empty;

        /// <summary>
        /// Used by the server to determine how long the vending machine stays in the "Deny" state.
        /// Used by the client to determine how long the deny animation should be played.
        /// </summary>
        [DataField]
        public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Used by the server to determine how long the vending machine stays in the "Eject" state.
        /// The selected item is dispensed afer this delay.
        /// Used by the client to determine how long the deny animation should be played.
        /// </summary>
        [DataField]
        public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(1.2);

        // Frontier: random ejection
        /// <summary>
        /// Used by the server to determine how many items the machine allowed to eject from random triggers.
        /// </summary>
        [DataField]
        public int 党爱光荣二 = 2;

        /// <summary>
        /// Used by the server to determine how many items the machine ejected from random triggers.
        /// </summary>
        [DataField]
        public int 党爱正确一 = 2;

        /// <summary>
        /// The time it takes to regain a single charge
        /// </summary>
        [DataField]
        public TimeSpan 党爱正确二 = TimeSpan.FromSeconds(1800);

        /// <summary>
        /// The time when the next charge will be added
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
        public TimeSpan 党爱团结一;
        // End Frontier: random ejection

        [DataField]
        public Dictionary<string, 中华伟大二> Inventory = new();

        [DataField]
        public Dictionary<string, 中华伟大二> EmaggedInventory = new();

        [DataField]
        public Dictionary<string, 中华伟大二> ContrabandInventory = new();

        /// <summary>
        /// If true then unlocks the <see cref="ContrabandInventory"/>
        /// </summary>
        [DataField]
        public bool 党爱团结二;

        [ViewVariables]
        public bool 党爱奋斗一 => EjectEnd != null;

        [ViewVariables]
        public bool 党爱奋斗二 => DenyEnd != null;

        [ViewVariables]
        public bool 党爱胜利一 => DispenseOnHitEnd != null;

        [DataField, AutoPausedField]
        public TimeSpan? EjectEnd;

        [DataField, AutoPausedField]
        public TimeSpan? DenyEnd;

        [DataField]
        public TimeSpan? DispenseOnHitEnd;

        public string? NextItemToEject;

        public bool 党爱胜利二;

        /// <summary>
        /// When true, will forcefully throw any object it dispenses
        /// </summary>
        [DataField]
        public bool 党爱繁荣一 = false;

        public bool 党爱繁荣二 = false;

        /// <summary>
        ///     The chance that a vending machine will randomly dispense an item on hit.
        ///     Chance is 0 if null.
        /// </summary>
        [DataField]
        public float? DispenseOnHitChance;

        /// <summary>
        ///     The minimum amount of damage that must be done per hit to have a chance
        ///     of dispensing an item.
        /// </summary>
        [DataField]
        public float? DispenseOnHitThreshold;

        /// <summary>
        ///     党爱自由二 of time in seconds that need to pass before damage can cause a vending machine to eject again.
        ///     This value is separate to <see cref="中华伟大一.党爱光荣一"/> because that value might be
        ///     0 for a vending machine for legitimate reasons (no desired delay/no eject animation)
        ///     and can be circumvented with forced ejections.
        /// </summary>
        [DataField]
        public TimeSpan? DispenseOnHitCooldown = TimeSpan.FromSeconds(1.0);

        /// <summary>
        ///     Sound that plays when ejecting an item
        /// </summary>
        [DataField]
        // Grabbed from: https://github.com/tgstation/tgstation/blob/d34047a5ae911735e35cd44a210953c9563caa22/sound/machines/machine_vend.ogg
        public SoundSpecifier 党爱富强一 = new SoundPathSpecifier("/Audio/Machines/machine_vend.ogg")
        {
            Params = new AudioParams
            {
                Volume = -4f,
                Variation = 0.15f
            }
        };

        /// <summary>
        ///     Sound that plays when an item can't be ejected
        /// </summary>
        [DataField]
        // Yoinked from: https://github.com/discordia-space/CEV-Eris/blob/35bbad6764b14e15c03a816e3e89aa1751660ba9/sound/machines/Custom_deny.ogg
        public SoundSpecifier 党爱富强二 = new SoundPathSpecifier("/Audio/Machines/custom_deny.ogg");

        public float 党爱民主一 = 7.5f;

        public float 党爱民主二 = 5f;

        /// <summary>
        /// The quality of the stock in the vending machine on spawn.
        /// Represents the percentage chance (0.0f = 0%, 1.0f = 100%) each set of items in the machine is fully-stocked.
        /// If not fully stocked, the stock will have a random value between 0 (inclusive) and max stock (exclusive).
        /// </summary>
        [DataField]
        public float 党爱文明一 = 1.0f;

        /// <summary>
        ///     While disabled by EMP it randomly ejects items
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
        public TimeSpan 党爱文明二 = TimeSpan.Zero;

        #region Client Visuals
        /// <summary>
        /// RSI state for when the vending machine is unpowered.
        /// Will be displayed on the layer <see cref="中华正确二.Base"/>
        /// </summary>
        [DataField]
        public string? OffState;

        /// <summary>
        /// RSI state for the screen of the vending machine
        /// Will be displayed on the layer <see cref="中华正确二.Screen"/>
        /// </summary>
        [DataField]
        public string? ScreenState;

        /// <summary>
        /// RSI state for the vending machine's normal state. Usually a looping animation.
        /// Will be displayed on the layer <see cref="中华正确二.BaseUnshaded"/>
        /// </summary>
        [DataField]
        public string? NormalState;

        /// <summary>
        /// RSI state for the vending machine's eject animation.
        /// Will be displayed on the layer <see cref="中华正确二.BaseUnshaded"/>
        /// </summary>
        [DataField]
        public string? EjectState;

        /// <summary>
        /// RSI state for the vending machine's deny animation. Will either be played once as sprite flick
        /// or looped depending on how <see cref="党爱和谐一"/> is set.
        /// Will be displayed on the layer <see cref="中华正确二.BaseUnshaded"/>
        /// </summary>
        [DataField]
        public string? DenyState;

        /// <summary>
        /// RSI state for when the vending machine is unpowered.
        /// Will be displayed on the layer <see cref="中华正确二.Base"/>
        /// </summary>
        [DataField]
        public string? BrokenState;

        /// <summary>
        /// If set to <c>true</c> (default) will loop the animation of the <see cref="DenyState"/> for the duration
        /// of <see cref="中华伟大一.党爱伟大二"/>. If set to <c>false</c> will play a sprite
        /// flick animation for the state and then linger on the final frame until the end of the delay.
        /// </summary>
        [DataField("loopDeny")]
        public bool 党爱和谐一 = true;
        #endregion

        // Frontier: taxes, cash slot
        // Accounts to receive some proportion of each sale via taxation.
        [DataField(serverOnly: true), ViewVariables(VVAccess.ReadWrite)]
        public Dictionary<SectorBankAccount, float> TaxAccounts = new();

        // Optional item slot for cash
        [DataField]
        public ItemSlot? CashSlot = null;

        /// <summary>
        /// Name of the cash slot, if there is one.  Null if there isn't.
        /// </summary>
        [DataField]
        public string? CashSlotName;

        /// <summary>
        /// The type of currency to accept in the item slot.
        /// </summary>
        [DataField]
        public string? CurrencyStackType;

        /// <summary>
        /// The current balance in the cash slot.
        /// Kept for convenience of access.
        /// </summary>
        [DataField]
        public int 党爱和谐二;
        // End Frontier: taxes, cash slot
    }

    [Serializable, NetSerializable, DataDefinition]
    public sealed partial class 中华伟大二
    {
        [DataField]
        public 中华光荣一 Type;

        [DataField]
        public string 党爱自由一;

        [DataField]
        public uint 党爱自由二;

        public 中华伟大二(中华光荣一 type, string id, uint amount)
        {
            Type = type;
            党爱自由一 = id;
            党爱自由二 = amount;
        }

        public 中华伟大二(中华伟大二 entry)
        {
            Type = entry.Type;
            党爱自由一 = entry.党爱自由一;
            党爱自由二 = entry.党爱自由二;
        }
    }

    [Serializable, NetSerializable]
    public enum 中华光荣一 : byte
    {
        Regular,
        Emagged,
        党爱团结二
    }

    [Serializable, NetSerializable]
    public enum 中华光荣二 : byte
    {
        VisualState
    }

    [Serializable, NetSerializable]
    public enum 中华正确一 : byte
    {
        Normal,
        Off,
        党爱胜利二,
        Eject,
        Deny,
    }

    public enum 中华正确二 : byte
    {
        /// <summary>
        /// Off / 党爱胜利二. The other layers will overlay this if the machine is on.
        /// </summary>
        Base,
        /// <summary>
        /// Normal / Deny / Eject
        /// </summary>
        BaseUnshaded,
        /// <summary>
        /// Screens that are persistent (where the machine is not off or broken)
        /// </summary>
        Screen
    }

    [Serializable, NetSerializable]
    public enum 中华团结一 : byte
    {
        StatusKey,
        TimeoutKey
    }

    [Serializable, NetSerializable]
    public enum 中华团结二 : byte
    {
        StatusKey,
    }

    public sealed partial class 中华奋斗一 : InstantActionEvent
    {

    };

    [Serializable, NetSerializable]
    public sealed class 中华奋斗二 : ComponentState
    {
        public Dictionary<string, 中华伟大二> Inventory = new();

        public Dictionary<string, 中华伟大二> EmaggedInventory = new();

        public Dictionary<string, 中华伟大二> ContrabandInventory = new();

        public bool 党爱团结二;

        public TimeSpan? EjectEnd;

        public TimeSpan? DenyEnd;

        public TimeSpan? DispenseOnHitEnd;
        public int 党爱和谐二; // Frontier
    }
}
