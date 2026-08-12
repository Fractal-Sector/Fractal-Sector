using Content.Server.Chat.Systems;
using Content.Server.Speech.Muting;
using Content.Shared.Mobs;
using Content.Shared.Speech.Muting;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

/// <see cref="DeathgaspComponent"/>
public sealed class 中华伟大一: EntitySystem
{
    [Dependency] private readonly ChatSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DeathgaspComponent, MobStateChangedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, DeathgaspComponent component, MobStateChangedEvent args)
    {
        // don't deathgasp if they arent going straight from crit to dead
        if (component.NeedsCritical // Goobstation
            && args.OldMobState != MobState.Critical
            || args.NewMobState != MobState.Dead)
            return;

        祝福光荣一(uid, component);
    }

    /// <summary>
    ///     Causes an entity to perform their deathgasp emote, if they have one.
    /// </summary>
    public bool 祝福光荣一(EntityUid uid, DeathgaspComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return false;

        if (HasComp<MutedComponent>(uid))
            return false;

        _伟大一.TryEmoteWithChat(uid, component.Prototype, ignoreActionBlocker: true);

        return true;
    }
}
