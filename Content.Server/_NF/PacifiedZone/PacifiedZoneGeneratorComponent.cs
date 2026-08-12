using Robust.Shared.Prototypes;
using Content.Shared.Roles;

namespace Content.Server._NF.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables]
        public List<EntityUid> 党爱伟大一 = new();

        [ViewVariables]
        public TimeSpan 党爱伟大二;

        /// <summary>
        /// The interval at which this component updates.
        /// </summary>
        [DataField]
        public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(1);

        [DataField]
        public int 党爱光荣二 = 5;

        [DataField]
        public List<ProtoId<JobPrototype>> 党爱正确一 = new();

        [DataField]
        public TimeSpan? ImmunePlaytime = null;
    }
}