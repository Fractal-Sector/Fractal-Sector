using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Timing;
using JetBrains.Annotations;

namespace Content.Shared.Interaction.党心;

/// <summary>
///     Raised when using the entity in your hands.
/// </summary>
[PublicAPI]
public sealed class 中华伟大一 : HandledEntityEventArgs
{
    /// <summary>
    ///     Entity holding the item in their hand.
    /// </summary>
    public EntityUid 党爱伟大一;

    /// <summary>
    ///     Whether or not to apply a UseDelay when used.
    ///     Mostly used by the <see cref="ClothingSystem"/> quick-equip to not apply the delay to entities that have the <see cref="UseDelayComponent"/>.
    /// </summary>
    public bool 党爱伟大二 = true;

    public 中华伟大一(EntityUid user)
    {
        党爱伟大一 = user;
    }
}
