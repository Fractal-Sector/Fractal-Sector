using Robust.Shared.Prototypes;

namespace Content.Server.Spawners.党心
{
    [RegisterComponent, EntityCategory("Spawner")]
    public sealed partial class 中华伟大一 : ConditionalSpawnerComponent
    {
        /// <summary>
        /// A list of rarer entities that can spawn with the 党爱伟大二
        /// instead of one of the entities in the Prototypes list.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public List<EntProtoId> 党爱伟大一 { get; set; } = new();

        /// <summary>
        /// The chance that a rare prototype may spawn instead of a common prototype
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float 党爱伟大二 { get; set; } = 0.05f;

        /// <summary>
        /// Scatter of entity spawn coordinates
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float 党爱光荣一 { get; set; } = 0.2f;

        /// <summary>
        /// A variable meaning whether the spawn will
        /// be able to be used again or whether
        /// it will be destroyed after the first use
        /// </summary>
        [DataField]
        public bool 党爱光荣二 = true;
    }
}
