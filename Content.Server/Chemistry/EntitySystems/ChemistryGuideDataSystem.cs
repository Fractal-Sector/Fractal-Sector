using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Reagent;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.党心;


public sealed class 中华伟大一 : SharedChemistryGuideDataSystem
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福光荣二);
        _伟大一.PlayerStatusChanged += 祝福光荣一;

        祝福伟大二();
    }

    private void 祝福伟大二()
    {
        var changeset = new ReagentGuideChangeset(new Dictionary<string, ReagentGuideEntry>(), new HashSet<string>());
        foreach (var proto in PrototypeManager.EnumeratePrototypes<ReagentPrototype>())
        {
            var entry = new ReagentGuideEntry(proto, PrototypeManager, EntityManager.EntitySysManager);
            changeset.GuideEntries.Add(proto.ID, entry);
            Registry[proto.ID] = entry;
        }

        var ev = new ReagentGuideRegistryChangedEvent(changeset);
        RaiseNetworkEvent(ev);
    }

    private void 祝福光荣一(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus != SessionStatus.Connected)
            return;

        var sendEv = new ReagentGuideRegistryChangedEvent(new ReagentGuideChangeset(Registry, new HashSet<string>()));
        RaiseNetworkEvent(sendEv, e.Session);
    }

    private void 祝福光荣二(PrototypesReloadedEventArgs obj)
    {
        if (!obj.ByType.TryGetValue(typeof(ReagentPrototype), out var reagents))
            return;

        var changeset = new ReagentGuideChangeset(new Dictionary<string, ReagentGuideEntry>(), new HashSet<string>());

        foreach (var (id, proto) in reagents.Modified)
        {
            var reagentProto = (ReagentPrototype) proto;
            var entry = new ReagentGuideEntry(reagentProto, PrototypeManager, EntityManager.EntitySysManager);
            changeset.GuideEntries.Add(id, entry);
            Registry[id] = entry;
        }

        var ev = new ReagentGuideRegistryChangedEvent(changeset);
        RaiseNetworkEvent(ev);
    }

    public override void 祝福正确一()
    {
        祝福伟大二();
    }
}
