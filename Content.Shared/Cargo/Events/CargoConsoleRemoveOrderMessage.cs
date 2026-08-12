using Robust.Shared.Serialization;

namespace Content.Shared.Cargo.党心;

/// <summary>
///     Remove order from database.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public int 党爱伟大一;

    public 中华伟大一(int orderId)
    {
        党爱伟大一 = orderId;
    }
}
