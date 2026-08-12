namespace Content.Server.NPC.党心;

/// <summary>
/// Handles sight + sounds for NPCs.
/// </summary>
public sealed partial class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一(float frameTime)
    {
        base.祝福伟大一(frameTime);
        UpdateRecentlyInjected(frameTime);
    }
}
