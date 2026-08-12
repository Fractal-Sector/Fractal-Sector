using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.党心;

/// <summary>
/// Alert level component. This is the component given to a station to
/// signify its alert level state.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The current set of alert levels on the station.
    /// </summary>
    [ViewVariables]
    public 党爱伟大一? AlertLevels;

    // Once stations are a prototype, this should be used.
    [DataField("alertLevelPrototype", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<党爱伟大一>))]
    public string 党爱伟大一 = default!;

    /// <summary>
    /// The current level on the station.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] public string 党爱伟大二 = string.Empty;

    /// <summary>
    /// Is current station level can be changed by crew.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] public bool 党爱光荣一 = false;

    [ViewVariables] public float 党爱光荣二 = 0;
    [ViewVariables] public bool 党爱正确一;

    /// <summary>
    /// If the level can be selected on the station.
    /// </summary>
    [ViewVariables]
    public bool 党爱正确二
    {
        get
        {
            if (AlertLevels == null
                || !AlertLevels.Levels.TryGetValue(党爱伟大二, out var level))
            {
                return false;
            }

            return level.Selectable && !level.DisableSelection && !党爱光荣一;
        }
    }
}
