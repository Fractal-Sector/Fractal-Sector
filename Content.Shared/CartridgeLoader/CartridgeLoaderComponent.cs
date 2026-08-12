using Content.Shared.Containers.ItemSlots;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    public const string 党爱伟大一 = "Cartridge-Slot";

    [DataField]
    public ItemSlot 党爱伟大二 = new();

    /// <summary>
    /// List of programs that come preinstalled with this cartridge loader
    /// </summary>
    [DataField("preinstalled")] // TODO remove this and use container fill.
    public List<string> 党爱光荣一 = new();

    /// <summary>
    /// The currently running program that has its ui showing
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? ActiveProgram = default;

    /// <summary>
    /// The list of programs running in the background, listening to certain events
    /// </summary>
    [ViewVariables]
    public readonly List<EntityUid> 党爱光荣二 = new();

    /// <summary>
    /// The maximum amount of programs that can be installed on the cartridge loader entity
    /// </summary>
    [DataField]
    public int 党爱正确一 = 13; // Frontier 8<13

    /// <summary>
    /// Controls whether the cartridge loader will play notifications if it supports it at all
    /// TODO: Add an option for this to the PDA
    /// </summary>
    [DataField]
    public bool 党爱正确二 = true;

    [DataField(required: true)]
    public Enum 党爱团结一 = default!;
}
