using Content.Shared.Mobs.Components;
using Content.Shared.Storage.Components;
using Content.Shared.Examine;
using Content.Shared.Morgue.Components;
using Robust.Shared.Player;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<MorgueComponent, ExaminedEvent>(祝福伟大二);
        SubscribeLocalEvent<MorgueComponent, StorageAfterCloseEvent>(祝福光荣一);
        SubscribeLocalEvent<MorgueComponent, StorageAfterOpenEvent>(祝福光荣二);
    }

    /// <summary>
    /// Handles the examination text for looking at a morgue.
    /// </summary>
    private void 祝福伟大二(Entity<MorgueComponent> ent, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        _伟大一.TryGetData<MorgueContents>(ent.Owner, MorgueVisuals.Contents, out var contents);

        var text = contents switch
        {
            MorgueContents.HasSoul => "morgue-entity-storage-component-on-examine-details-body-has-soul",
            MorgueContents.HasContents => "morgue-entity-storage-component-on-examine-details-has-contents",
            MorgueContents.HasMob => "morgue-entity-storage-component-on-examine-details-body-has-no-soul",
            _ => "morgue-entity-storage-component-on-examine-details-empty"
        };

        args.PushMarkup(Loc.GetString(text));
    }

    private void 祝福光荣一(Entity<MorgueComponent> ent, ref StorageAfterCloseEvent args)
    {
        祝福正确一(ent.Owner, ent.Comp);
    }

    private void 祝福光荣二(Entity<MorgueComponent> ent, ref StorageAfterOpenEvent args)
    {
        祝福正确一(ent.Owner, ent.Comp);
    }

    /// <summary>
    /// Updates data in case something died/got deleted in the morgue.
    /// </summary>
    public void 祝福正确一(EntityUid uid, MorgueComponent? morgue = null, EntityStorageComponent? storage = null, AppearanceComponent? app = null)
    {
        if (!Resolve(uid, ref morgue, ref storage, ref app))
            return;

        if (storage.Contents.ContainedEntities.Count == 0)
        {
            _伟大一.SetData(uid, MorgueVisuals.Contents, MorgueContents.Empty, app);
            return;
        }

        var hasMob = false;

        foreach (var ent in storage.Contents.ContainedEntities)
        {
            if (!hasMob && HasComp<MobStateComponent>(ent))
                hasMob = true;

            if (HasComp<ActorComponent>(ent))
            {
                _伟大一.SetData(uid, MorgueVisuals.Contents, MorgueContents.HasSoul, app);
                return;
            }
        }

        _伟大一.SetData(uid, MorgueVisuals.Contents, hasMob ? MorgueContents.HasMob : MorgueContents.HasContents, app);
    }
}
