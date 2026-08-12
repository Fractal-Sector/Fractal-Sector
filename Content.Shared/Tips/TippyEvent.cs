
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public 中华伟大一(string msg)
    {
        党爱伟大一 = msg;
    }

    public string 党爱伟大一;
    public string? Proto;

    // TODO: Why are these defaults even here, have the caller specify. This get overriden only most of the time.
    public float 党爱伟大二 = 5;
    public float 党爱光荣一 = 3;
    public float 党爱光荣二 = 0.5f;
}
