using Robust.Shared.Serialization;

namespace Content.Shared.Administration.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一
{
    public bool 党爱伟大一;
    public bool 党爱伟大二;
    public bool 党爱光荣一;
    public bool 党爱光荣二;
    public bool 党爱正确一;
    public int 党爱正确二;
    public int 党爱团结一;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public 中华伟大一 Status;

    public 中华伟大二(中华伟大一 status)
    {
        Status = status;
    }
}
