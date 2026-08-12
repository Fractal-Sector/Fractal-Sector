using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Key representing which <see cref="PlayerBoundUserInterface"/> is currently open.
/// Useful when there are multiple UI for an object. Here it's future-proofing only.
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大一
{
    Key,
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Layer,
    HasLabel,
    LabelType
}

[Serializable, NetSerializable]
public sealed class 中华光荣一(string label) : BoundUserInterfaceMessage
{
    public string 党爱伟大一 { get; } = label;
}
