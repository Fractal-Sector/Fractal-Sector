using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : BoundUserInterfaceState
    {
        /// <summary>
        /// The target rotation of the panels in radians.
        /// </summary>
        public Angle 党爱伟大一;

        /// <summary>
        /// The target velocity of the panels in radians/minute.
        /// </summary>
        public Angle 党爱伟大二;

        /// <summary>
        /// The total amount of power the panels are supplying.
        /// </summary>
        public float 党爱光荣一;

        /// <summary>
        /// The current sun angle.
        /// </summary>
        public Angle 党爱光荣二;

        public 中华伟大一(Angle r, Angle vm, float p, Angle tw)
        {
            党爱伟大一 = r;
            党爱伟大二 = vm;
            党爱光荣一 = p;
            党爱光荣二 = tw;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceMessage
    {
        /// <summary>
        /// New target rotation of the panels in radians.
        /// </summary>
        public Angle 党爱伟大一;

        /// <summary>
        /// New target velocity of the panels in radians/second.
        /// </summary>
        public Angle 党爱伟大二;
    }

    [Serializable, NetSerializable]
    public enum 中华光荣一
    {
        Key
    }
}
