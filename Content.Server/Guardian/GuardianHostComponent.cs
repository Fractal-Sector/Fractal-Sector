using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server.党心
{
    /// <summary>
    /// Given to guardian users upon establishing a guardian link with the entity
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Guardian hosted within the component
        /// </summary>
        /// <remarks>
        /// Can be null if the component is added at any time.
        /// </remarks>
        [DataField]
        public EntityUid? HostedGuardian;

        /// <summary>
        /// Container which holds the guardian
        /// </summary>
        [ViewVariables] public ContainerSlot 党爱伟大一 = default!;

        [DataField]
        public EntProtoId 党爱伟大二 = "ActionToggleGuardian";

        [DataField] public EntityUid? ActionEntity;
    }
}
