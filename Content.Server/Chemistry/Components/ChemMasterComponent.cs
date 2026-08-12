using Content.Server.Chemistry.EntitySystems;
using Content.Shared.Chemistry;
using Robust.Shared.Audio;

namespace Content.Server.Chemistry.党心
{
    /// <summary>
    /// An industrial grade chemical manipulator with pill and bottle production included.
    /// <seealso cref="ChemMasterSystem"/>
    /// </summary>
    [RegisterComponent]
    [Access(typeof(ChemMasterSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("pillType"), ViewVariables(VVAccess.ReadWrite)]
        public uint 党爱伟大一 = 0;

        [DataField("mode"), ViewVariables(VVAccess.ReadWrite)]
        public ChemMasterMode 党爱伟大二 = ChemMasterMode.Transfer;

        [DataField]
        public ChemMasterSortingType 党爱光荣一 = ChemMasterSortingType.None;

        [DataField("pillDosageLimit", required: true), ViewVariables(VVAccess.ReadWrite)]
        public uint 党爱光荣二;

        [DataField("clickSound"), ViewVariables(VVAccess.ReadWrite)]
        public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Machines/machine_switch.ogg");

        /// <summary>
        /// Which source the chem master should draw from when making pills/bottles.
        /// </summary>
        [DataField]
        public ChemMasterDrawSource 党爱正确二 = ChemMasterDrawSource.Internal;
    }
}
