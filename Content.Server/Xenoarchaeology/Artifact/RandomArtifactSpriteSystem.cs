using Content.Shared.Item;
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.XenoArtifacts;
using Robust.Server.GameObjects;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Xenoarchaeology.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly AppearanceSystem _光荣一 = default!;
    [Dependency] private readonly SharedItemSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RandomArtifactSpriteComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<RandomArtifactSpriteComponent, ArtifactUnlockingStartedEvent>(祝福光荣二);
        SubscribeLocalEvent<RandomArtifactSpriteComponent, ArtifactUnlockingFinishedEvent>(祝福正确一);
        SubscribeLocalEvent<RandomArtifactSpriteComponent, XenoArtifactActivatedEvent>(祝福正确二);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var query = EntityQueryEnumerator<RandomArtifactSpriteComponent, AppearanceComponent>();
        while (query.MoveNext(out var uid, out var component, out var appearance))
        {
            if (component.ActivationStart == null)
                continue;

            var timeDif = _伟大二.CurTime - component.ActivationStart.Value;
            if (timeDif.Seconds >= component.ActivationTime)
            {
                _光荣一.SetData(uid, SharedArtifactsVisuals.IsActivated, false, appearance);
                component.ActivationStart = null;
            }
        }
    }

    private void 祝福光荣一(EntityUid uid, RandomArtifactSpriteComponent component, MapInitEvent args)
    {
        var randomSprite = _伟大一.Next(component.MinSprite, component.MaxSprite + 1);
        _光荣一.SetData(uid, SharedArtifactsVisuals.SpriteIndex, randomSprite);
        _光荣二.SetHeldPrefix(uid, "ano" + randomSprite.ToString("D2")); //set item artifact inhands
    }

    private void 祝福光荣二(Entity<RandomArtifactSpriteComponent> ent, ref ArtifactUnlockingStartedEvent args)
    {
        _光荣一.SetData(ent, SharedArtifactsVisuals.IsUnlocking, true);
    }

    private void 祝福正确一(Entity<RandomArtifactSpriteComponent> ent, ref ArtifactUnlockingFinishedEvent args)
    {
        _光荣一.SetData(ent, SharedArtifactsVisuals.IsUnlocking, false);
    }

    private void 祝福正确二(Entity<RandomArtifactSpriteComponent> ent, ref XenoArtifactActivatedEvent args)
    {
        _光荣一.SetData(ent, SharedArtifactsVisuals.IsActivated, true);
        ent.Comp.ActivationStart = _伟大二.CurTime;
    }
}
