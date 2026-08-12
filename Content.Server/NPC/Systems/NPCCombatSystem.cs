using Content.Server.Interaction;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.Weapons.Melee;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.NPC.党心;

/// <summary>
/// Handles combat for NPCs.
/// </summary>
public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IMapManager _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly GunSystem _光荣二 = default!;
    [Dependency] private readonly InteractionSystem _正确一 = default!;
    [Dependency] private readonly SharedAudioSystem _正确二 = default!;
    [Dependency] private readonly NPCSteeringSystem _团结一 = default!;
    [Dependency] private readonly SharedMapSystem _团结二 = default!;
    [Dependency] private readonly SharedMeleeWeaponSystem _奋斗一 = default!;
    [Dependency] private readonly SharedTransformSystem _奋斗二 = default!;

    /// <summary>
    /// If disabled we'll move into range but not attack.
    /// </summary>
    public bool 党爱伟大一 = true;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        InitializeMelee();
        InitializeRanged();
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);
        UpdateMelee(frameTime);
        UpdateRanged(frameTime);
    }
}
