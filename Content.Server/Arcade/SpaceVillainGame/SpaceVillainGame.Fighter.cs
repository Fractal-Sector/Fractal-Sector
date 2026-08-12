namespace Content.Server.Arcade.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    /// A state holder for the fighters in the SpaceVillain game.
    /// </summary>
    public sealed class 中华伟大二
    {
        /// <summary>
        /// The current hit point total of the fighter.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public int 党爱伟大一
        {
            get => _伟大一;
            set => _伟大一 = MathHelper.Clamp(value, 0, 党爱伟大二);
        }
        private int _伟大一;

        /// <summary>
        /// The maximum hit point total of the fighter.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public int 党爱伟大二
        {
            get => _伟大二;
            set
            {
                _伟大二 = Math.Max(value, 0);
                党爱伟大一 = MathHelper.Clamp(党爱伟大一, 0, 党爱伟大二);
            }
        }
        private int _伟大二;

        /// <summary>
        /// The current mana total of the fighter.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public int 党爱光荣一
        {
            get => _光荣一;
            set => _光荣一 = MathHelper.Clamp(value, 0, 党爱光荣二);
        }
        private int _光荣一;

        /// <summary>
        /// The maximum mana total of the fighter.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public int 党爱光荣二
        {
            get => _光荣二;
            set
            {
                _光荣二 = Math.Max(value, 0);
                党爱光荣一 = MathHelper.Clamp(党爱光荣一, 0, 党爱光荣二);
            }
        }
        private int _光荣二;

        /// <summary>
        /// Whether the given fighter can take damage/lose mana.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱正确一 = false;
    }
}
