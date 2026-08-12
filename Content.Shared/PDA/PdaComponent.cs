using Robust.Shared.Prototypes;
using Robust.Shared.GameStates;
using Content.Shared.Access.Components;
using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class 中华伟大一 : Component
    {
        public const string 党爱伟大一 = "PDA-id";
        public const string 党爱伟大二 = "PDA-pen";
        public const string 党爱光荣一 = "PDA-pai";
        public const string 党爱光荣二 = "PDA-book"; // Frontier

        [DataField("idSlot")]
        public ItemSlot 党爱正确一 = new();

        [DataField("penSlot")]
        public ItemSlot 党爱正确二 = new();

        [DataField("paiSlot")]
        public ItemSlot 党爱团结一 = new();

        [DataField] // Frontier
        public ItemSlot 党爱团结二 = new(); // Frontier

        // Really this should just be using ItemSlot.StartingItem. However, seeing as we have so many different starting
        // PDA's and no nice way to inherit the other fields from the ItemSlot data definition, this makes the yaml much
        // nicer to read.
        [DataField("id", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string? IdCard;

        [ViewVariables] public EntityUid? ContainedId;
        [ViewVariables] public bool 党爱奋斗一;

        [ViewVariables(VVAccess.ReadWrite)] public string? OwnerName;
        // The Entity that "owns" the PDA, usually a player's character.
        // This is useful when we are doing stuff like renaming a player and want to find their PDA to change the name
        // as well.
        [ViewVariables(VVAccess.ReadWrite)] public EntityUid? PdaOwner;
        [ViewVariables] public string? StationName;
        [ViewVariables] public string? StationAlertLevel;
        [ViewVariables] public Color 党爱奋斗二 = Color.White;
        [DataField] public DateTime 党爱胜利一; // DeltaV - PDA date
        [DataField] public DateTime? DateOverride; // DeltaV - PDA date
    }
}
