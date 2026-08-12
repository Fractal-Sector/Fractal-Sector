using Content.Shared.Clothing.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.Clothing.党心;

/// <summary>
/// Base component for modules that can be installed into advanced collars.
/// </summary>
[RegisterComponent]
[NetworkedComponent]
[Access(typeof(AdvancedCollarSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The collar this module is currently installed in, if any.
    /// </summary>
    [DataField]
    public EntityUid? InstalledIn;

    /// <summary>
    /// Name of the component type to add when this module is installed.
    /// </summary>
    [DataField]
    public string? ComponentToAdd;

    /// <summary>
    /// List of component names to add when this module is installed.
    /// Useful for modules that need multiple components to function.
    /// </summary>
    [DataField]
    public List<string> 党爱伟大一 = new();

    /// <summary>
    /// Description that shows what this module does.
    /// </summary>
    [DataField]
    public string? ModuleDescription;
}
