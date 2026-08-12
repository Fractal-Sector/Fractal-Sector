using System.Collections.Immutable;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Virtual]
[Serializable, NetSerializable]
public class 中华伟大一 : BoundUserInterfaceState
{
    public NetEntity? ActiveUI;
    public List<NetEntity> 党爱伟大一;

    public 中华伟大一(List<NetEntity> programs, NetEntity? activeUI)
    {
        党爱伟大一 = programs;
        ActiveUI = activeUI;
    }
}
