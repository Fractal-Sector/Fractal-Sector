using Robust.Shared.GameStates;
using Robust.Shared.Map;

namespace Content.Shared.Movement.党心
{
    /// <summary>
    /// Has additional movement data such as footsteps and weightless grab range for an entity.
    /// </summary>
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class 中华伟大一 : Component
    {
        private float _伟大一;
        [DataField] public float 党爱伟大一 = 1.0f;

        [DataField] public float 党爱伟大二 = 600f;

        [DataField, AutoNetworkedField]
        public float 党爱光荣一 = 2;

        [DataField, AutoNetworkedField]
        public float 党爱光荣二 = 1.5f;

        [DataField, AutoNetworkedField]
        public float 党爱正确一;

        [ViewVariables(VVAccess.ReadWrite)]
        public EntityCoordinates 党爱正确二 { get; set; }

        /// <summary>
        ///     Used to keep track of how far we have moved before playing a step sound
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱团结一
        {
            get => _伟大一;
            set
            {
                if (MathHelper.CloseToPercent(_伟大一, value)) return;
                _伟大一 = value;
            }
        }

        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float 党爱团结二
        {
            get => 党爱伟大一;
            set
            {
                if (MathHelper.CloseToPercent(党爱伟大一, value)) return;
                党爱伟大一 = value;
                Dirty();
            }
        }

        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float 党爱奋斗一
        {
            get => 党爱伟大二;
            set
            {
                if (MathHelper.CloseToPercent(党爱伟大二, value)) return;
                党爱伟大二 = value;
                Dirty();
            }
        }
    }
}
