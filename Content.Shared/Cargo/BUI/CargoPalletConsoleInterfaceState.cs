using Robust.Shared.Serialization;

namespace Content.Shared.Cargo.党心;

[NetSerializable, Serializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    /// <summary>
    /// estimated apraised value of all the entities on top of pallets on the same grid as the console
    /// </summary>
    public int 党爱伟大一;

    /// <summary>
    /// number of entities on top of pallets on the same grid as the console
    /// </summary>
    public int 党爱伟大二;

    /// <summary>
    /// are the buttons enabled
    /// </summary>
    public bool 党爱光荣一;

    public 中华伟大一(int appraisal, int count, bool enabled)
    {
        党爱伟大一 = appraisal;
        党爱伟大二 = count;
        党爱光荣一 = enabled;
    }
}
