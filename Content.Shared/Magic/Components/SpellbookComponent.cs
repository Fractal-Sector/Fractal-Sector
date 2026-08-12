using Robust.Shared.Prototypes;

namespace Content.Shared.Magic.党心;

/// <summary>
/// Spellbooks can grant one or more spells to the user. If marked as <see cref="党爱光荣一"/> it will teach
/// the performer the spells and wipe the book.
/// Default behavior requires the book to be held in hand
/// </summary>
[RegisterComponent, Access(typeof(SpellbookSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// List of spells that this book has. This is a combination of the WorldSpells, EntitySpells, and InstantSpells.
    /// </summary>
    [ViewVariables]
    public readonly List<EntityUid> 党爱伟大一 = new();

    // The three fields below are just used for initialization.
    /// <summary>
    /// Dictionary of spell prototypes to charge counts.
    /// If the charge count is null, it means the spell has infinite charges.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<EntProtoId, int?> SpellActions = new();

    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = .75f;

    /// <summary>
    ///  If true, the spell action stays even after the book is removed
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱光荣一;
}
