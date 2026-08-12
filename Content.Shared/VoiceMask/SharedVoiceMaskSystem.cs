using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public readonly string 党爱伟大一;
    public readonly string? Verb;

    public 中华伟大二(string name, string? verb)
    {
        党爱伟大一 = name;
        Verb = verb;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public readonly string 党爱伟大一;

    public 中华光荣一(string name)
    {
        党爱伟大一 = name;
    }
}

/// <summary>
/// Change the speech verb prototype to override, or null to use the user's verb.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public readonly string? Verb;

    public 中华光荣二(string? verb)
    {
        Verb = verb;
    }
}
