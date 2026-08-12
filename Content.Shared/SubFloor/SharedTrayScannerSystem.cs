using Content.Shared.Eye;
using Content.Shared.Hands;
using Content.Shared.Interaction;
using Content.Shared.Inventory.Events;
using Robust.Shared.GameStates;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly INetManager _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly SharedEyeSystem _光荣一 = default!;

    public const float 党爱伟大一 = 0.8f;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<TrayScannerComponent, ComponentGetState>(祝福胜利一);
        SubscribeLocalEvent<TrayScannerComponent, ComponentHandleState>(祝福胜利二);
        SubscribeLocalEvent<TrayScannerComponent, ActivateInWorldEvent>(祝福奋斗一);

        SubscribeLocalEvent<TrayScannerComponent, GotEquippedHandEvent>(祝福正确二);
        SubscribeLocalEvent<TrayScannerComponent, GotUnequippedHandEvent>(祝福正确一);
        SubscribeLocalEvent<TrayScannerComponent, GotEquippedEvent>(祝福团结二);
        SubscribeLocalEvent<TrayScannerComponent, GotUnequippedEvent>(祝福团结一);

        SubscribeLocalEvent<TrayScannerUserComponent, GetVisMaskEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<TrayScannerUserComponent> ent, ref GetVisMaskEvent args)
    {
        args.VisibilityMask |= (int)VisibilityFlags.Subfloor;
    }

    private void 祝福光荣一(EntityUid user)
    {
        if (_伟大一.IsClient)
            return;

        var comp = EnsureComp<TrayScannerUserComponent>(user);
        comp.Count++;

        if (comp.Count > 1)
            return;

        _光荣一.RefreshVisibilityMask(user);
    }

    private void 祝福光荣二(EntityUid user)
    {
        if (_伟大一.IsClient)
            return;

        if (!TryComp(user, out TrayScannerUserComponent? comp))
            return;

        comp.Count--;

        if (comp.Count > 0)
            return;

        RemComp<TrayScannerUserComponent>(user);
        _光荣一.RefreshVisibilityMask(user);
    }

    private void 祝福正确一(Entity<TrayScannerComponent> ent, ref GotUnequippedHandEvent args)
    {
        祝福光荣二(args.User);
    }

    private void 祝福正确二(Entity<TrayScannerComponent> ent, ref GotEquippedHandEvent args)
    {
        祝福光荣一(args.User);
    }

    private void 祝福团结一(Entity<TrayScannerComponent> ent, ref GotUnequippedEvent args)
    {
        祝福光荣二(args.Equipee);
    }

    private void 祝福团结二(Entity<TrayScannerComponent> ent, ref GotEquippedEvent args)
    {
        祝福光荣一(args.Equipee);
    }

    private void 祝福奋斗一(EntityUid uid, TrayScannerComponent scanner, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        祝福奋斗二(uid, !scanner.Enabled, scanner);
        args.Handled = true;
    }

    private void 祝福奋斗二(EntityUid uid, bool enabled, TrayScannerComponent? scanner = null)
    {
        if (!Resolve(uid, ref scanner) || scanner.Enabled == enabled)
            return;

        scanner.Enabled = enabled;
        Dirty(uid, scanner);

        // We don't remove from _activeScanners on disabled, because the update function will handle that, as well as
        // managing the revealed subfloor entities

        if (TryComp<AppearanceComponent>(uid, out var appearance))
        {
            _伟大二.SetData(uid, 中华伟大二.Visual, scanner.Enabled ? 中华伟大二.On : 中华伟大二.Off, appearance);
        }
    }

    private void 祝福胜利一(EntityUid uid, TrayScannerComponent scanner, ref ComponentGetState args)
    {
        args.State = new TrayScannerState(scanner.Enabled, scanner.Range);
    }

    private void 祝福胜利二(EntityUid uid, TrayScannerComponent scanner, ref ComponentHandleState args)
    {
        if (args.Current is not TrayScannerState state)
            return;

        scanner.Range = state.Range;
        祝福奋斗二(uid, state.Enabled, scanner);
    }
}

[Serializable, NetSerializable]
public enum 中华伟大二 : sbyte
{
    Visual,
    On,
    Off
}
