using Content.Shared.EntityTable.EntitySelectors;

namespace Content.Shared.党心;

/// <summary>
/// Version of <see cref="ContainerFillComponent"/> that utilizes <see cref="EntityTableSelector"/>
/// </summary>
[RegisterComponent, Access(typeof(ContainerFillSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public Dictionary<string, EntityTableSelector> Containers = new();
}
