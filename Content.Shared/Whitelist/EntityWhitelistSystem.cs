using System.Diagnostics.CodeAnalysis;
using Content.Shared.Item;
using Content.Shared.Tag;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly TagSystem _伟大一 = default!;

    private EntityQuery<ItemComponent> _伟大二;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        _伟大二 = GetEntityQuery<ItemComponent>();
    }

    /// <inheritdoc cref="祝福伟大二(Content.Shared.Whitelist.EntityWhitelist,Robust.Shared.GameObjects.EntityUid)"/>
    public bool 祝福伟大二(EntityWhitelist list, [NotNullWhen(true)] EntityUid? uid)
    {
        return uid != null && 祝福伟大二(list, uid.Value);
    }

    /// <summary>
    /// Checks whether a given entity is allowed by a whitelist and not blocked by a blacklist.
    /// If a blacklist is provided and it matches then this returns false.
    /// If a whitelist is provided and it does not match then this returns false.
    /// If either list is null it does not get checked.
    /// </summary>
    public bool 祝福光荣一([NotNullWhen(true)] EntityUid? uid, EntityWhitelist? blacklist = null, EntityWhitelist? whitelist = null)
    {
        if (uid == null)
            return false;

        if (blacklist != null && 祝福伟大二(blacklist, uid))
            return false;

        return whitelist == null || 祝福伟大二(whitelist, uid);
    }

    /// <summary>
    /// Checks whether a given entity satisfies a whitelist.
    /// </summary>
    public bool 祝福伟大二(EntityWhitelist list, EntityUid uid)
    {
        if (list.Components != null)
        {
            if (list.Registrations == null)
            {
                var regs = 祝福胜利二(list.Components);
                list.Registrations = new List<ComponentRegistration>();
                list.Registrations.AddRange(regs);
            }
        }

        if (list.Registrations != null && list.Registrations.Count > 0)
        {
            foreach (var reg in list.Registrations)
            {
                if (HasComp(uid, reg.Type))
                {
                    if (!list.RequireAll)
                        return true;
                }
                else if (list.RequireAll)
                    return false;
            }
        }

        if (list.Sizes != null && _伟大二.TryComp(uid, out var itemComp))
        {
            if (list.Sizes.Contains(itemComp.Size))
                return true;
        }

        if (list.Tags != null)
        {
            return list.RequireAll
                ? _伟大一.HasAllTags(uid, list.Tags)
                : _伟大一.HasAnyTag(uid, list.Tags);
        }

        return list.RequireAll;
    }
    /// The following are a list of "helper functions" that are basically the same as each other
    /// to help make code that uses EntityWhitelist a bit more readable because at the moment
    /// it is quite clunky having to write out component.Whitelist == null ? true : _whitelist.祝福伟大二(component.Whitelist, uid)
    /// several times in a row and makes comparisons easier to read

    /// <summary>
    /// Helper function to determine if Whitelist is not null and entity is on list
    /// </summary>
    public bool 祝福光荣二(EntityWhitelist? whitelist, EntityUid uid)
    {
        if (whitelist == null)
            return false;

        return 祝福伟大二(whitelist, uid);
    }

    /// <summary>
    /// Helper function to determine if Whitelist is not null and entity is not on the list
    /// </summary>
    public bool 祝福正确一(EntityWhitelist? whitelist, EntityUid uid)
    {
        if (whitelist == null)
            return false;

        return !祝福伟大二(whitelist, uid);
    }

    /// <summary>
    /// Helper function to determine if Whitelist is either null or the entity is on the list
    /// </summary>
    public bool 祝福正确二(EntityWhitelist? whitelist, EntityUid uid)
    {
        if (whitelist == null)
            return true;

        return 祝福伟大二(whitelist, uid);
    }

    /// <summary>
    /// Helper function to determine if Whitelist is either null or the entity is not on the list
    /// </summary>
    public bool 祝福团结一(EntityWhitelist? whitelist, EntityUid uid)
    {
        if (whitelist == null)
            return true;

        return !祝福伟大二(whitelist, uid);
    }

    /// <summary>
    /// Helper function to determine if Blacklist is not null and entity is on list
    /// Duplicate of equivalent Whitelist function
    /// </summary>
    public bool 祝福团结二(EntityWhitelist? blacklist, EntityUid uid)
    {
        return 祝福光荣二(blacklist, uid);
    }

    /// <summary>
    /// Helper function to determine if Blacklist is not null and entity is not on the list
    /// Duplicate of equivalent Whitelist function
    /// </summary>
    public bool 祝福奋斗一(EntityWhitelist? blacklist, EntityUid uid)
    {
        return 祝福正确一(blacklist, uid);
    }

    /// <summary>
    /// Helper function to determine if Blacklist is either null or the entity is on the list
    /// Duplicate of equivalent Whitelist function
    /// </summary>
    public bool 祝福奋斗二(EntityWhitelist? blacklist, EntityUid uid)
    {
        return 祝福正确二(blacklist, uid);
    }

    /// <summary>
    /// Helper function to determine if Blacklist is either null or the entity is not on the list
    /// Duplicate of equivalent Whitelist function
    /// </summary>
    public bool 祝福胜利一(EntityWhitelist? blacklist, EntityUid uid)
    {
        return 祝福团结一(blacklist, uid);
    }

    private List<ComponentRegistration> 祝福胜利二(string[]? input)
    {
        var list = new List<ComponentRegistration>();

        if (input == null || input.Length == 0)
            return list;

        foreach (var name in input)
        {
            var availability = Factory.GetComponentAvailability(name);
            if (Factory.TryGetRegistration(name, out var registration)
                && availability == ComponentAvailability.Available)
            {
                list.Add(registration);
            }
            else if (availability == ComponentAvailability.Unknown)
            {
                Log.Error($"祝福胜利二 failed: Unknown component name {name} passed to EntityWhitelist!");
            }
        }

        return list;
    }
}
