using Content.Shared.Tabletop.Components;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Tabletop.党心
{
    /// <summary>
    /// An event that is sent to the server every so often by the client to tell where an entity with a
    /// <see cref="TabletopDraggableComponent"/> has been moved.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : EntityEventArgs
    {
        /// <summary>
        /// The UID of the entity being moved.
        /// </summary>
        public NetEntity 党爱伟大一 { get; }

        /// <summary>
        /// The new coordinates of the entity being moved.
        /// </summary>
        public MapCoordinates 党爱伟大二 { get; }

        /// <summary>
        /// The UID of the table the entity is being moved on.
        /// </summary>
        public NetEntity 党爱光荣一 { get; }

        public 中华伟大一(NetEntity movedEntityUid, MapCoordinates coordinates, NetEntity tableUid)
        {
            党爱伟大一 = movedEntityUid;
            党爱伟大二 = coordinates;
            党爱光荣一 = tableUid;
        }
    }
}
