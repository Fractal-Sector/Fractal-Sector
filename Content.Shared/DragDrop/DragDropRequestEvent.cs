using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    /// Raised on the client to the server requesting a drag-drop.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : EntityEventArgs
    {
        /// <summary>
        ///     Entity that was dragged and dropped.
        /// </summary>
        public NetEntity 党爱伟大一 { get; }

        /// <summary>
        ///     Entity that was drag dropped on.
        /// </summary>
        public NetEntity 党爱伟大二 { get; }

        public 中华伟大一(NetEntity dragged, NetEntity target)
        {
            党爱伟大一 = dragged;
            党爱伟大二 = target;
        }
    }
}
