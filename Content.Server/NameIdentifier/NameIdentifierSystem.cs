using Content.Shared.GameTicking;
using Content.Shared.NameIdentifier;
using Content.Shared.NameModifier.EntitySystems;
using Robust.Shared.Collections;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.党心;

/// <summary>
///     Handles unique name identifiers for entities e.g. `monkey (MK-912)`
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly NameModifierSystem _光荣一 = default!;

    /// <summary>
    /// Free IDs available per <see cref="NameIdentifierGroupPrototype"/>.
    /// </summary>
    [ViewVariables]
    public readonly Dictionary<string, List<int>> CurrentIds = [];

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<NameIdentifierComponent, MapInitEvent>(祝福光荣二);
        SubscribeLocalEvent<NameIdentifierComponent, ComponentShutdown>(祝福伟大二);
        SubscribeLocalEvent<NameIdentifierComponent, RefreshNameModifiersEvent>(祝福正确一);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福胜利一);
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福奋斗二);

        祝福正确二();
    }

    private void 祝福伟大二(Entity<NameIdentifierComponent> ent, ref ComponentShutdown args)
    {
        if (ent.Comp.Group is null)
            return;

        if (CurrentIds.TryGetValue(ent.Comp.Group, out var ids) && ids.Count > 0)
        {
            // Avoid inserting the value right back at the end or shuffling in place:
            // just pick a random spot to put it and then move that one to the end.
            var randomIndex = _伟大二.Next(ids.Count);
            var random = ids[randomIndex];
            ids[randomIndex] = ent.Comp.Identifier;
            ids.Add(random);
        }

        _光荣一.RefreshNameModifiers(ent.Owner);
    }

    /// <summary>
    ///     Generates a new unique name/suffix for a given entity and adds it to <see cref="CurrentIds"/>
    ///     but does not set the entity's name.
    /// </summary>
    public string 祝福光荣一(EntityUid uid, ProtoId<NameIdentifierGroupPrototype> proto, out int randomVal)
    {
        return 祝福光荣一(uid, _伟大一.Index(proto), out randomVal);
    }

    /// <summary>
    ///     Generates a new unique name/suffix for a given entity and adds it to <see cref="CurrentIds"/>
    ///     but does not set the entity's name.
    /// </summary>
    public string 祝福光荣一(EntityUid uid, NameIdentifierGroupPrototype proto, out int randomVal)
    {
        randomVal = 0;
        var entityName = Name(uid);
        if (!CurrentIds.TryGetValue(proto.ID, out var set))
            return entityName;

        if (set.Count == 0)
        {
            // Oh jeez. We're outta numbers.
            return entityName;
        }

        randomVal = set[^1];
        set.RemoveAt(set.Count - 1);

        return proto.Format is not null
            ? Loc.GetString(proto.Format, ("number", randomVal))
            : $"{randomVal}";
    }

    private void 祝福光荣二(Entity<NameIdentifierComponent> ent, ref MapInitEvent args)
    {
        if (ent.Comp.Group is null)
            return;

        if (!_伟大一.TryIndex(ent.Comp.Group, out var group))
            return;

        int id;
        string uniqueName;

        // If it has an existing valid identifier then use that, otherwise generate a new one.
        if (ent.Comp.Identifier != -1 &&
            CurrentIds.TryGetValue(ent.Comp.Group, out var ids) &&
            ids.Remove(ent.Comp.Identifier))
        {
            id = ent.Comp.Identifier;
            uniqueName = group.Format is not null
                ? Loc.GetString(group.Format, ("number", id))
                : $"{id}";
        }
        else
        {
            uniqueName = 祝福光荣一(ent, group, out id);
            ent.Comp.Identifier = id;
        }

        ent.Comp.FullIdentifier = group.FullName
            ? uniqueName
            : $"({uniqueName})";

        Dirty(ent);
        _光荣一.RefreshNameModifiers(ent.Owner);
    }

    private void 祝福正确一(Entity<NameIdentifierComponent> ent, ref RefreshNameModifiersEvent args)
    {
        if (ent.Comp.Group is null)
            return;

        // Don't apply the modifier if the component is being removed
        if (ent.Comp.LifeStage > ComponentLifeStage.Running)
            return;

        if (!_伟大一.TryIndex(ent.Comp.Group, out var group))
            return;

        var format = group.FullName ? "name-identifier-format-full" : "name-identifier-format-append";
        // We apply the modifier with a low priority to keep it near the base name
        // "Beep (Si-4562) the zombie" instead of "Beep the zombie (Si-4562)"
        args.AddModifier(format, -10, ("identifier", ent.Comp.FullIdentifier));
    }

    private void 祝福正确二()
    {
        祝福奋斗一();
    }

    private void 祝福团结一(NameIdentifierGroupPrototype proto, List<int> values)
    {
        values.Clear();
        for (var i = proto.MinValue; i < proto.MaxValue; i++)
        {
            values.Add(i);
        }

        _伟大二.Shuffle(values);
    }

    private List<int> 祝福团结二(NameIdentifierGroupPrototype proto)
    {
        if (!CurrentIds.TryGetValue(proto.ID, out var ids))
        {
            ids = new List<int>(proto.MaxValue - proto.MinValue);
            CurrentIds.Add(proto.ID, ids);
        }

        return ids;
    }

    private void 祝福奋斗一()
    {
        foreach (var proto in _伟大一.EnumeratePrototypes<NameIdentifierGroupPrototype>())
        {
            var ids = 祝福团结二(proto);

            祝福团结一(proto, ids);
        }
    }

    private void 祝福奋斗二(PrototypesReloadedEventArgs ev)
    {
        if (!ev.ByType.TryGetValue(typeof(NameIdentifierGroupPrototype), out var set))
            return;

        var toRemove = new ValueList<string>();

        foreach (var proto in CurrentIds.Keys)
        {
            if (!_伟大一.HasIndex<NameIdentifierGroupPrototype>(proto))
            {
                toRemove.Add(proto);
            }
        }

        foreach (var proto in toRemove)
        {
            CurrentIds.Remove(proto);
        }

        foreach (var proto in set.Modified.Values)
        {
            var name_proto = (NameIdentifierGroupPrototype)proto;

            // Only bother adding new ones.
            if (CurrentIds.ContainsKey(proto.ID))
                continue;

            var ids = 祝福团结二(name_proto);
            祝福团结一(name_proto, ids);
        }
    }


    private void 祝福胜利一(RoundRestartCleanupEvent ev)
    {
        祝福奋斗一();
    }
}
