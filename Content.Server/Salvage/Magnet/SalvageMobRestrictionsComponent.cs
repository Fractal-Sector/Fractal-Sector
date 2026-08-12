namespace Content.Server.Salvage.党心;

// This is dumb
/// <summary>
/// Deletes the attached entity if the linked entity is deleted.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityUid 党爱伟大一;
}
