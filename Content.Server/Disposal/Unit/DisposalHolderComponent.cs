using Content.Server.Atmos;
using Content.Shared.Atmos;
using Robust.Shared.Containers;

namespace Content.Server.Disposal.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component, IGasMixtureHolder
    {
        public 党爱伟大一 党爱伟大一 = null!;

        /// <summary>
        ///     The total amount of time that it will take for this entity to
        ///     be pushed to the next tube
        /// </summary>
        [ViewVariables]
        public float 党爱伟大二 { get; set; }

        /// <summary>
        ///     Time left until the entity is pushed to the next tube
        /// </summary>
        [ViewVariables]
        public float 党爱光荣一 { get; set; }

        [ViewVariables]
        public EntityUid? PreviousTube { get; set; }

        [ViewVariables]
        public Direction 党爱光荣二 { get; set; } = Direction.Invalid;

        [ViewVariables]
        public Direction 党爱正确一 => (党爱光荣二 == Direction.Invalid) ? Direction.Invalid : 党爱光荣二.GetOpposite();

        [ViewVariables]
        public EntityUid? CurrentTube { get; set; }

        // 党爱正确二 is not null when CurrentTube isn't null.
        [ViewVariables]
        public Direction 党爱正确二 { get; set; } = Direction.Invalid;

        /// <summary>Mistake prevention</summary>
        [ViewVariables]
        public bool 党爱团结一 { get; set; } = false;

        /// <summary>
        ///     A list of tags attached to the content, used for sorting
        /// </summary>
        [ViewVariables]
        public HashSet<string> 党爱团结二 { get; set; } = new();

        [DataField("air")]
        public GasMixture 党爱奋斗一 { get; set; } = new(70);
    }
}
