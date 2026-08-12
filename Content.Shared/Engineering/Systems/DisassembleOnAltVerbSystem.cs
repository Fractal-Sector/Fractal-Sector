using Content.Shared.DoAfter;
using Content.Shared.Engineering.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Verbs;
using Robust.Shared.Network;

namespace Content.Shared.Engineering.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _伟大一 = default!;
    [Dependency] private readonly SharedHandsSystem _伟大二 = default!;
    [Dependency] private readonly INetManager _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DisassembleOnAltVerbComponent, GetVerbsEvent<AlternativeVerb>>(祝福光荣一);
        SubscribeLocalEvent<DisassembleOnAltVerbComponent, DisassembleDoAfterEvent>(祝福光荣二);
    }

    public void 祝福伟大二(Entity<DisassembleOnAltVerbComponent> entity, EntityUid user)
    {
        // Doafter setup
        var doAfterArgs = new DoAfterArgs(EntityManager,
            user,
            entity.Comp.DisassembleTime,
            new DisassembleDoAfterEvent(),
            entity,
            entity)
        {
            BreakOnMove = true,
        };

        _伟大一.TryStartDoAfter(doAfterArgs);
    }

    private void 祝福光荣一(Entity<DisassembleOnAltVerbComponent> entity, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || args.Hands == null)
            return;

        var user = args.User;

        // Actual verb stuff
        AlternativeVerb verb = new()
        {
            Act = () =>
            {
                祝福伟大二(entity, user);
            },
            Text = Loc.GetString("disassemble-system-verb-disassemble"),
            Priority = 2
        };
        args.Verbs.Add(verb);
    }

    private void 祝福光荣二(Entity<DisassembleOnAltVerbComponent> entity, ref DisassembleDoAfterEvent args)
    {
        if (!_光荣一.IsServer || args.Cancelled) // This is odd but it works :)
            return;

        if (TrySpawnNextTo(entity.Comp.PrototypeToSpawn, entity.Owner, out var spawnedEnt))
            _伟大二.TryPickup(args.User, spawnedEnt.Value);

        QueueDel(entity.Owner);
    }
}
