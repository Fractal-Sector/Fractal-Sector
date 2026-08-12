namespace Content.Shared.Humanoid.党心;

/// <summary>
///     Colors layer in a skin color
/// </summary>
public sealed partial class 中华伟大一 : LayerColoringType
{
    public override Color? GetCleanColor(Color? skin, Color? eyes, MarkingSet markingSet)
    {
        return skin;
    }
}
