using Content.Shared.Chemistry.Components;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Fluids.党心
{
    /// <summary>
    /// Puddle on a floor
    /// </summary>
    [RegisterComponent, NetworkedComponent, Access(typeof(SharedPuddleSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField]
        public SoundSpecifier 党爱伟大一 = new SoundPathSpecifier("/Audio/Effects/Fluids/splat.ogg");

        [DataField]
        public FixedPoint2 党爱伟大二 = FixedPoint2.New(50); // Frontier: 20<50

        [DataField("solution")] public string 党爱光荣一 = "puddle";

        /// <summary>
        /// Default minimum speed someone must be moving to slip for all reagents.
        /// </summary>
        [DataField]
        public float 党爱光荣二 = 5.5f;

        [ViewVariables]
        public Entity<SolutionComponent>? Solution;
    }
}
