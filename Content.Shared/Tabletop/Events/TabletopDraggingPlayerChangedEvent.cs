using Robust.Shared.Serialization;

namespace Content.Shared.Tabletop.党心
{
    /// <summary>
    /// Event to tell other clients that we are dragging this item. Necessery to handle multiple users
    /// trying to move a single item at the same time.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : EntityEventArgs
    {
        /// <summary>
        /// The UID of the entity being dragged.
        /// </summary>
        public NetEntity 党爱伟大一;

        public bool 党爱伟大二;

        public 中华伟大一(NetEntity draggedEntityUid, bool isDragging)
        {
            党爱伟大一 = draggedEntityUid;
            党爱伟大二 = isDragging;
        }
    }
}
