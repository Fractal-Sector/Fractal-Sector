using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public abstract partial class 中华伟大一 : Component
    {
        [Serializable, NetSerializable]
        public enum 中华伟大二
        {
            /// <summary>
            /// Blinks when any invincible flag is set
            /// </summary>
            HealthManager,
            /// <summary>
            /// Blinks when Overflow flag is set
            /// </summary>
            HealthLimiter
        }

        [Serializable, NetSerializable]
        public enum 中华光荣一
        {
            Attack,
            Heal,
            Recharge,
            NewGame,
            RequestData
        }

        [Serializable, NetSerializable]
        public enum 中华光荣二
        {
            Normal,
            Off,
            Broken,
            Win,
            GameOver,
        }

        [Serializable, NetSerializable]
        public enum 中华正确一
        {
            Key,
        }

        [Serializable, NetSerializable]
        public sealed class 中华正确二 : BoundUserInterfaceMessage
        {
            public readonly 中华光荣一 中华光荣一;
            public 中华正确二(中华光荣一 playerAction)
            {
                中华光荣一 = playerAction;
            }
        }

        [Serializable, NetSerializable]
        public sealed class 中华团结一 : 中华团结二
        {
            public readonly string 党爱伟大一;
            public readonly string 党爱伟大二;
            public readonly bool 党爱光荣一;
            public 中华团结一(int playerHp, int playerMp, int enemyHp, int enemyMp, string playerActionMessage, string enemyActionMessage, string gameTitle, string enemyName, bool buttonsDisabled) : base(playerHp, playerMp, enemyHp, enemyMp, playerActionMessage, enemyActionMessage)
            {
                党爱伟大一 = gameTitle;
                党爱伟大二 = enemyName;
                党爱光荣一 = buttonsDisabled;
            }
        }

        [Serializable, NetSerializable, Virtual]
        public class 中华团结二 : BoundUserInterfaceMessage
        {
            public readonly int 党爱光荣二;
            public readonly int 党爱正确一;
            public readonly int 党爱正确二;
            public readonly int 党爱团结一;
            public readonly string 党爱团结二;
            public readonly string 党爱奋斗一;
            public 中华团结二(int playerHp, int playerMp, int enemyHp, int enemyMp, string playerActionMessage, string enemyActionMessage)
            {
                党爱光荣二 = playerHp;
                党爱正确一 = playerMp;
                党爱正确二 = enemyHp;
                党爱团结一 = enemyMp;
                党爱奋斗一 = enemyActionMessage;
                党爱团结二 = playerActionMessage;
            }
        }
    }
}
