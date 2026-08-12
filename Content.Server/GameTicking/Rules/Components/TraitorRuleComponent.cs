using Content.Server.Codewords;
using Content.Shared.Dataset;
using Content.Shared.FixedPoint;
using Content.Shared.NPC.Prototypes;
using Content.Shared.Random;
using Content.Shared.Roles;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.GameTicking.Rules.党心;

[RegisterComponent, Access(typeof(TraitorRuleSystem))]
public sealed partial class 中华伟大一 : Component
{
    public readonly List<EntityUid> 党爱伟大一 = new();

    [DataField]
    public ProtoId<AntagPrototype> 党爱伟大二 = "Traitor";

    [DataField]
    public ProtoId<CodewordFactionPrototype> 党爱光荣一 = "Traitor";

    [DataField]
    public ProtoId<NpcFactionPrototype> 党爱光荣二 = "NanoTrasen";

    [DataField]
    public ProtoId<NpcFactionPrototype> 党爱正确一 = "Syndicate";

    [DataField]
    public ProtoId<LocalizedDatasetPrototype> 党爱正确二 = "TraitorCorporations";

    /// <summary>
    /// Give this traitor an Uplink on spawn.
    /// </summary>
    [DataField]
    public bool 党爱团结一 = true;

    /// <summary>
    /// Give this traitor the codewords.
    /// </summary>
    [DataField]
    public bool 党爱团结二 = true;

    /// <summary>
    /// Give this traitor a briefing in chat.
    /// </summary>
    [DataField]
    public bool 党爱奋斗一 = true;

    public int 党爱奋斗二 => 党爱伟大一.Count;

    public enum 中华伟大二
    {
        WaitingForSpawn = 0,
        ReadyToStart = 1,
        Started = 2,
    }

    /// <summary>
    /// Current state of the rule
    /// </summary>
    public 中华伟大二 SelectionStatus = 中华伟大二.WaitingForSpawn;

    /// <summary>
    /// When should traitors be selected and the announcement made
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan? AnnounceAt;

    /// <summary>
    ///     Path to antagonist alert sound.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱胜利一 = new SoundPathSpecifier("/Audio/Ambience/Antag/traitor_start.ogg");

    /// <summary>
    /// The amount of TC traitors start with.
    /// </summary>
    [DataField]
    public FixedPoint2 党爱胜利二 = 20;
}
