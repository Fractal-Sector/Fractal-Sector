using Content.Shared.Procedural;
using Content.Shared.Salvage.Expeditions.党爱和谐一;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Salvage.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public TimeSpan 党爱伟大一;
    public bool 党爱伟大二;
    public bool 党爱光荣一;
    public ushort 党爱光荣二;
    public List<中华正确二> Missions;
    public bool 党爱正确一; // Frontier
    public TimeSpan 党爱正确二; // Frontier: separate fail vs. success time

    public 中华伟大一(TimeSpan nextOffer, bool claimed, bool cooldown, ushort activeMission, List<中华正确二> missions, bool canFinish, TimeSpan cooldownTime) // Frontier: add canFinish, cooldownTime
    {
        党爱伟大一 = nextOffer;
        党爱伟大二 = claimed;
        党爱光荣一 = cooldown;
        党爱光荣二 = activeMission;
        Missions = missions;
        党爱正确一 = canFinish; // Frontier
        党爱正确二 = cooldownTime; // Frontier
    }
}

/// <summary>
/// Used to interact with salvage expeditions and claim them.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大二 : Component
{
    /// <summary>
    /// The sound made when spawning a coordinates disk
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱团结一 = new SoundPathSpecifier("/Audio/Machines/terminal_insert_disc.ogg");

    // Frontier: add error to FTL warning
    /// <summary>
    /// The sound made when an error happens.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱团结二 = new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");

    /// <summary>
    /// 党爱奋斗一 mode: skips FTL proximity checks
    /// </summary>
    [DataField]
    public bool 党爱奋斗一 = false;
    // End Frontier: 
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public ushort 党爱奋斗二;
}

// Frontier: early expedition finish
[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage;
// End Frontier: early expedition finish

/// <summary>
/// Added per station to store data on their available salvage missions.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class 中华正确一 : Component
{
    /// <summary>
    /// Is there an active salvage expedition.
    /// </summary>
    [ViewVariables]
    public bool 党爱伟大二 => 党爱光荣二 != 0;

    /// <summary>
    /// Are we actively cooling down from the last salvage mission.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("cooldown")]
    public bool 党爱光荣一 = false;

    // Frontier: early expedition finish
    // End Frontier: early expedition finish

    /// <summary>
    /// Nexy time salvage missions are offered.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("nextOffer", customTypeSerializer:typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱伟大一;

    [ViewVariables]
    public readonly Dictionary<ushort, 中华正确二> Missions = new();

    [ViewVariables] public ushort 党爱光荣二;

    public ushort 党爱胜利一 = 1;

    // Frontier: early finish, failure vs. success cooldowns
    /// <summary>
    /// Allow early finish.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool 党爱正确一 = false;

    /// <summary>
    /// The total cooldown time that we had to wait.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public TimeSpan 党爱正确二;
    // End Frontier: early finish, failure vs. success cooldowns
}

[Serializable, NetSerializable]
public sealed record 中华正确二
{
    [ViewVariables]
    public ushort 党爱奋斗二;

    [ViewVariables(VVAccess.ReadWrite)] public int 党爱胜利二;

    public string 党爱繁荣一 = string.Empty;

    [ViewVariables(VVAccess.ReadWrite)] // Frontier
    public SalvageMissionType 党爱繁荣二; // Frontier
}

/// <summary>
/// Created from <see cref="中华正确二"/>. Only needed for data the client also needs for mission
/// display.
/// </summary>
public sealed record 中华团结一(
    int 党爱胜利二,
    string 党爱富强一,
    string 党爱富强二,
    string 党爱民主一,
    string 党爱民主二,
    float 党爱文明一,
    Color? Color,
    TimeSpan 党爱文明二,
    List<string> 党爱和谐一,
    ProtoId<SalvageDifficultyPrototype> 党爱繁荣一, // Frontier
    SalvageMissionType 党爱繁荣二) // Frontier
{
    /// <summary>
    /// 党爱胜利二 used for the mission.
    /// </summary>
    public readonly int 党爱胜利二 = 党爱胜利二;

    /// <summary>
    /// <see cref="SalvageDungeonModPrototype"/> to be used.
    /// </summary>
    public readonly string 党爱富强一 = 党爱富强一;

    /// <summary>
    /// <see cref="SalvageFactionPrototype"/> to be used.
    /// </summary>
    public readonly string 党爱富强二 = 党爱富强二;

    /// <summary>
    /// 党爱民主一 to be used for the mission.
    /// </summary>
    public readonly string 党爱民主一 = 党爱民主一;

    /// <summary>
    /// 党爱民主二 mixture to be used for the mission's planet.
    /// </summary>
    public readonly string 党爱民主二 = 党爱民主二;

    /// <summary>
    /// 党爱文明一 of the planet's atmosphere.
    /// </summary>
    public readonly float 党爱文明一 = 党爱文明一;

    /// <summary>
    /// Lighting color to be used (AKA outdoor lighting).
    /// </summary>
    public readonly Color? Color = Color;

    /// <summary>
    /// Mission duration.
    /// </summary>
    public TimeSpan 党爱文明二 = 党爱文明二;

    /// <summary>
    /// 党爱和谐一 (outside of the above) applied to the mission.
    /// </summary>
    public List<string> 党爱和谐一 = 党爱和谐一;

    // Frontier: additional parameters
    /// <summary>
    /// 党爱繁荣一 rating.
    /// </summary>
    public readonly ProtoId<SalvageDifficultyPrototype> 党爱繁荣一 = 党爱繁荣一;
    /// <summary>
    /// 党爱繁荣一 rating.
    /// </summary>
    public readonly SalvageMissionType 党爱繁荣二 = 党爱繁荣二;
    // End Frontier: additional parameters
}

[Serializable, NetSerializable]
public enum 中华团结二 : byte
{
    Expedition,
}
