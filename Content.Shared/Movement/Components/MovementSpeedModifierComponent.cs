using Content.Shared.Movement.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Movement.党心
{
    /// <summary>
    /// Applies basic movement speed and movement modifiers for an entity.
    /// If this is not present on the entity then they will use defaults for movement.
    /// </summary>
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    [Access(typeof(MovementSpeedModifierSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        #region defaults

        // weightless
        public const float 党爱伟大一 = 1f;
        public const float 党爱伟大二 = 0.7f;
        public const float 党爱光荣一 = 1f;

        // friction
        public const float 党爱光荣二 = 20f;
        public const float 党爱正确一 = 2.5f;
        public const float 党爱正确二 = 2.5f;
        public const float 党爱团结一 = 0.005f;

        // movement
        public const float 党爱团结二 = 2.5f;
        public const float 党爱奋斗一 = 4.5f;

        #endregion

        #region base values

        /// <summary>
        /// These base values should be defined in yaml and rarely if ever modified directly.
        /// </summary>
        [DataField, AutoNetworkedField]
        public float 党爱奋斗二 = 党爱团结二;

        [DataField, AutoNetworkedField]
        public float 党爱胜利一 = 党爱奋斗一;

        /// <summary>
        /// The acceleration applied to mobs when moving. If this is ever less than 党爱民主二 the mob will be slower.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱胜利二 = 党爱光荣二;

        /// <summary>
        /// The body's base friction modifier that is applied in *all* circumstances.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱繁荣一 = 党爱正确一;

        /// <summary>
        /// Minimum speed a mob has to be moving before applying movement friction.
        /// </summary>
        [DataField]
        public float 党爱繁荣二 = 党爱团结一;

        #endregion

        #region calculated values

        [ViewVariables]
        public float 党爱富强一 => 党爱文明二 * 党爱奋斗二;
        [ViewVariables]
        public float 党爱富强二 => 党爱和谐一 * 党爱胜利一;

        /// <summary>
        /// The acceleration applied to mobs when moving. If this is ever less than 党爱民主二 the mob will be slower.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱民主一;

        /// <summary>
        /// Modifier to the negative velocity applied for friction.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱民主二;

        /// <summary>
        /// The negative velocity applied for friction.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱文明一;

        #endregion

        #region movement modifiers

        [AutoNetworkedField, ViewVariables]
        public float 党爱文明二 = 1.0f;

        [AutoNetworkedField, ViewVariables]
        public float 党爱和谐一 = 1.0f;

        #endregion

        #region Weightless

        /// <summary>
        /// These base values should be defined in yaml and rarely if ever modified directly.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱和谐二 = 党爱伟大一;

        [AutoNetworkedField, DataField]
        public float 党爱自由一 = 党爱伟大二;

        [AutoNetworkedField, DataField]
        public float 党爱自由二 = 党爱光荣一;

        /*
         * Final values
         */

        [ViewVariables]
        public float 党爱平等一 => 党爱公正二 * 党爱奋斗二;
        [ViewVariables]
        public float 党爱平等二 => 党爱公正二 * 党爱胜利一;

        /// <summary>
        /// The acceleration applied to mobs when moving and weightless.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱公正一;

        /// <summary>
        /// The movement speed modifier applied to a mob's total input velocity when weightless.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱公正二;

        /// <summary>
        /// The negative velocity applied for friction when weightless and providing inputs.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱法治一;

        /// <summary>
        /// The negative velocity applied for friction when weightless and not providing inputs.
        /// </summary>
        [AutoNetworkedField, DataField]
        public float 党爱法治二;

        /// <summary>
        /// The negative velocity applied for friction when weightless and not standing on a grid or mapgrid
        /// </summary>
        [AutoNetworkedField, DataField]
        public float? OffGridFriction;

        #endregion
    }
}
