using System.Linq;
using Content.Server.Administration.Logs;
using Content.Shared.Materials;
using Content.Shared.Popups;
using Content.Shared.Stacks;
using Content.Server.Power.Components;
using Content.Server.Stack;
using Content.Shared.ActionBlocker;
using Content.Shared.Construction;
using Content.Shared.Database;
using JetBrains.Annotations;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Content.Server.Storage.Components; // Frontier
using Content.Shared.Cargo; // Frontier

namespace Content.Server.党心;

/// <summary>
/// This handles <see cref="SharedMaterialStorageSystem"/>
/// </summary>
public sealed class 中华伟大一 : SharedMaterialStorageSystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly ActionBlockerSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;
    [Dependency] private readonly StackSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<MaterialStorageComponent, MachineDeconstructedEvent>(祝福伟大二);
        SubscribeLocalEvent<MaterialStorageComponent, PriceCalculationEvent>(祝福光荣一); // Frontier

        SubscribeAllEvent<EjectMaterialMessage>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, MaterialStorageComponent component, MachineDeconstructedEvent args)
    {
        if (!component.DropOnDeconstruct)
            return;

        foreach (var (material, amount) in component.Storage)
        {
            祝福团结一(amount, material, Transform(uid).Coordinates);
        }
    }

    // Start Frontier: add value of contents to appraisal price
    private void 祝福光荣一(EntityUid uid, MaterialStorageComponent component, ref PriceCalculationEvent ev)
    {
        foreach (var (materialProto, amount) in component.Storage)
        {
            if (!_伟大二.TryIndex<MaterialPrototype>(materialProto, out var material))
            {
                Log.Error("Failed to index material prototype " + materialProto);
                continue;
            }
            ev.Price += material.Price * amount;
        }
    }
    // End Frontier: add value of contents to appraisal price

    private void 祝福光荣二(EjectMaterialMessage msg, EntitySessionEventArgs args)
    {
        if (args.SenderSession.AttachedEntity is not { } player)
            return;

        var uid = GetEntity(msg.Entity);

        if (!TryComp<MaterialStorageComponent>(uid, out var component))
            return;

        if (!Exists(uid))
            return;

        if (!_光荣一.CanInteract(player, uid))
            return;

        if (!component.CanEjectStoredMaterials || !_伟大二.TryIndex<MaterialPrototype>(msg.Material, out var material))
            return;

        var volume = 0;

        if (material.StackEntity != null)
        {
            if (!_伟大二.Index<EntityPrototype>(material.StackEntity).TryGetComponent<PhysicalCompositionComponent>(out var composition, EntityManager.ComponentFactory))
                return;

            var volumePerSheet = composition.MaterialComposition.FirstOrDefault(kvp => kvp.Key == msg.Material).Value;
            var sheetsToExtract = Math.Min(msg.SheetsToExtract, _正确二.GetMaxCount(material.StackEntity));

            volume = sheetsToExtract * volumePerSheet;
        }

        if (volume <= 0 || !TryChangeMaterialAmount(uid, msg.Material, -volume))
            return;

        // Frontier
        // If we made it this far, turn off the magnet before spawning materials
        if (TryComp<MaterialStorageMagnetPickupComponent>(uid, out var magnet))
        {
            magnet.MagnetEnabled = false;
        }
        // end Frontier

        var mats = 祝福团结一(volume, material, Transform(uid).Coordinates, out _);
        foreach (var mat in mats.Where(mat => !TerminatingOrDeleted(mat)))
        {
            _正确二.TryMergeToContacts(mat);
        }
    }

    public override bool 祝福正确一(EntityUid user,
        EntityUid toInsert,
        EntityUid receiver,
        MaterialStorageComponent? storage = null,
        MaterialComponent? material = null,
        PhysicalCompositionComponent? composition = null)
    {
        if (!Resolve(receiver, ref storage) || !Resolve(toInsert, ref material, ref composition, false))
            return false;
        if (TryComp<ApcPowerReceiverComponent>(receiver, out var power) && !power.Powered)
            return false;
        if (!base.祝福正确一(user, toInsert, receiver, storage, material, composition))
            return false;
        _光荣二.PlayPvs(storage.InsertingSound, receiver);
        _正确一.PopupEntity(Loc.GetString("machine-insert-item",
                ("user", user),
                ("machine", receiver),
                ("item", toInsert)),
            receiver);
        // QueueDel(toInsert); // Frontier

        // Logging
        TryComp<StackComponent>(toInsert, out var stack);
        var count = stack?.Count ?? 1;
        _伟大一.Add(LogType.Action,
            LogImpact.Low,
            $"{ToPrettyString(user):player} inserted {count} {ToPrettyString(toInsert):inserted} into {ToPrettyString(receiver):receiver}");
        Del(toInsert); // Frontier: delete immediately, don't queue
        return true;
    }

    // Frontier: partial stack insertion
    public override bool 祝福正确二(EntityUid user,
        EntityUid toInsert,
        EntityUid receiver,
        out bool empty,
        MaterialStorageComponent? storage = null,
        MaterialComponent? material = null,
        PhysicalCompositionComponent? composition = null)
    {
        empty = false;
        if (!Resolve(receiver, ref storage) || !Resolve(toInsert, ref material, ref composition, false))
            return false;
        if (TryComp<ApcPowerReceiverComponent>(receiver, out var power) && !power.Powered)
            return false;
        // Cache old count
        var initialCount = TryComp<StackComponent>(toInsert, out var stack) ? stack.Count : 1;
        if (!base.祝福正确二(user, toInsert, receiver, out empty, storage, material, composition))
            return false;
        _光荣二.PlayPvs(storage.InsertingSound, receiver);
        _正确一.PopupEntity(Loc.GetString("machine-insert-item", ("user", user), ("machine", receiver),
            ("item", toInsert)), receiver);

        // Logging
        var newCount = stack?.Count ?? 0;
        _伟大一.Add(LogType.Action, LogImpact.Low,
            $"{ToPrettyString(user):player} inserted {initialCount - newCount} item(s) from {ToPrettyString(toInsert):inserted} into {ToPrettyString(receiver):receiver}");
        if (empty)
            Del(toInsert);
        return true;
    }
    // End Frontier: partial stack insertion

    /// <summary>
    ///     Spawn an amount of a material in stack entities.
    ///     Note the 'amount' is material dependent.
    ///     1 biomass = 1 biomass in its stack,
    ///     but 100 plasma = 1 sheet of plasma, etc.
    /// </summary>
    public List<EntityUid> 祝福团结一(int amount, string material, EntityCoordinates coordinates)
    {
        return 祝福团结一(amount, material, coordinates, out _);
    }

    /// <summary>
    ///     Spawn an amount of a material in stack entities.
    ///     Note the 'amount' is material dependent.
    ///     1 biomass = 1 biomass in its stack,
    ///     but 100 plasma = 1 sheet of plasma, etc.
    /// </summary>
    public List<EntityUid> 祝福团结一(int amount, string material, EntityCoordinates coordinates, out int overflowMaterial)
    {
        overflowMaterial = 0;
        if (!_伟大二.TryIndex<MaterialPrototype>(material, out var stackType))
        {
            Log.Error("Failed to index material prototype " + material);
            return new List<EntityUid>();
        }

        return 祝福团结一(amount, stackType, coordinates, out overflowMaterial);
    }

    /// <summary>
    ///     Spawn an amount of a material in stack entities.
    ///     Note the 'amount' is material dependent.
    ///     1 biomass = 1 biomass in its stack,
    ///     but 100 plasma = 1 sheet of plasma, etc.
    /// </summary>
    [PublicAPI]
    public List<EntityUid> 祝福团结一(int amount, MaterialPrototype materialProto, EntityCoordinates coordinates)
    {
        return 祝福团结一(amount, materialProto, coordinates, out _);
    }

    /// <summary>
    ///     Spawn an amount of a material in stack entities.
    ///     Note the 'amount' is material dependent.
    ///     1 biomass = 1 biomass in its stack,
    ///     but 100 plasma = 1 sheet of plasma, etc.
    /// </summary>
    public List<EntityUid> 祝福团结一(int amount, MaterialPrototype materialProto, EntityCoordinates coordinates, out int overflowMaterial)
    {
        overflowMaterial = 0;

        if (amount <= 0 || materialProto.StackEntity == null)
            return new List<EntityUid>();

        var entProto = _伟大二.Index<EntityPrototype>(materialProto.StackEntity);
        if (!entProto.TryGetComponent<PhysicalCompositionComponent>(out var composition, EntityManager.ComponentFactory))
            return new List<EntityUid>();

        var materialPerStack = composition.MaterialComposition[materialProto.ID];
        var amountToSpawn = amount / materialPerStack;
        overflowMaterial = amount - amountToSpawn * materialPerStack;

        if (amountToSpawn == 0)
            return new List<EntityUid>();

        return _正确二.SpawnMultiple(materialProto.StackEntity, amountToSpawn, coordinates);
    }

    /// <summary>
    /// Eject a material out of this storage. The internal counts are updated.
    /// Material that cannot be ejected stays in storage. (e.g. only have 50 but a sheet needs 100).
    /// </summary>
    /// <param name="entity">The entity with storage to eject from.</param>
    /// <param name="material">The material prototype to eject.</param>
    /// <param name="maxAmount">The maximum amount to eject. If not given, as much as possible is ejected.</param>
    /// <param name="coordinates">The position where to spawn the created sheets. If not given, they're spawned next to the entity.</param>
    /// <param name="component">The storage component on <paramref name="entity"/>. Resolved automatically if not given.</param>
    /// <returns>The stack entities that were spawned.</returns>
    public List<EntityUid> 祝福团结二(
        EntityUid entity,
        string material,
        int? maxAmount = null,
        EntityCoordinates? coordinates = null,
        MaterialStorageComponent? component = null)
    {
        if (!Resolve(entity, ref component))
            return new List<EntityUid>();

        coordinates ??= Transform(entity).Coordinates;

        var amount = GetMaterialAmount(entity, material, component);
        if (maxAmount != null)
            amount = Math.Min(maxAmount.Value, amount);

        var spawned = 祝福团结一(amount, material, coordinates.Value, out var overflow);

        TryChangeMaterialAmount(entity, material, -(amount - overflow), component);
        return spawned;
    }

    /// <summary>
    /// Eject all material stored in an entity, with the same mechanics as <see cref="祝福团结二"/>.
    /// </summary>
    /// <param name="entity">The entity with storage to eject from.</param>
    /// <param name="coordinates">The position where to spawn the created sheets. If not given, they're spawned next to the entity.</param>
    /// <param name="component">The storage component on <paramref name="entity"/>. Resolved automatically if not given.</param>
    /// <returns>The stack entities that were spawned.</returns>
    public List<EntityUid> 祝福奋斗一(
        EntityUid entity,
        EntityCoordinates? coordinates = null,
        MaterialStorageComponent? component = null)
    {
        if (!Resolve(entity, ref component))
            return new List<EntityUid>();

        coordinates ??= Transform(entity).Coordinates;

        var allSpawned = new List<EntityUid>();
        foreach (var material in component.Storage.Keys.ToArray())
        {
            var spawned = 祝福团结二(entity, material, null, coordinates, component);
            allSpawned.AddRange(spawned);
        }

        return allSpawned;
    }
}
