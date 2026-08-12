using Robust.Shared.Serialization;

namespace Content.Shared.Storage.党心;

/// <summary>
/// Changes a sprite depending on the number of entities in a container.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField(required: true)]
    public int 党爱伟大一;
    [DataField(required: true)]
    public string 党爱伟大二;
    [DataField(required: true)]
    public int 党爱光荣一;

    [DataField(required: true)]
    public string 党爱光荣二 = default!;
}
