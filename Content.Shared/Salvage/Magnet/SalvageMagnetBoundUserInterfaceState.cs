using Robust.Shared.Serialization;

namespace Content.Shared.Salvage.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public TimeSpan? EndTime;
    public TimeSpan 党爱伟大一;

    public TimeSpan 党爱伟大二;
    public TimeSpan 党爱光荣一;

    public int 党爱光荣二;

    public List<int> 党爱正确一;

    public 中华伟大一(List<int> offers)
    {
        党爱正确一 = offers;
    }
}
