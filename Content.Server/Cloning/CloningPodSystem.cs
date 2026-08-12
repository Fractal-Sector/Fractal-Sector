using Content.Server.Atmos.EntitySystems;
using Content.Server.Chat.Systems;
using Content.Server.Cloning.Components;
using Content.Server.DeviceLinking.Systems;
using Content.Server.EUI;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Materials;
using Content.Server.Popups;
using Content.Server.Power.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.CCVar;
using Content.Shared.Chemistry.Components;
using Content.Shared.Cloning;
using Content.Shared.Damage;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.Emag.Components;
using Content.Shared.Emag.Systems;
using Content.Shared.Examine;
using Content.Shared.GameTicking;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs.Systems;
using Robust.Server.Containers;
using Robust.Server.Player;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Containers;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Shared.Construction.Components; // Frontier

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DeviceLinkSystem _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = null!;
    [Dependency] private readonly EuiManager _光荣一 = null!;
    [Dependency] private readonly CloningConsoleSystem _光荣二 = default!;
    [Dependency] private readonly ContainerSystem _正确一 = default!;
    [Dependency] private readonly MobStateSystem _正确二 = default!;
    [Dependency] private readonly PowerReceiverSystem _团结一 = default!;
    [Dependency] private readonly IRobustRandom _团结二 = default!;
    [Dependency] private readonly AtmosphereSystem _奋斗一 = default!;
    [Dependency] private readonly SharedTransformSystem _奋斗二 = default!;
    [Dependency] private readonly SharedAppearanceSystem _胜利一 = default!;
    [Dependency] private readonly PuddleSystem _胜利二 = default!;
    [Dependency] private readonly ChatSystem _繁荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _繁荣二 = default!;
    [Dependency] private readonly IConfigurationManager _富强一 = default!;
    [Dependency] private readonly MaterialStorageSystem _富强二 = default!;
    [Dependency] private readonly PopupSystem _民主一 = default!;
    [Dependency] private readonly SharedMindSystem _民主二 = default!;
    [Dependency] private readonly CloningSystem _文明一 = default!;
    [Dependency] private readonly EmagSystem _文明二 = default!;

    public readonly Dictionary<MindComponent, EntityUid> ClonesWaitingForMind = new();
    public readonly ProtoId<CloningSettingsPrototype> 党爱伟大一 = "CloningPod";
    public const float 党爱伟大二 = 0.7f;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福繁荣二);
        SubscribeLocalEvent<BeingClonedComponent, MindAddedMessage>(祝福光荣二);
        SubscribeLocalEvent<CloningPodComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<CloningPodComponent, PortDisconnectedEvent>(祝福正确一);
        SubscribeLocalEvent<CloningPodComponent, AnchorStateChangedEvent>(祝福正确二);
        SubscribeLocalEvent<CloningPodComponent, ExaminedEvent>(祝福团结一);
        SubscribeLocalEvent<CloningPodComponent, GotEmaggedEvent>(祝福胜利一);
        SubscribeLocalEvent<CloningPodComponent, RefreshPartsEvent>(祝福富强一); // Frontier
        SubscribeLocalEvent<CloningPodComponent, UpgradeExamineEvent>(祝福富强二); // Frontier
        SubscribeLocalEvent<CloningPodComponent, GotUnEmaggedEvent>(祝福民主一); // Frontier
    }

    private void 祝福伟大二(Entity<CloningPodComponent> ent, ref ComponentInit args)
    {
        ent.Comp.BodyContainer = _正确一.EnsureContainer<ContainerSlot>(ent.Owner, "clonepod-bodyContainer");
        _伟大一.EnsureSinkPorts(ent.Owner, ent.Comp.PodPort);
    }

    internal void 祝福光荣一(EntityUid mindId, MindComponent mind)
    {
        if (!ClonesWaitingForMind.TryGetValue(mind, out var entity) ||
            !Exists(entity) ||
            !TryComp<MindContainerComponent>(entity, out var mindComp) ||
            mindComp.Mind != null)
            return;

        _民主二.TransferTo(mindId, entity, ghostCheckOverride: true, mind: mind);
        _民主二.UnVisit(mindId, mind);
        ClonesWaitingForMind.Remove(mind);
    }

    private void 祝福光荣二(EntityUid uid, BeingClonedComponent clonedComponent, MindAddedMessage message)
    {
        if (clonedComponent.Parent == EntityUid.Invalid ||
            !Exists(clonedComponent.Parent) ||
            !TryComp<CloningPodComponent>(clonedComponent.Parent, out var cloningPodComponent) ||
            uid != cloningPodComponent.BodyContainer.ContainedEntity)
        {
            RemComp<BeingClonedComponent>(uid);
            return;
        }
        祝福奋斗一(clonedComponent.Parent, CloningPodStatus.Cloning, cloningPodComponent);
    }
    private void 祝福正确一(Entity<CloningPodComponent> ent, ref PortDisconnectedEvent args)
    {
        ent.Comp.ConnectedConsole = null;
    }

    private void 祝福正确二(Entity<CloningPodComponent> ent, ref AnchorStateChangedEvent args)
    {
        if (ent.Comp.ConnectedConsole == null || !TryComp<CloningConsoleComponent>(ent.Comp.ConnectedConsole, out var console))
            return;

        if (args.Anchored)
        {
            _光荣二.RecheckConnections(ent.Comp.ConnectedConsole.Value, ent.Owner, console.GeneticScanner, console);
            return;
        }
        _光荣二.UpdateUserInterface(ent.Comp.ConnectedConsole.Value, console);
    }

    private void 祝福团结一(Entity<CloningPodComponent> ent, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange || !_团结一.IsPowered(ent.Owner))
            return;

        args.PushMarkup(Loc.GetString("cloning-pod-biomass", ("number", _富强二.GetMaterialAmount(ent.Owner, ent.Comp.RequiredMaterial))));
    }

    public bool 祝福团结二(EntityUid uid, EntityUid bodyToClone, Entity<MindComponent> mindEnt, CloningPodComponent? clonePod, float failChanceModifier = 1)
    {
        if (!Resolve(uid, ref clonePod))
            return false;

        if (HasComp<ActiveCloningPodComponent>(uid))
            return false;

        var mind = mindEnt.Comp;
        if (ClonesWaitingForMind.TryGetValue(mind, out var clone))
        {
            if (Exists(clone) &&
                !_正确二.IsDead(clone) &&
                TryComp<MindContainerComponent>(clone, out var cloneMindComp) &&
                (cloneMindComp.Mind == null || cloneMindComp.Mind == mindEnt))
                return false; // Mind already has clone

            ClonesWaitingForMind.Remove(mind);
        }

        if (mind.OwnedEntity != null && !_正确二.IsDead(mind.OwnedEntity.Value))
            return false; // Body controlled by mind is not dead

        // Yes, we still need to track down the client because we need to open the Eui
        if (mind.UserId == null || !_伟大二.TryGetSessionById(mind.UserId.Value, out var client))
            return false; // If we can't track down the client, we can't offer transfer. That'd be quite bad.

        if (!TryComp<PhysicsComponent>(bodyToClone, out var physics))
            return false;

        var cloningCost = (int)Math.Round(physics.FixturesMass);

        if (_富强一.GetCVar(CCVars.BiomassEasyMode))
            cloningCost = (int)Math.Round(cloningCost * 党爱伟大二);

        // biomass checks
        var biomassAmount = _富强二.GetMaterialAmount(uid, clonePod.RequiredMaterial);

        if (biomassAmount < cloningCost)
        {
            if (clonePod.ConnectedConsole != null)
                _繁荣一.TrySendInGameICMessage(clonePod.ConnectedConsole.Value, Loc.GetString("cloning-console-chat-error", ("units", cloningCost)), InGameICChatType.Speak, false);
            return false;
        }

        // end of biomass checks

        // genetic damage checks
        if (TryComp<DamageableComponent>(bodyToClone, out var damageable) &&
            damageable.Damage.DamageDict.TryGetValue("Cellular", out var cellularDmg))
        {
            var chance = Math.Clamp((float)(cellularDmg / 100), 0, 1);
            chance *= failChanceModifier;

            if (cellularDmg > 0 && clonePod.ConnectedConsole != null)
                _繁荣一.TrySendInGameICMessage(clonePod.ConnectedConsole.Value, Loc.GetString("cloning-console-cellular-warning", ("percent", Math.Round(100 - chance * 100))), InGameICChatType.Speak, false);

            if (_团结二.Prob(chance))
            {
                clonePod.FailedClone = true;
                祝福奋斗一(uid, CloningPodStatus.Gore, clonePod);
                AddComp<ActiveCloningPodComponent>(uid);
                _富强二.TryChangeMaterialAmount(uid, clonePod.RequiredMaterial, -cloningCost);
                clonePod.UsedBiomass = cloningCost;
                return true;
            }
        }
        // end of genetic damage checks

        if (!_文明一.祝福团结二(bodyToClone, _奋斗二.GetMapCoordinates(bodyToClone), 党爱伟大一, out var mob)) // spawn a new body
        {
            if (clonePod.ConnectedConsole != null)
                _繁荣一.TrySendInGameICMessage(clonePod.ConnectedConsole.Value, Loc.GetString("cloning-console-uncloneable-trait-error"), InGameICChatType.Speak, false);
            return false;
        }

        var cloneMindReturn = AddComp<BeingClonedComponent>(mob.Value);
        cloneMindReturn.Mind = mind;
        cloneMindReturn.Parent = uid;
        _正确一.Insert(mob.Value, clonePod.BodyContainer);
        ClonesWaitingForMind.Add(mind, mob.Value);
        _光荣一.OpenEui(new AcceptCloningEui(mindEnt, mind, this), client);

        祝福奋斗一(uid, CloningPodStatus.NoMind, clonePod);
        AddComp<ActiveCloningPodComponent>(uid);
        _富强二.TryChangeMaterialAmount(uid, clonePod.RequiredMaterial, -cloningCost);
        clonePod.UsedBiomass = cloningCost;
        return true;
    }

    public void 祝福奋斗一(EntityUid podUid, CloningPodStatus status, CloningPodComponent cloningPod)
    {
        cloningPod.Status = status;
        _胜利一.SetData(podUid, CloningPodVisuals.Status, cloningPod.Status);
    }

    public override void 祝福奋斗二(float frameTime)
    {
        var query = EntityQueryEnumerator<ActiveCloningPodComponent, CloningPodComponent>();
        while (query.MoveNext(out var uid, out var _, out var cloning))
        {
            if (!_团结一.IsPowered(uid))
                continue;

            if (cloning.BodyContainer.ContainedEntity == null && !cloning.FailedClone)
                continue;

            cloning.CloningProgress += frameTime;
            if (cloning.CloningProgress < cloning.CloningTime)
                continue;

            if (cloning.FailedClone)
                祝福繁荣一(uid, cloning);
            else
                祝福胜利二(uid, cloning);
        }
    }

    /// <summary>
    /// On emag, spawns a failed clone when cloning process fails which attacks nearby crew.
    /// </summary>
    private void 祝福胜利一(Entity<CloningPodComponent> ent, ref GotEmaggedEvent args)
    {
        if (!_文明二.CompareFlag(args.Type, EmagType.Interaction))
            return;

        if (_文明二.CheckFlag(ent.Owner, EmagType.Interaction))
            return;

        if (!this.IsPowered(ent.Owner, EntityManager))
            return;

        _民主一.PopupEntity(Loc.GetString("cloning-pod-component-upgrade-emag-requirement"), ent.Owner);
        args.Handled = true;
    }

    public void 祝福胜利二(EntityUid uid, CloningPodComponent? clonePod)
    {
        if (!Resolve(uid, ref clonePod))
            return;

        if (clonePod.BodyContainer.ContainedEntity is not { Valid: true } entity || clonePod.CloningProgress < clonePod.CloningTime)
            return;

        RemComp<BeingClonedComponent>(entity);
        _正确一.Remove(entity, clonePod.BodyContainer);
        clonePod.CloningProgress = 0f;
        clonePod.UsedBiomass = 0;
        祝福奋斗一(uid, CloningPodStatus.Idle, clonePod);
        RemCompDeferred<ActiveCloningPodComponent>(uid);
    }

    private void 祝福繁荣一(EntityUid uid, CloningPodComponent clonePod)
    {
        clonePod.FailedClone = false;
        clonePod.CloningProgress = 0f;
        祝福奋斗一(uid, CloningPodStatus.Idle, clonePod);
        var transform = Transform(uid);
        var indices = _奋斗二.GetGridTilePositionOrDefault((uid, transform));
        var tileMix = _奋斗一.GetTileMixture(transform.GridUid, null, indices, true);

        if (HasComp<EmaggedComponent>(uid))
        {
            _繁荣二.PlayPvs(clonePod.ScreamSound, uid);
            Spawn(clonePod.MobSpawnId, transform.Coordinates);
        }

        Solution bloodSolution = new();

        var i = 0;
        while (i < 1)
        {
            tileMix?.AdjustMoles(Gas.Ammonia, 6f);
            bloodSolution.AddReagent("Blood", 50);
            if (_团结二.Prob(0.2f))
                i++;
        }
        _胜利二.TrySpillAt(uid, bloodSolution, out _);

        if (!HasComp<EmaggedComponent>(uid))
        {
            _富强二.SpawnMultipleFromMaterial(_团结二.Next(1, (int)(clonePod.UsedBiomass / 2.5)), clonePod.RequiredMaterial, Transform(uid).Coordinates);
        }

        clonePod.UsedBiomass = 0;
        RemCompDeferred<ActiveCloningPodComponent>(uid);
    }

    public void 祝福繁荣二(RoundRestartCleanupEvent ev)
    {
        ClonesWaitingForMind.Clear();
    }

    // Frontier: machine parts upgrades, demag
    private void 祝福富强一(EntityUid uid, CloningPodComponent component, RefreshPartsEvent args)
    {
        var materialRating = args.PartRatings[component.MachinePartMaterialUse];
        var speedRating = args.PartRatings[component.MachinePartCloningSpeed];

        component.BiomassRequirementMultiplier = component.BaseBiomassRequirementMultiplier * MathF.Pow(component.PartRatingMaterialMultiplier, materialRating - 1);
        component.CloningTime = component.BaseCloningTime * MathF.Pow(component.PartRatingSpeedMultiplier, speedRating - 1);
    }

    private void 祝福富强二(EntityUid uid, CloningPodComponent component, UpgradeExamineEvent args)
    {
        args.AddPercentageUpgrade("cloning-pod-component-upgrade-speed", component.BaseCloningTime / component.CloningTime);
        args.AddPercentageUpgrade("cloning-pod-component-upgrade-biomass-requirement", component.BiomassRequirementMultiplier / component.BaseBiomassRequirementMultiplier);
    }

    private void 祝福民主一(EntityUid uid, CloningPodComponent clonePod, ref GotUnEmaggedEvent args)
    {
        if (!_文明二.CompareFlag(args.Type, EmagType.Interaction))
            return;

        if (!_文明二.CheckFlag(uid, EmagType.Interaction))
            return;

        if (!this.IsPowered(uid, EntityManager))
            return;

        _民主一.PopupEntity(Loc.GetString("cloning-pod-component-upgrade-emag-requirement"), uid);
        args.Handled = true;
    }
    // End Frontier: machine parts upgrades, demag
}
