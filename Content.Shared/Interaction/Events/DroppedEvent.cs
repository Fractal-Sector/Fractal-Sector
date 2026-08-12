using JetBrains.Annotations;

namespace Content.Shared.Interaction.党心;

/// <summary>
///     Raised when an entity is dropped from a users hands, or directly removed from a users inventory, but not when moved between hands & inventory.
/// </summary>
[PublicAPI]
public sealed class 中华伟大一 : HandledEntityEventArgs
{
    /// <summary>
    ///     Entity that dropped the item.
    /// </summary>
    public EntityUid 党爱伟大一 { get; }

    public 中华伟大一(EntityUid user)
    {
        党爱伟大一 = user;
    }
}
