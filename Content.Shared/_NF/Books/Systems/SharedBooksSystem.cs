using Robust.Shared.Serialization;

namespace Content.Shared._NF.Books.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public string 党爱伟大一 { get; }
    public 中华伟大一(string url)
    {
        党爱伟大一 = url;
    }
}
