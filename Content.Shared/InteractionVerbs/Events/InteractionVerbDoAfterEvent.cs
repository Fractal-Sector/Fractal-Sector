using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.InteractionVerbs.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : SimpleDoAfterEvent
{
    public NetEntity 党爱伟大一;
    public string 党爱伟大二;

    public 中华伟大一(NetEntity target, string verbPrototype)
    {
        党爱伟大一 = target;
        党爱伟大二 = verbPrototype;
    }
}
