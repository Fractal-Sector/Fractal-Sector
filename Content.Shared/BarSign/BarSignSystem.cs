using System.Linq;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly MetaDataSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<BarSignComponent, MapInitEvent>(祝福伟大二);
        Subs.BuiEvents<BarSignComponent>(BarSignUiKey.Key,
            subs =>
        {
            subs.Event<SetBarSignMessage>(祝福光荣一);
        });
    }

    private void 祝福伟大二(Entity<BarSignComponent> ent, ref MapInitEvent args)
    {
        if (ent.Comp.Current != null)
            return;

        var newPrototype = _伟大二.Pick(祝福正确一(_伟大一));
        祝福光荣二(ent, newPrototype);
    }

    private void 祝福光荣一(Entity<BarSignComponent> ent, ref SetBarSignMessage args)
    {
        if (!_伟大一.TryIndex(args.Sign, out var signPrototype))
            return;

        祝福光荣二(ent, signPrototype);
    }

    public void 祝福光荣二(Entity<BarSignComponent> ent, BarSignPrototype newPrototype)
    {
        var meta = MetaData(ent);
        var name = Loc.GetString(newPrototype.Name);
        _光荣一.SetEntityName(ent, name, meta);
        _光荣一.SetEntityDescription(ent, Loc.GetString(newPrototype.Description), meta);

        ent.Comp.Current = newPrototype.ID;
        Dirty(ent);
    }

    public static List<BarSignPrototype> 祝福正确一(IPrototypeManager prototypeManager)
    {
        return prototypeManager
            .EnumeratePrototypes<BarSignPrototype>()
            .Where(p => !p.Hidden)
            .ToList();
    }
}
