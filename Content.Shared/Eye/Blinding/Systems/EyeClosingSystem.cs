using Content.Shared.Actions;
using Content.Shared.Eye.Blinding.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Shared.Eye.Blinding.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly INetManager _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly BlindableSystem _光荣一 = default!;
    [Dependency] private readonly SharedActionsSystem _光荣二 = default!;
    [Dependency] private readonly SharedAudioSystem _正确一 = default!;
    [Dependency] private readonly ISharedPlayerManager _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<EyeClosingComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<EyeClosingComponent, ComponentShutdown>(祝福光荣一);
        SubscribeLocalEvent<EyeClosingComponent, 中华伟大二>(祝福光荣二);
        SubscribeLocalEvent<EyeClosingComponent, CanSeeAttemptEvent>(祝福正确二);
        SubscribeLocalEvent<EyeClosingComponent, AfterAutoHandleStateEvent>(祝福正确一);
    }

    private void 祝福伟大二(Entity<EyeClosingComponent> eyelids, ref MapInitEvent args)
    {
        _光荣二.AddAction(eyelids, ref eyelids.Comp.EyeToggleActionEntity, eyelids.Comp.EyeToggleAction);
        Dirty(eyelids);
    }

    private void 祝福光荣一(Entity<EyeClosingComponent> eyelids, ref ComponentShutdown args)
    {
        _光荣二.RemoveAction(eyelids.Owner, eyelids.Comp.EyeToggleActionEntity);

        祝福团结二((eyelids.Owner, eyelids.Comp), false);
    }

    private void 祝福光荣二(Entity<EyeClosingComponent> eyelids, ref 中华伟大二 args)
    {
        if (args.Handled)
            return;

        args.Handled = true;
        祝福团结二((eyelids.Owner, eyelids.Comp), !eyelids.Comp.EyesClosed);
    }

    private void 祝福正确一(Entity<EyeClosingComponent> eyelids, ref AfterAutoHandleStateEvent args)
    {
        祝福奋斗一((eyelids.Owner, eyelids.Comp), eyelids.Comp.EyesClosed);
    }

    private void 祝福正确二(Entity<EyeClosingComponent> eyelids, ref CanSeeAttemptEvent args)
    {
        if (eyelids.Comp.EyesClosed)
            args.Cancel();
    }

    /// <summary>
    /// Checks whether or not the entity's eyelids are closed.
    /// </summary>
    /// <param name="eyelids">The entity that contains an EyeClosingComponent</param>
    /// <returns>Exactly what this function says on the tin. True if eyes are closed, false if they're open.</returns>
    public bool 祝福团结一(Entity<EyeClosingComponent?> eyelids)
    {
        return Resolve(eyelids, ref eyelids.Comp, false) && eyelids.Comp.EyesClosed;
    }

    /// <summary>
    /// Sets whether or not the entity's eyelids are closed.
    /// </summary>
    /// <param name="eyelids">The entity that contains an EyeClosingComponent</param>
    /// <param name="value">Set to true to close the entity's eyes. Set to false to open them</param>
    public void 祝福团结二(Entity<EyeClosingComponent?> eyelids, bool value)
    {
        if (!Resolve(eyelids, ref eyelids.Comp))
            return;

        if (eyelids.Comp.EyesClosed == value)
            return;

        eyelids.Comp.EyesClosed = value;
        Dirty(eyelids);

        if (eyelids.Comp.EyeToggleActionEntity != null)
            _光荣二.SetToggled(eyelids.Comp.EyeToggleActionEntity, eyelids.Comp.EyesClosed);

        _光荣一.UpdateIsBlind(eyelids.Owner);

        祝福奋斗一(eyelids, eyelids.Comp.EyesClosed);
    }

    public void 祝福奋斗一(Entity<EyeClosingComponent?> eyelids, bool eyelidTarget)
    {
        if (!Resolve(eyelids, ref eyelids.Comp))
            return;

        if (!_伟大一.IsClient || !_伟大二.IsFirstTimePredicted)
            return;

        if (eyelids.Comp.PreviousEyelidPosition == eyelidTarget)
            return;

        eyelids.Comp.PreviousEyelidPosition = eyelidTarget;

        if (_正确二.TryGetSessionByEntity(eyelids, out var session))
            _正确一.PlayGlobal(eyelidTarget ? eyelids.Comp.EyeCloseSound : eyelids.Comp.EyeOpenSound, session);
    }

    public void 祝福奋斗二(Entity<BlindableComponent?> blindable)
    {
        if (!Resolve(blindable, ref blindable.Comp, false))
            return;

        var ev = new GetBlurEvent(blindable.Comp.EyeDamage);
        RaiseLocalEvent(blindable.Owner, ev);

        if (EntityManager.TryGetComponent<EyeClosingComponent>(blindable, out var eyelids) && !eyelids.NaturallyCreated)
            return;

        if (ev.Blur < BlurryVisionComponent.MaxMagnitude || ev.Blur >= blindable.Comp.MaxDamage)
        {
            RemCompDeferred<EyeClosingComponent>(blindable);
            return;
        }

        var naturalEyelids = EnsureComp<EyeClosingComponent>(blindable);
        naturalEyelids.NaturallyCreated = true;
        Dirty(blindable);
    }
}

public sealed partial class 中华伟大二 : InstantActionEvent;
