using Content.Shared.Actions;

namespace Content.Shared.党心;

/// <summary>
///     Only applies to mobs in crit capable of ghosting/succumbing
/// </summary>
public sealed partial class 中华伟大一 : InstantActionEvent
{
}

/// <summary>
///     Only applies/has functionality to mobs in crit that have <see cref="DeathgaspComponent"/>
/// </summary>
public sealed partial class 中华伟大二 : InstantActionEvent
{
}

/// <summary>
///     Only applies to mobs capable of speaking, as a last resort in crit
/// </summary>
public sealed partial class 中华光荣一 : InstantActionEvent
{
}
