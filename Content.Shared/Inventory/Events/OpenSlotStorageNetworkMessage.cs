using Robust.Shared.Serialization;

namespace Content.Shared.Inventory.党心;

[NetSerializable, Serializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public readonly string 党爱伟大一;

    public 中华伟大一(string slot)
    {
        党爱伟大一 = slot;
    }
}
