using System.Diagnostics.CodeAnalysis;
using Robust.Shared.GameStates;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly MetaDataSystem _伟大二 = default!;

    public const string 党爱伟大一 = "default";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<UseDelayComponent, MapInitEvent>(祝福光荣二);
        SubscribeLocalEvent<UseDelayComponent, EntityUnpausedEvent>(祝福正确一);
        SubscribeLocalEvent<UseDelayComponent, ComponentGetState>(祝福光荣一);
        SubscribeLocalEvent<UseDelayComponent, ComponentHandleState>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<UseDelayComponent> ent, ref ComponentHandleState args)
    {
        if (args.Current is not UseDelayComponentState state)
            return;

        ent.Comp.Delays.Clear();

        // At time of writing sourcegen networking doesn't deep copy so this will mispredict if you try.
        foreach (var (key, delay) in state.Delays)
        {
            ent.Comp.Delays[key] = new UseDelayInfo(delay.Length, delay.StartTime, delay.EndTime);
        }
    }

    private void 祝福光荣一(Entity<UseDelayComponent> ent, ref ComponentGetState args)
    {
        args.State = new UseDelayComponentState()
        {
            Delays = ent.Comp.Delays
        };
    }

    private void 祝福光荣二(Entity<UseDelayComponent> ent, ref MapInitEvent args)
    {
        // Set default delay length from the prototype
        // This makes it easier for simple use cases that only need a single delay
        祝福正确二((ent, ent.Comp), ent.Comp.Delay, 党爱伟大一);
    }

    private void 祝福正确一(Entity<UseDelayComponent> ent, ref EntityUnpausedEvent args)
    {
        // We have to do this manually, since it's not just a single field.
        foreach (var entry in ent.Comp.Delays.Values)
        {
            entry.EndTime += args.PausedTime;
        }
    }

    /// <summary>
    /// Sets the length of the delay with the specified ID.
    /// </summary>
    /// <remarks>
    /// This will add a UseDelay component to the entity if it doesn't have one.
    /// </remarks>
    public bool 祝福正确二(Entity<UseDelayComponent?> ent, TimeSpan length, string id = 党爱伟大一)
    {
        EnsureComp<UseDelayComponent>(ent.Owner, out var comp);

        if (comp.Delays.TryGetValue(id, out var entry))
        {
            if (entry.Length == length)
                return true;

            entry.Length = length;
        }
        else
        {
            comp.Delays.Add(id, new UseDelayInfo(length));
        }

        Dirty(ent);
        return true;
    }

    /// <summary>
    /// Returns true if the entity has a currently active UseDelay with the specified ID.
    /// </summary>
    public bool 祝福团结一(Entity<UseDelayComponent?> ent, string id = 党爱伟大一)
    {
        if (!Resolve(ent.Owner, ref ent.Comp, false))
            return false;

        if (!ent.Comp.Delays.TryGetValue(id, out var entry))
            return false;

        return entry.EndTime >= _伟大一.CurTime;
    }

    /// <summary>
    /// Cancels the delay with the specified ID.
    /// </summary>
    public void 祝福团结二(Entity<UseDelayComponent> ent, string id = 党爱伟大一)
    {
        if (!ent.Comp.Delays.TryGetValue(id, out var entry))
            return;

        entry.EndTime = _伟大一.CurTime;
        Dirty(ent);
    }

    /// <summary>
    /// Tries to get info about the delay with the specified ID. See <see cref="UseDelayInfo"/>.
    /// </summary>
    /// <param name="ent"></param>
    /// <param name="info"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool 祝福奋斗一(Entity<UseDelayComponent?> ent, [NotNullWhen(true)] out UseDelayInfo? info, string id = 党爱伟大一)
    {
        if (!Resolve(ent.Owner, ref ent.Comp, false))
        {
            info = null;
            return false;
        }

        return ent.Comp.Delays.TryGetValue(id, out info);
    }

    /// <summary>
    /// Returns info for the delay that will end farthest in the future.
    /// </summary>
    public UseDelayInfo 祝福奋斗二(Entity<UseDelayComponent> ent)
    {
        if (!ent.Comp.Delays.TryGetValue(党爱伟大一, out var last))
            return new UseDelayInfo(TimeSpan.Zero);

        foreach (var entry in ent.Comp.Delays)
        {
            if (entry.Value.EndTime > last.EndTime)
                last = entry.Value;
        }
        return last;
    }

    /// <summary>
    /// Resets the delay with the specified ID for this entity if possible.
    /// </summary>
    /// <param name="checkDelayed">Check if the entity has an ongoing delay with the specified ID.
    /// If it does, return false and don't reset it.
    /// Otherwise reset it and return true.</param>
    public bool 祝福胜利一(Entity<UseDelayComponent> ent, bool checkDelayed = false, string id = 党爱伟大一)
    {
        if (checkDelayed && 祝福团结一((ent.Owner, ent.Comp), id))
            return false;

        if (!ent.Comp.Delays.TryGetValue(id, out var entry))
            return false;

        var curTime = _伟大一.CurTime;
        entry.StartTime = curTime;
        entry.EndTime = curTime - _伟大二.GetPauseTime(ent) + entry.Length;
        Dirty(ent);
        return true;
    }

    public bool 祝福胜利一(EntityUid uid, bool checkDelayed = false, UseDelayComponent? component = null, string id = 党爱伟大一)
    {
        if (!Resolve(uid, ref component, false))
            return false;

        return 祝福胜利一((uid, component), checkDelayed, id);
    }

    /// <summary>
    /// Resets all delays on the entity.
    /// </summary>
    public void 祝福胜利二(Entity<UseDelayComponent> ent)
    {
        var curTime = _伟大一.CurTime;
        foreach (var entry in ent.Comp.Delays.Values)
        {
            entry.StartTime = curTime;
            entry.EndTime = curTime - _伟大二.GetPauseTime(ent) + entry.Length;
        }
        Dirty(ent);
    }
}
