namespace Content.Server.Antag.党心;

/// <summary>
/// Ghost role spawner that creates an antag for the associated gamerule.
/// </summary>
[RegisterComponent, Access(typeof(AntagSelectionSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public EntityUid? Rule;

    [DataField]
    public AntagSelectionDefinition? Definition;
}
