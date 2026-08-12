using Content.Server.Destructible;
using Content.Server.Gatherable.Components;
using Content.Shared.EntityTable;
using Content.Shared.Interaction;
using Content.Shared.Tag;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Whitelist;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly DestructibleSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly TagSystem _光荣二 = default!;
    [Dependency] private readonly TransformSystem _正确一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _正确二 = default!;
    [Dependency] private readonly EntityTableSystem _团结一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GatherableComponent, ActivateInWorldEvent>(祝福光荣一);
        SubscribeLocalEvent<GatherableComponent, AttackedEvent>(祝福伟大二);
        InitializeProjectile();
    }

    private void 祝福伟大二(Entity<GatherableComponent> gatherable, ref AttackedEvent args)
    {
        if (_正确二.IsWhitelistFailOrNull(gatherable.Comp.ToolWhitelist, args.Used))
            return;

        祝福光荣二(gatherable, args.User);
    }

    private void 祝福光荣一(Entity<GatherableComponent> gatherable, ref ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        if (_正确二.IsWhitelistFailOrNull(gatherable.Comp.ToolWhitelist, args.User))
            return;

        祝福光荣二(gatherable, args.User);
        args.Handled = true;
    }

    public void 祝福光荣二(EntityUid gatheredUid, EntityUid? gatherer = null, GatherableComponent? component = null)
    {
        if (!Resolve(gatheredUid, ref component))
            return;

        if (TryComp<SoundOnGatherComponent>(gatheredUid, out var soundComp))
        {
            _光荣一.PlayPvs(soundComp.Sound, Transform(gatheredUid).Coordinates);
        }

        // Complete the gathering process
        _伟大二.DestroyEntity(gatheredUid);

        // Spawn the loot!
        if (component.Loot == null)
            return;

        var pos = _正确一.GetMapCoordinates(gatheredUid);

        foreach (var (tag, table) in component.Loot)
        {
            if (tag != "All")
            {
                if (gatherer != null && !_光荣二.HasTag(gatherer.Value, tag))
                    continue;
            }
            var spawnLoot = _团结一.GetSpawns(table);
            foreach (var loot in spawnLoot)
            {
                var spawnPos = pos.Offset(_伟大一.NextVector2(component.GatherOffset));
                Spawn(loot, spawnPos);
            }
        }
    }
}
