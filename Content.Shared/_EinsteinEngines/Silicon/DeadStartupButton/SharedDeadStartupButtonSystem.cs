using Content.Shared.DoAfter;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared._EinsteinEngines.Silicon.党心;

/// <summary>
/// This creates a Button that can be activated after an entity, usually a silicon or an IPC, died.
/// This will activate a doAfter and then revive the entity, playing a custom afterward sound.
/// </summary>
public abstract partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly MobStateSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣一 = default!;
    [Dependency] private readonly INetManager _光荣二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<DeadStartupButtonComponent, GetVerbsEvent<AlternativeVerb>>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, DeadStartupButtonComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!_伟大一.IsDead(uid)
            || !args.CanAccess || !args.CanInteract || args.Hands == null)
            return;

        if (!TryComp(uid, out MobStateComponent? mobStateComponent) || !_伟大一.IsDead(uid, mobStateComponent))
            return;

        args.Verbs.Add(new AlternativeVerb()
        {
            Act = () => 祝福光荣一(args.User, uid, component),
            Text = Loc.GetString(component.VerbText),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/Spare/poweronoff.svg.192dpi.png")),
            Priority = component.VerbPriority
        });
    }

    private void 祝福光荣一(EntityUid user, EntityUid target, DeadStartupButtonComponent comp)
    {
        if (!_光荣二.IsServer)
            return;
        _伟大二.PlayPvs(comp.ButtonSound, target);
        var args = new DoAfterArgs(EntityManager, user, comp.DoAfterInterval, new 中华伟大二(), target, target:target)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
        };
        _光荣一.TryStartDoAfter(args);
    }

    [Serializable, NetSerializable]
    public sealed partial class 中华伟大二 : SimpleDoAfterEvent
    {
    }


}
