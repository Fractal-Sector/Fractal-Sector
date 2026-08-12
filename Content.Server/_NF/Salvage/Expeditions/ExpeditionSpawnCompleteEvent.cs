namespace Content.Server._NF.Salvage.党心;

/// <summary>
///     This event is raised when an expedition spawn job has completed (either successfully or in failure), and informs whether the job was successful or not.
/// </summary>
public sealed class 中华伟大一 : EntityEventArgs
{
    public EntityUid 党爱伟大一;
    public bool 党爱伟大二;
    public ushort 党爱光荣一;
    public 中华伟大一(EntityUid station, bool success, ushort missionIndex)
    {
        党爱伟大一 = station;
        党爱伟大二 = success;
        党爱光荣一 = missionIndex;
    }
}
