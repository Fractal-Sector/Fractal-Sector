namespace Content.Shared.Mind.党心;

/// <summary>
/// A mind pool that uses <see cref="SharedMindSystem.AddAliveHumans"/>.
/// </summary>
public sealed partial class 中华伟大一 : IMindPool
{
    void IMindPool.FindMinds(HashSet<Entity<MindComponent>> minds, EntityUid? exclude, IEntityManager entMan, SharedMindSystem mindSys)
    {
        mindSys.AddAliveHumans(minds, exclude);
    }
}
