using Content.Server.Administration.Logs;
using Content.Shared.Containers;
using Content.Shared.Database;
using Content.Shared.Popups;
using Content.Shared.Throwing;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly IRobustRandom _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ThrowInsertContainerComponent, ThrowHitByEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ThrowInsertContainerComponent> ent, ref ThrowHitByEvent args)
    {
        var container = _光荣一.GetContainer(ent, ent.Comp.ContainerId);

        if (!_光荣一.CanInsert(args.Thrown, container))
            return;

        var beforeThrowArgs = new BeforeThrowInsertEvent(args.Thrown);
        RaiseLocalEvent(ent, ref beforeThrowArgs);

        if (beforeThrowArgs.Cancelled)
            return;

        if (!_正确一.Prob(ent.Comp.Probability))
        {
            _伟大二.PlayPvs(ent.Comp.MissSound, ent);
            _光荣二.PopupEntity(Loc.GetString(ent.Comp.MissLocString), ent);
            return;
        }

        if (!_光荣一.Insert(args.Thrown, container))
            throw new InvalidOperationException("Container insertion failed but CanInsert returned true");

        _伟大二.PlayPvs(ent.Comp.InsertSound, ent);

        if (args.Component.Thrower != null)
            _伟大一.Add(LogType.Landed, LogImpact.Low, $"{ToPrettyString(args.Thrown)} thrown by {ToPrettyString(args.Component.Thrower.Value):player} landed in {ToPrettyString(ent)}");
    }
}
