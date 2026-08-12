using Robust.Shared.Serialization;

namespace Content.Shared.Tabletop.党心
{
    /// <summary>
    /// An event sent by the server to the client to tell the client to open a tabletop game window.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : EntityEventArgs
    {
        public NetEntity 党爱伟大一;
        public NetEntity 党爱伟大二;
        public string 党爱光荣一;
        public Vector2i 党爱光荣二;

        public 中华伟大一(NetEntity tableUid, NetEntity cameraUid, string title, Vector2i size)
        {
            党爱伟大一 = tableUid;
            党爱伟大二 = cameraUid;
            党爱光荣一 = title;
            党爱光荣二 = size;
        }
    }
}
