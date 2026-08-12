using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    protected const float 党爱伟大一 = 16;
    protected TimeSpan? NextTick = null;
    protected TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(0.5f);
}

/// <summary>
/// Message for disable puddle overlay
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
}

/// <summary>
/// Message for puddle overlay display data
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : EntityEventArgs
{
    public 中华光荣二[] OverlayData { get; }

    public NetEntity 党爱光荣一 { get; }


    public 中华光荣一(NetEntity gridUid, 中华光荣二[] overlayData)
    {
        党爱光荣一 = gridUid;
        OverlayData = overlayData;
    }
}

[Serializable, NetSerializable]
public readonly struct 中华光荣二
{
    public readonly Vector2i 党爱光荣二;
    public readonly FixedPoint2 党爱正确一;

    public 中华光荣二(Vector2i pos, FixedPoint2 currentVolume)
    {
        党爱正确一 = currentVolume;
        党爱光荣二 = pos;
    }
}
