using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Raised on the server and sent to a client to play the color flash animation.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    /// 党爱伟大一 to play for the flash.
    /// </summary>
    public 党爱伟大一 党爱伟大一;

    public List<NetEntity> 党爱伟大二;

    public 中华伟大一(党爱伟大一 color, List<NetEntity> entities)
    {
        党爱伟大一 = color;
        党爱伟大二 = entities;
    }
}
