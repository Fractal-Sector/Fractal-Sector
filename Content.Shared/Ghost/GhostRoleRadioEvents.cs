using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Ghost.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public 党爱伟大一<GhostRolePrototype> 党爱伟大一;

    public 中华伟大一(党爱伟大一<GhostRolePrototype> protoId)
    {
        党爱伟大一 = protoId;
    }
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Key
}
