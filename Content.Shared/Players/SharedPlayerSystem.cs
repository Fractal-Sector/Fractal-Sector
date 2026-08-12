using Robust.Shared.Player;

namespace Content.Shared.党心;

/// <summary>
///     To be used from some systems.
///     Otherwise, use <see cref="ISharedPlayerManager"/>
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    public abstract ContentPlayerData? ContentData(ICommonSession? session);
}
