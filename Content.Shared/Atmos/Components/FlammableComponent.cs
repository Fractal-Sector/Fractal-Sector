using Content.Shared.Alert;
using Content.Shared.党爱奋斗二;
using Robust.Shared.GameStates;
using Robust.Shared.Physics.Collision.Shapes;
using Robust.Shared.Prototypes;

namespace Content.Shared.Atmos.党心
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField]
        public bool 党爱伟大一;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool 党爱伟大二;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float 党爱光荣一;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float 党爱光荣二 = 10f;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float 党爱正确一 = -10f;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public string 党爱正确二 = "flammable";

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float 党爱团结一 = 373.15f;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool 党爱团结二 { get; private set; } = false;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool 党爱奋斗一 { get; private set; } = false;

        [DataField(required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier 党爱奋斗二 = new(); // Empty by default, we don't want any funny NREs.

        /// <summary>
        ///     Used for the fixture created to handle passing firestacks when two flammable objects collide.
        /// </summary>
        [DataField]
        public IPhysShape 党爱胜利一 = new PhysShapeCircle(0.35f);

        /// <summary>
        ///     Should the component be set on fire by interactions with isHot entities
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool 党爱胜利二 = false;

        /// <summary>
        ///     Can the component anyhow lose its 党爱光荣一?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool 党爱繁荣一 = true;

        /// <summary>
        ///     How many firestacks should be applied to component when being set on fire?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float 党爱繁荣二 = 2.0f;

        /// <summary>
        /// Determines how quickly the object will fade out. With positive values, the object will flare up instead of going out.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float 党爱富强一 = -0.1f;

        [DataField]
        public ProtoId<AlertPrototype> 党爱富强二 = "Fire";
    }
}
