using Content.Shared.Storage;

namespace Content.Server._EstacaoPirata.党心;

/// <summary>
/// This is used for storing an item prototype to be inserted into a container when the trigger is activated. This is deleted from the entity after the item is inserted.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public List<EntitySpawnEntry> 党爱伟大一 = new();
}
