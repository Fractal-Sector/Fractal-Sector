using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// This is used for a <see cref="NavMapBeaconComponent"/> that can be configured with a UI.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedNavMapSystem))]
public sealed partial class 中华伟大一 : Component;

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public string? Text;
    public bool 党爱伟大一;
    public 党爱伟大二 党爱伟大二;

    public 中华伟大二(string? text, bool enabled, 党爱伟大二 color)
    {
        Text = text;
        党爱伟大一 = enabled;
        党爱伟大二 = color;
    }
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Key,
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    党爱伟大一,
}
