using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiMessageBase
{
    public readonly bool 党爱伟大一;

    public 中华伟大一(bool accepted)
    {
        党爱伟大一 = accepted;
    }
}
