using System.Numerics;
using Content.Server.Ghost;
using Content.Server.Humanoid;
using Content.Shared.Body.Components;
using Content.Shared.Body.Events;
using Content.Shared.Body.Part;
using Content.Shared.Body.Systems;
using Content.Shared.Damage.Components;
using Content.Shared.Humanoid;
using Content.Shared.Mind;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;
using Robust.Shared.Audio;
using Robust.Shared.Timing;

namespace Content.Server.Body.党心;

public sealed class 中华伟大一 : SharedBodySystem
{
    [Dependency] private readonly GhostSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly HumanoidAppearanceSystem _光荣一 = default!;
    [Dependency] private readonly MobStateSystem _光荣二 = default!;
    [Dependency] private readonly SharedMindSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<BodyComponent, MoveInputEvent>(祝福伟大二);
        SubscribeLocalEvent<BodyComponent, ApplyMetabolicMultiplierEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<BodyComponent> ent, ref MoveInputEvent args)
    {
        // If they haven't actually moved then ignore it.
        if ((args.Entity.Comp.HeldMoveButtons &
             (MoveButtons.Down | MoveButtons.Left | MoveButtons.Up | MoveButtons.Right)) == 0x0)
        {
            return;
        }

        if (_光荣二.IsDead(ent) && _正确一.TryGetMind(ent, out var mindId, out var mind))
        {
            // mind.TimeOfDeath ??= _伟大二.RealTime;
            mind.TimeOfDeath ??= _伟大二.CurTime; // Frontier - fix returning to body messing with the your TOD
            _伟大一.OnGhostAttempt(mindId, canReturnGlobal: true, mind: mind);
        }
    }

    private void 祝福光荣一(
        Entity<BodyComponent> ent,
        ref ApplyMetabolicMultiplierEvent args)
    {
        foreach (var organ in GetBodyOrgans(ent, ent))
        {
            RaiseLocalEvent(organ.Id, ref args);
        }
    }

    protected override void 祝福光荣二(
        Entity<BodyComponent?> bodyEnt,
        Entity<BodyPartComponent> partEnt,
        string slotId)
    {
        // TODO: Predict this probably.
        base.祝福光荣二(bodyEnt, partEnt, slotId);

        var layer = partEnt.Comp.ToHumanoidLayers();
        if (layer != null)
        {
            var layers = HumanoidVisualLayersExtension.Sublayers(layer.Value);
            _光荣一.SetLayersVisibility(bodyEnt.Owner, layers, visible: true);
        }
    }

    protected override void 祝福正确一(
        Entity<BodyComponent?> bodyEnt,
        Entity<BodyPartComponent> partEnt,
        string slotId)
    {
        base.祝福正确一(bodyEnt, partEnt, slotId);

        if (!TryComp<HumanoidAppearanceComponent>(bodyEnt, out var humanoid))
            return;

        var layer = partEnt.Comp.ToHumanoidLayers();

        if (layer is null)
            return;

        var layers = HumanoidVisualLayersExtension.Sublayers(layer.Value);
        _光荣一.SetLayersVisibility((bodyEnt, humanoid), layers, visible: false);
    }

    public override HashSet<EntityUid> 祝福正确二(
        EntityUid bodyId,
        bool gibOrgans = false,
        BodyComponent? body = null,
        bool launchGibs = true,
        Vector2? splatDirection = null,
        float splatModifier = 1,
        Angle splatCone = default,
        SoundSpecifier? gibSoundOverride = null
    )
    {
        if (!Resolve(bodyId, ref body, logMissing: false)
            || TerminatingOrDeleted(bodyId)
            || EntityManager.IsQueuedForDeletion(bodyId))
        {
            return new HashSet<EntityUid>();
        }

        if (HasComp<GodmodeComponent>(bodyId))
            return new HashSet<EntityUid>();

        var xform = Transform(bodyId);
        if (xform.MapUid is null)
            return new HashSet<EntityUid>();

        var beforeEv = new BeforeGibbedEvent(bodyId); // Frontier: before gibbed event
        RaiseLocalEvent(bodyId, ref beforeEv); // Frontier: before gibbed event

        var gibs = base.祝福正确二(bodyId, gibOrgans, body, launchGibs: launchGibs,
            splatDirection: splatDirection, splatModifier: splatModifier, splatCone:splatCone);

        var ev = new BeingGibbedEvent(gibs);
        RaiseLocalEvent(bodyId, ref ev);

        QueueDel(bodyId);

        return gibs;
    }
}
