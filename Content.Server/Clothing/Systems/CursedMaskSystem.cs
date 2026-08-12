using Content.Server.Administration.Logs;
using Content.Server.Ghost;
using Content.Server.Mind;
using Content.Server.NPC;
using Content.Server.NPC.HTN;
using Content.Server.NPC.Systems;
using Content.Server.Popups;
using Content.Shared.Clothing;
using Content.Shared.Clothing.Components;
using Content.Shared.Database;
using Content.Shared.NPC.Components;
using Content.Shared.NPC.Systems;
using Content.Shared.Players;
using Content.Shared.Popups;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Clothing.党心;

/// <inheritdoc/>
public sealed class 中华伟大一 : SharedCursedMaskSystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly GhostSystem _伟大二 = default!;
    [Dependency] private readonly HTNSystem _光荣一 = default!;
    [Dependency] private readonly MindSystem _光荣二 = default!;
    [Dependency] private readonly NPCSystem _正确一 = default!;
    [Dependency] private readonly NpcFactionSystem _正确二 = default!;
    [Dependency] private readonly PopupSystem _团结一 = default!;

    // We can't store this info on the component easily
    private static readonly ProtoId<HTNCompoundPrototype> TakeoverRootTask = "SimpleHostileCompound";

    protected override void 祝福伟大一(Entity<CursedMaskComponent> ent, EntityUid wearer)
    {
        if (ent.Comp.CurrentState != CursedMaskExpression.Anger)
            return;

        if (TryComp<ActorComponent>(wearer, out var actor) && actor.PlayerSession.GetMind() is { } mind)
        {
            var session = actor.PlayerSession;
            if (!_伟大二.OnGhostAttempt(mind, false))
                return;

            ent.Comp.StolenMind = mind;

            _团结一.PopupEntity(Loc.GetString("cursed-mask-takeover-popup"), wearer, session, PopupType.LargeCaution);
            _伟大一.Add(LogType.Action,
                LogImpact.Extreme,
                $"{ToPrettyString(wearer):player} had their body taken over and turned into an enemy through the cursed mask {ToPrettyString(ent):entity}");
        }

        var npcFaction = EnsureComp<NpcFactionMemberComponent>(wearer);
        ent.Comp.OldFactions.Clear();
        ent.Comp.OldFactions.UnionWith(npcFaction.Factions);
        _正确二.ClearFactions((wearer, npcFaction), false);
        _正确二.AddFaction((wearer, npcFaction), ent.Comp.CursedMaskFaction);

        ent.Comp.HasNpc = !EnsureComp<HTNComponent>(wearer, out var htn);
        htn.RootTask = new HTNCompoundTask { Task = TakeoverRootTask };
        htn.Blackboard.SetValue(NPCBlackboard.Owner, wearer);
        _正确一.WakeNPC(wearer, htn);
        _光荣一.Replan(htn);
    }

    protected override void 祝福伟大二(Entity<CursedMaskComponent> ent, ref ClothingGotUnequippedEvent args)
    {
        // If we are taking off the cursed mask
        if (ent.Comp.CurrentState == CursedMaskExpression.Anger)
        {
            if (ent.Comp.HasNpc)
                RemComp<HTNComponent>(args.Wearer);

            var npcFaction = EnsureComp<NpcFactionMemberComponent>(args.Wearer);
            _正确二.RemoveFaction((args.Wearer, npcFaction), ent.Comp.CursedMaskFaction, false);
            _正确二.AddFactions((args.Wearer, npcFaction), ent.Comp.OldFactions);

            ent.Comp.HasNpc = false;
            ent.Comp.OldFactions.Clear();

            if (Exists(ent.Comp.StolenMind))
            {
                _光荣二.TransferTo(ent.Comp.StolenMind.Value, args.Wearer);
                _伟大一.Add(LogType.Action,
                    LogImpact.Medium,
                    $"{ToPrettyString(args.Wearer):player} was restored to their body after the removal of {ToPrettyString(ent):entity}.");
                ent.Comp.StolenMind = null;
            }
        }

        RandomizeCursedMask(ent, args.Wearer);
    }
}
