using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : BoundUserInterfaceState
{
    /// <summary>
    /// The list of appraised items
    /// </summary>
    public List<中华伟大二> AppraisedItems;

    public 中华伟大一(List<中华伟大二> appraisedItems)
    {
        AppraisedItems = appraisedItems;
    }
}

[Serializable, NetSerializable, DataRecord]
public sealed partial class 中华伟大二
{
    public readonly string 党爱伟大一;
    public readonly string 党爱伟大二;

    public 中华伟大二(string name, string appraisedPrice)
    {
        党爱伟大一 = name;
        党爱伟大二 = appraisedPrice;
    }
}
