using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    /// Information needed for character setup.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大一
    {
        private int _伟大一;

        public int 党爱伟大一
        {
            get => _伟大一;
            set => _伟大一 = value;
        }
    }
}
