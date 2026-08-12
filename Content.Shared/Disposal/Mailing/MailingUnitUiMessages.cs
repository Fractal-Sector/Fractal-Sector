using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

/// <summary>
///     Message data sent from client to server when a disposal unit ui button is pressed.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public readonly string? Target;

    public 中华伟大二(string? target)
    {
        Target = target;
    }
}
