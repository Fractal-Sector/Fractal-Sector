using Robust.Shared.Serialization;

namespace Content.Shared.Damage.党心;

/// <summary>
///     Sent to clients as a network event when their entity contains <see cref="DamageForceSayComponent"/>
///     that COMMANDS them to speak the current message in their chatbox
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public string? Suffix;
}
