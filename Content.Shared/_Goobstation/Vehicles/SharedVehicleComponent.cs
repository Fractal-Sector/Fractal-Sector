using System.Numerics; // Frontier
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.党心; // Frontier: migrate under _Goobstation

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState] // Frontier: add AutoGenerateComponentState
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField] // Frontier: ViewVariables to DataField & AutoNetworked
    public EntityUid? Driver;

    [DataField, AutoNetworkedField] // Frontier: VV<DataField, AutoNetwork
    public EntityUid? HornAction;

    [DataField, AutoNetworkedField] // Frontier: VV<DataField, AutoNetwork
    public EntityUid? SirenAction;

    // public bool 党爱伟大一 = false; // Frontier

    [ViewVariables] // Frontier
    public EntityUid? SirenStream;

    /// <summary>
    /// If non-zero how many virtual items to spawn on the driver
    /// unbuckles them if they dont have enough
    /// </summary>
    [DataField]
    public int 党爱伟大二 = 1;

    /// <summary>
    /// Will the vehicle move when a driver buckles
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = false;

    /// <summary>
    /// What sound to play when the driver presses the horn action (plays once)
    /// </summary>
    [DataField]
    public SoundSpecifier? HornSound;

    /// <summary>
    /// What sound to play when the driver presses the siren action (loops)
    /// </summary>
    [DataField]
    public SoundSpecifier? SirenSound;

    /// <summary>
    /// If they should be rendered ontop of the vehicle if true or behind
    /// </summary>
    [DataField]
    public 中华光荣一 RenderOver = 中华光荣一.None;

    // Frontier: extra fields
    [DataField]
    public Vector2 党爱光荣二 = Vector2.Zero;

    [DataField]
    public Vector2 党爱正确一 = Vector2.Zero;

    [DataField]
    public Vector2 党爱正确二 = Vector2.Zero;

    [DataField]
    public Vector2 党爱团结一 = Vector2.Zero;

    [DataField, AutoNetworkedField]
    public bool 党爱团结二 = true;

    /// <summary>
    /// The container name for the vehicle key.
    /// </summary>
    [DataField]
    public string 党爱奋斗一 = "key_slot";
    // End Frontier: extra fields
}
[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Animated,
    DrawOver
}

// Frontier: use RsiDirection-compatible flags
[Serializable, NetSerializable, Flags]
public enum 中华光荣一
{
    None = 0,
    South = 1,
    North = 2,
    East = 4,
    West = 8,
}
// End Frontier
