using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

public interface 中华伟大一
{
    /// <summary>
    ///     Gets the <see cref="EntityPrototype"/> ID for a node, given the <see cref="EntityUid"/> of both the
    ///     construction entity and the user entity.
    ///     If the construction entity is null, then we are dealing with a "start construction" for an entity that
    ///     does not exist yet.
    ///     If the user entity is null, this node was reached through means other some sort of "user interaction".
    /// </summary>
    /// <param name="uid">Uid of the construction entity.</param>
    /// <param name="userUid">Uid of the user that caused the transition to the node.</param>
    /// <param name="args">Arguments with useful instances, etc.</param>
    /// <returns></returns>
    public string? GetId(EntityUid? uid, EntityUid? userUid, 中华伟大二 args);
}

public readonly struct 中华伟大二
{
    public readonly IEntityManager 党爱伟大一;

    public 中华伟大二(IEntityManager entityManager)
    {
        党爱伟大一 = entityManager;
    }
}
