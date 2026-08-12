using Robust.Shared.Serialization;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem {}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public Dictionary<string, (string State, Color? Color)> Selected = default!;
}
