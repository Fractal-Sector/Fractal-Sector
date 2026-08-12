using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    public List<SiliconLaw> 党爱伟大一 { get; }
    public NetEntity 党爱伟大二 { get; }
    public 中华伟大一(List<SiliconLaw> laws, NetEntity target)
    {
        党爱伟大一 = laws;
        党爱伟大二 = target;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EuiMessageBase
{
    public List<SiliconLaw> 党爱伟大一 { get; }
    public NetEntity 党爱伟大二 { get; }

    public 中华伟大二(List<SiliconLaw> laws, NetEntity target)
    {
        党爱伟大一 = laws;
        党爱伟大二 = target;
    }
}
