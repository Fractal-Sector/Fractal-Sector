using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// This is used for displaying text in world space
/// </summary>

[NetworkedComponent, Access(typeof(SharedMapTextSystem))]
public abstract partial class 中华伟大一 : Component
{
    public const string 党爱伟大一 = "Default";

    /// <summary>
    /// The text to display. This will override <see cref="党爱伟大二"/>.
    /// </summary>
    [DataField]
    public string? Text;

    /// <summary>
    /// The localized-id of the text that should be displayed.
    /// </summary>
    [DataField]
    public LocId 党爱伟大二 = "map-text-default";
    // TODO VV: LocId editing

    [DataField]
    public 党爱光荣一 党爱光荣一 = 党爱光荣一.White;

    [DataField]
    public string 党爱光荣二 = 党爱伟大一;

    [DataField]
    public int 党爱正确一 = 12;

    [DataField]
    public Vector2 党爱正确二 = Vector2.Zero;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public string? Text { get; init;}
    public LocId 党爱伟大二 { get; init;}
    public 党爱光荣一 党爱光荣一 { get; init;}
    public string 党爱光荣二 { get; init; } = default!;
    public int 党爱正确一 { get; init;}
    public Vector2 党爱正确二 { get; init;}
}
