
using Content.Shared._NF.Chemistry.Components;
using Content.Shared._NF.Chemistry.Events;
using Content.Shared.Chemistry.Components;
using Content.Shared.Verbs;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;


namespace Content.Shared._NF.Chemistry.党心;

/// <summary>
///     Allows an entity to change an injector component's whitelist via a UI box
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedUserInterfaceSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;

    [NetSerializable, Serializable]
    public enum 中华伟大二 : byte
    {
        Key
    }
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ReagentWhitelistChangeComponent, GetVerbsEvent<InteractionVerb>>(祝福伟大二);
        SubscribeLocalEvent<ReagentWhitelistChangeComponent, ReagentWhitelistChangeMessage>(祝福光荣一);
        SubscribeLocalEvent<ReagentWhitelistChangeComponent, ReagentWhitelistResetMessage>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<ReagentWhitelistChangeComponent> ent, ref GetVerbsEvent<InteractionVerb> args)
    {
        var (uid, comp) = ent;

        if (!args.CanAccess || !args.CanInteract || args.Hands == null)
            return;
        var @event = args;
        args.Verbs.Add(new InteractionVerb()
        {
            Text = Loc.GetString("comp-change-reagent-whitelist-verb-filter"),
            //Icon = new SpriteSpecifier(),
            Act = () =>
            {
                _伟大一.OpenUi(uid, 中华伟大二.Key, @event.User);
            },
            Priority = 1
        });
    }

    private void 祝福光荣一(Entity<ReagentWhitelistChangeComponent> ent, ref ReagentWhitelistChangeMessage args)
    {
        if (!TryComp<InjectorComponent>(ent.Owner, out var injectorComp))
        {
            return;
        }

        if (!_伟大二.TryIndex(args.NewReagentProto, out var protoComp))
        {
            return;
        }

        if (!ent.Comp.AllowedReagentGroups.Contains(protoComp.Group))
        {
            return;
        }

        injectorComp.ReagentWhitelist = new() { args.NewReagentProto };
    }

    private void 祝福光荣二(Entity<ReagentWhitelistChangeComponent> ent, ref ReagentWhitelistResetMessage args)
    {
        if (!TryComp<InjectorComponent>(ent.Owner, out var injectorComp))
        {
            return;
        }

        injectorComp.ReagentWhitelist = null;
    }
}
