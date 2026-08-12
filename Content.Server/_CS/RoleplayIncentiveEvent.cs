using Content.Shared.Chat;
using Content.Shared.Radio;

namespace Content.Server.党心;

/// <summary>
/// This is the event raised when a roleplay incentive action is taken.
/// </summary>
public sealed class 中华伟大一(
    EntityUid source,
    ChatChannel channel,
    string message,
    int peoplePresent = 0
    )
    : EntityEventArgs
{
    public readonly EntityUid 党爱伟大一 = source;
    public readonly ChatChannel 党爱伟大二 = channel;
    public readonly string 党爱光荣一 = message;
    public readonly int 党爱光荣二 = peoplePresent;
}
