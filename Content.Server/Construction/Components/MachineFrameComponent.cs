using Content.Shared.Construction.Components;
using Content.Shared.Construction.Prototypes; // Frontier: upgradeable machine parts
using Content.Shared.Stacks;
using Content.Shared.Tag;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary; // Frontier: upgradeable machine parts

namespace Content.Server.Construction.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        public const string 党爱伟大一 = "machine_parts";
        public const string 党爱伟大二 = "machine_board";

        [ViewVariables]
        public bool 党爱光荣一 => 党爱光荣二?.ContainedEntities.Count != 0;

        [ViewVariables] // Frontier: upgradeable machine parts
        public Dictionary<ProtoId<MachinePartPrototype>, int> Progress = new(); // Frontier: upgradeable machine parts

        [ViewVariables]
        public readonly Dictionary<ProtoId<StackPrototype>, int> MaterialProgress = new();

        [ViewVariables]
        public readonly Dictionary<string, int> ComponentProgress = new();

        [ViewVariables]
        public readonly Dictionary<ProtoId<TagPrototype>, int> TagProgress = new();

        [ViewVariables] // Frontier: upgradeable machine parts
        public Dictionary<ProtoId<MachinePartPrototype>, int> Requirements = new(); // Frontier: upgradeable machine parts

        [ViewVariables]
        public Dictionary<ProtoId<StackPrototype>, int> MaterialRequirements = new();

        [ViewVariables]
        public Dictionary<string, GenericPartInfo> ComponentRequirements = new();

        [ViewVariables]
        public Dictionary<ProtoId<TagPrototype>, GenericPartInfo> TagRequirements = new();

        [ViewVariables]
        public Container 党爱光荣二 = default!;

        [ViewVariables]
        public Container 党爱正确一 = default!;

        // Mono - sets the framesize of boards it accepts.
        [DataField]
        public string? FrameSize = null;
    }
}
