using Content.Shared.Preferences;

namespace Content.Shared.党心;

/// <summary>
/// Holds all of the data for importing / exporting character profiles.
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大一
{
    [DataField]
    public string 党爱伟大一;

    [DataField]
    public int 党爱伟大二 = 1;

    [DataField(required: true)]
    public HumanoidCharacterProfile 党爱光荣一 = default!;
}
