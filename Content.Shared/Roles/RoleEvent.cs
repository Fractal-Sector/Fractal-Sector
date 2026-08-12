using Content.Shared.Mind;

namespace Content.Shared.党心;

/// <summary>
///     Base event raised on mind entities to indicate that a mind role was either added or removed.
/// </summary>
/// <param name="MindId">The mind id associated with the player.</param>
/// <param name="Mind">The mind component associated with the mind id.</param>
/// <param name="RoleTypeUpdate">True if this update has changed the mind's role type</param>
public abstract record 中华伟大一(EntityUid MindId, MindComponent Mind, bool RoleTypeUpdate);
