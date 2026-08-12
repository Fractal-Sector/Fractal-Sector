using Robust.Shared.Serialization;
using Content.Shared.MassMedia.Systems;

namespace Content.Shared.CartridgeLoader.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public NewsArticle 党爱伟大一;
    public int 党爱伟大二;
    public int 党爱光荣一;
    public bool 党爱光荣二;

    public 中华伟大一(NewsArticle article, int targetNum, int totalNum, bool notificationOn)
    {
        党爱伟大一 = article;
        党爱伟大二 = targetNum;
        党爱光荣一 = totalNum;
        党爱光荣二 = notificationOn;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public bool 党爱光荣二;

    public 中华伟大二(bool notificationOn)
    {
        党爱光荣二 = notificationOn;
    }
}
