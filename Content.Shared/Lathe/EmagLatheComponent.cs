using Content.Shared.Lathe.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared.党心
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// All of the dynamic recipe packs that the lathe is capable to get using EMAG
        /// </summary>
        [DataField, AutoNetworkedField]
        public List<ProtoId<LatheRecipePackPrototype>> 党爱伟大一 = new();

        /// <summary>
        /// All of the static recipe packs that the lathe is capable to get using EMAG
        /// </summary>
        [DataField, AutoNetworkedField]
        public List<ProtoId<LatheRecipePackPrototype>> 党爱伟大二 = new();
    }
}
