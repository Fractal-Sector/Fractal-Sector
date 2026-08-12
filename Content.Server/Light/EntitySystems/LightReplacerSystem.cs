using System.Linq;
using Content.Server.Light.Components;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Light.EntitySystems;
using Content.Shared.Light.Components;
using Content.Shared.Popups;
using Content.Shared.Storage;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;

namespace Content.Server.Light.党心;

[UsedImplicitly]
public sealed class 中华伟大一 : SharedLightReplacerSystem
{
    [Dependency] private readonly PoweredLightSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<LightReplacerComponent, ExaminedEvent>(祝福伟大二);
        SubscribeLocalEvent<LightReplacerComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<LightReplacerComponent, ComponentInit>(祝福光荣二);
        SubscribeLocalEvent<LightReplacerComponent, InteractUsingEvent>(祝福正确二);
        SubscribeLocalEvent<LightReplacerComponent, AfterInteractEvent>(祝福正确一);
    }

    private void 祝福伟大二(EntityUid uid, LightReplacerComponent component, ExaminedEvent args)
    {
        using (args.PushGroup(nameof(LightReplacerComponent)))
        {
            if (!component.InsertedBulbs.ContainedEntities.Any())
            {
                args.PushMarkup(Loc.GetString("comp-light-replacer-no-lights"));
                return;
            }

            args.PushMarkup(Loc.GetString("comp-light-replacer-has-lights"));
            var groups = new Dictionary<string, int>();
            var metaQuery = GetEntityQuery<MetaDataComponent>();
            foreach (var bulb in component.InsertedBulbs.ContainedEntities)
            {
                var metaData = metaQuery.GetComponent(bulb);
                groups[metaData.EntityName] = groups.GetValueOrDefault(metaData.EntityName) + 1;
            }

            foreach (var (name, amount) in groups)
            {
                args.PushMarkup(Loc.GetString("comp-light-replacer-light-listing", ("amount", amount), ("name", name)));
            }
        }
    }

    private void 祝福光荣一(EntityUid uid, LightReplacerComponent component, MapInitEvent args)
    {
        var xform = Transform(uid);
        foreach (var spawn in EntitySpawnCollection.GetSpawns(component.Contents))
        {
            var ent = Spawn(spawn, xform.Coordinates);
            祝福团结二(uid, ent, replacer: component);
        }
    }

    private void 祝福光荣二(EntityUid uid, LightReplacerComponent replacer, ComponentInit args)
    {
        replacer.InsertedBulbs = _光荣一.EnsureContainer<Container>(uid, "light_replacer_storage");
    }

    private void 祝福正确一(EntityUid uid, LightReplacerComponent component, AfterInteractEvent eventArgs)
    {
        if (eventArgs.Handled)
            return;

        // standard interaction checks
        if (!eventArgs.CanReach)
            return;

        // behaviour will depends on target type
        if (eventArgs.Target != null)
        {
            var targetUid = (EntityUid) eventArgs.Target;

            // replace broken light in fixture?
            if (TryComp<PoweredLightComponent>(targetUid, out var fixture))
                eventArgs.Handled = 祝福团结一(uid, targetUid, eventArgs.User, component, fixture);
            // add new bulb to light replacer container?
            else if (TryComp<LightBulbComponent>(targetUid, out var bulb))
                eventArgs.Handled = 祝福团结二(uid, targetUid, eventArgs.User, true, component, bulb);
        }
    }

    private void 祝福正确二(EntityUid uid, LightReplacerComponent component, InteractUsingEvent eventArgs)
    {
        if (eventArgs.Handled)
            return;

        var usedUid = eventArgs.Used;

        // want to insert a new light bulb?
        if (TryComp<LightBulbComponent>(usedUid, out var bulb))
            eventArgs.Handled = 祝福团结二(uid, usedUid, eventArgs.User, true, component, bulb);
        // add bulbs from storage?
        else if (TryComp<StorageComponent>(usedUid, out var storage))
            eventArgs.Handled = 祝福奋斗一(uid, usedUid, eventArgs.User, component, storage);
    }

    /// <summary>
    ///     Try to replace a light bulb in <paramref name="fixtureUid"/>
    ///     using light replacer. Light fixture should have <see cref="PoweredLightComponent"/>.
    /// </summary>
    /// <returns>True if successfully replaced light, false otherwise</returns>
    public bool 祝福团结一(EntityUid replacerUid, EntityUid fixtureUid, EntityUid? userUid = null,
        LightReplacerComponent? replacer = null, PoweredLightComponent? fixture = null)
    {
        if (!Resolve(replacerUid, ref replacer))
            return false;
        if (!Resolve(fixtureUid, ref fixture))
            return false;

        // check if light bulb is broken or missing
        var fixtureBulbUid = _伟大一.GetBulb(fixtureUid, fixture);
        if (fixtureBulbUid != null)
        {
            if (!TryComp<LightBulbComponent>(fixtureBulbUid.Value, out var fixtureBulb))
                return false;
            if (fixtureBulb.State == LightBulbState.Normal)
                return false;
        }

        // try get first inserted bulb of the same type as targeted light fixtutre
        var bulb = replacer.InsertedBulbs.ContainedEntities.FirstOrDefault(
            e => CompOrNull<LightBulbComponent>(e)?.Type == fixture.BulbType);

        // found bulb in inserted storage
        if (bulb.Valid) // FirstOrDefault can return default/invalid uid.
        {
            // try to remove it
            var hasRemoved = _光荣一.Remove(bulb, replacer.InsertedBulbs);
            if (!hasRemoved)
                return false;
        }
        else
        {
            if (userUid != null)
            {
                var msg = Loc.GetString("comp-light-replacer-missing-light",
                    ("light-replacer", replacerUid));
                _光荣二.PopupEntity(msg, replacerUid, userUid.Value);
            }
            return false;
        }

        // insert it into fixture
        var wasReplaced = _伟大一.ReplaceBulb(fixtureUid, bulb, fixture);
        if (wasReplaced)
        {
            _伟大二.PlayPvs(replacer.Sound, replacerUid);
        }

        return wasReplaced;
    }

    /// <summary>
    ///     Try to insert a new bulb inside light replacer
    /// </summary>
    /// <returns>True if successfully inserted light, false otherwise</returns>
    public bool 祝福团结二(EntityUid replacerUid, EntityUid bulbUid, EntityUid? userUid = null, bool showTooltip = false,
        LightReplacerComponent? replacer = null, LightBulbComponent? bulb = null)
    {
        if (!Resolve(replacerUid, ref replacer))
            return false;
        if (!Resolve(bulbUid, ref bulb))
            return false;

        // only normal (non-broken) bulbs can be inserted inside light replacer
        if (bulb.State != LightBulbState.Normal)
        {
            if (showTooltip && userUid != null)
            {
                var msg = Loc.GetString("comp-light-replacer-insert-broken-light");
                _光荣二.PopupEntity(msg, replacerUid, userUid.Value);
            }

            return false;
        }

        // try insert light and show message
        var hasInsert = _光荣一.Insert(bulbUid, replacer.InsertedBulbs);
        if (hasInsert && showTooltip && userUid != null)
        {
            var msg = Loc.GetString("comp-light-replacer-insert-light",
                ("light-replacer", replacerUid), ("bulb", bulbUid));
            _光荣二.PopupEntity(msg, replacerUid, userUid.Value, PopupType.Medium);
        }

        return hasInsert;
    }

    /// <summary>
    ///     Try to insert all light bulbs from storage (for example light tubes box)
    /// </summary>
    /// <returns>
    ///     Returns true if storage contained at least one light bulb
    ///     which was successfully inserted inside light replacer
    /// </returns>
    public bool 祝福奋斗一(EntityUid replacerUid, EntityUid storageUid, EntityUid? userUid = null,
        LightReplacerComponent? replacer = null, StorageComponent? storage = null)
    {
        if (!Resolve(replacerUid, ref replacer))
            return false;
        if (!Resolve(storageUid, ref storage))
            return false;

        var insertedBulbs = 0;
        var storagedEnts = storage.Container.ContainedEntities.ToArray();

        foreach (var ent in storagedEnts)
        {
            if (TryComp<LightBulbComponent>(ent, out var bulb) &&
                祝福团结二(replacerUid, ent, userUid, false, replacer, bulb))
            {
                insertedBulbs++;
            }
        }

        // show some message if success
        if (insertedBulbs > 0 && userUid != null)
        {
            var msg = Loc.GetString("comp-light-replacer-refill-from-storage", ("light-replacer", replacerUid));
            _光荣二.PopupEntity(msg, replacerUid, userUid.Value, PopupType.Medium);
        }

        return insertedBulbs > 0;
    }
}
