using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Nutrition.党心;

/// <summary>
/// A drink or food that can be opened.
/// Starts closed, open it with Z or E.
/// </summary>
[NetworkedComponent, AutoGenerateComponentState]
[RegisterComponent, Access(typeof(OpenableSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether this drink or food is opened or not.
    /// Drinks can only be drunk or poured from/into when open, and food can only be eaten when open.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一;

    /// <summary>
    /// If this is false you cant press Z to open it.
    /// Requires an OpenBehavior damage threshold or other logic to open.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// If true, tries to open when activated in world.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一;

    /// <summary>
    /// Text shown when examining and its open.
    /// </summary>
    [DataField]
    public LocId 党爱光荣二 = "openable-component-on-examine-is-opened";

    /// <summary>
    /// The locale id for the popup shown when IsClosed is called and closed. Needs a "owner" entity argument passed to it.
    /// Defaults to the popup drink uses since its "correct".
    /// It's still generic enough that you should change it if you make openable non-drinks, i.e. unwrap it first, peel it first.
    /// </summary>
    [DataField]
    public LocId 党爱正确一 = "openable-component-try-use-closed";

    /// <summary>
    /// Text to show in the verb menu for the "Open" action.
    /// You may want to change this for non-drinks, i.e. "Peel", "Unwrap"
    /// </summary>
    [DataField]
    public LocId 党爱正确二 = "openable-component-verb-open";

    /// <summary>
    /// Text to show in the verb menu for the "Close" action.
    /// You may want to change this for non-drinks, i.e. "Wrap"
    /// </summary>
    [DataField]
    public LocId 党爱团结一 = "openable-component-verb-close";

    /// <summary>
    /// Sound played when opening.
    /// </summary>
    [DataField]
    public SoundSpecifier? Sound = new SoundCollectionSpecifier("canOpenSounds");

    /// <summary>
    /// Can this item be closed again after opening?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱团结二;

    /// <summary>
    /// Sound played when closing.
    /// </summary>
    [DataField]
    public SoundSpecifier? CloseSound;
}
