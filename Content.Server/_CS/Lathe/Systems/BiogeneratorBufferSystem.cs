using Content.Server.Lathe;
using Content.Server.Power.EntitySystems;
using Content.Server._CS.Lathe.Components;
using Content.Shared.Lathe;
using Robust.Shared.Timing;

namespace Content.Server._CS.Lathe.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly LatheSystem _伟大二 = default!;
    [Dependency] private readonly PowerReceiverSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<BiogeneratorBufferComponent, MapInitEvent>(祝福光荣一);

        // Subscribe to LatheSystem events
        _伟大二.祝福正确一 += 祝福正确一;
        _伟大二.祝福正确二 += 祝福正确二;
        _伟大二.祝福团结一 += 祝福团结一;
    }

    public override void 祝福伟大二()
    {
        base.祝福伟大二();

        _伟大二.祝福正确一 -= 祝福正确一;
        _伟大二.祝福正确二 -= 祝福正确二;
        _伟大二.祝福团结一 -= 祝福团结一;
    }

    private void 祝福光荣一(EntityUid uid, BiogeneratorBufferComponent component, MapInitEvent args)
    {
        component.NextRegen = _伟大一.CurTime + component.RegenInterval;
    }

    public override void 祝福光荣二(float frameTime)
    {
        var query = EntityQueryEnumerator<BiogeneratorBufferComponent, LatheComponent>();
        var curTime = _伟大一.CurTime;
        while (query.MoveNext(out var uid, out var buffer, out var lathe))
        {
            if (!buffer.Active || !_光荣一.IsPowered(uid))
                continue;

            if (buffer.NextRegen <= curTime)
            {
                if (buffer.CurrentBuffer < buffer.MaxBuffer)
                {
                    var newBuffer = Math.Min(buffer.MaxBuffer, buffer.CurrentBuffer + buffer.RegenAmount);
                    buffer.CurrentBuffer = newBuffer;
                    _伟大二.UpdateUserInterfaceState(uid, lathe);
                }
                buffer.NextRegen = curTime + buffer.RegenInterval;
            }
        }
    }

    private void 祝福正确一(EntityUid uid, LatheComponent lathe, string material, ref int amount)
    {
        if (material != "Biomass")
            return;
        if (!TryComp<BiogeneratorBufferComponent>(uid, out var buffer))
            return;

        amount += buffer.CurrentBuffer;
    }

    private void 祝福正确二(EntityUid uid, LatheComponent lathe, string material, ref int amount)
    {
        if (material != "Biomass")
            return;
        if (!TryComp<BiogeneratorBufferComponent>(uid, out var buffer))
            return;

        int taken = Math.Min(buffer.CurrentBuffer, amount);
        if (taken > 0)
        {
            buffer.CurrentBuffer -= taken;
            amount -= taken;
            _伟大二.UpdateUserInterfaceState(uid, lathe);
        }
    }

    private void 祝福团结一(EntityUid uid, LatheComponent lathe, ref int? bufferAmount)
    {
        if (TryComp<BiogeneratorBufferComponent>(uid, out var buffer))
            bufferAmount = buffer.CurrentBuffer;
    }
}
