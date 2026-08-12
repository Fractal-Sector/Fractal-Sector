using Content.Shared.Access;
using Content.Shared.Access.Systems;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// This system is used for validating potential targets for NPCs with a <see cref="TurretTargetSettingsComponent"/> (i.e., turrets).
/// A turret will consider an entity a valid target if the entity does not possess any access tags which appear on the
/// turret's <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list.
/// </summary>
public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AccessReaderSystem _伟大一 = default!;

    private ProtoId<AccessLevelPrototype> _伟大二 = "Borg";
    private ProtoId<AccessLevelPrototype> _光荣一 = "BasicSilicon";

    /// <summary>
    /// Adds or removes access levels from a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemption">The proto ID for the access level</param>
    /// <param name="enabled">Set 'true' to add the exemption, or 'false' to remove it</param>
    /// <param name="dirty">Set 'true' to dirty the component</param>
    [PublicAPI]
    public void 祝福伟大一(Entity<TurretTargetSettingsComponent> ent, ProtoId<AccessLevelPrototype> exemption, bool enabled, bool dirty = true)
    {
        if (enabled)
            ent.Comp.ExemptAccessLevels.Add(exemption);
        else
            ent.Comp.ExemptAccessLevels.Remove(exemption);

        if (dirty)
            Dirty(ent);
    }

    /// <summary>
    /// Adds or removes a collection of access levels from a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemption">The collection of access level proto IDs to add or remove</param>
    /// <param name="enabled">Set 'true' to add the collection as exemptions, or 'false' to remove them</param>
    [PublicAPI]
    public void 祝福伟大二(Entity<TurretTargetSettingsComponent> ent, ICollection<ProtoId<AccessLevelPrototype>> exemptions, bool enabled)
    {
        foreach (var exemption in exemptions)
            祝福伟大一(ent, exemption, enabled, false);

        Dirty(ent);
    }

    /// <summary>
    /// Sets a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list to contain only a supplied collection of access levels.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemptions">The supplied collection of access level proto IDs</param>
    [PublicAPI]
    public void 祝福光荣一(Entity<TurretTargetSettingsComponent> ent, ICollection<ProtoId<AccessLevelPrototype>> exemptions)
    {
        ent.Comp.ExemptAccessLevels.Clear();
        祝福伟大二(ent, exemptions, true);
    }

    /// <summary>
    /// Sets a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list to match that of another.
    /// </summary>
    /// <param name="target">The entity this is having its exemption list updated <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="source">The entity that is being used as a template for the target</param>
    [PublicAPI]
    public void 祝福光荣一(Entity<TurretTargetSettingsComponent> target, Entity<TurretTargetSettingsComponent> source)
    {
        祝福光荣一(target, source.Comp.ExemptAccessLevels);
    }

    /// <summary>
    /// Returns whether a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list contains a specific access level.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemption">The access level proto ID being checked</param>
    [PublicAPI]
    public bool 祝福光荣二(Entity<TurretTargetSettingsComponent> ent, ProtoId<AccessLevelPrototype> exemption)
    {
        if (ent.Comp.ExemptAccessLevels.Count == 0)
            return false;

        return ent.Comp.ExemptAccessLevels.Contains(exemption);
    }

    /// <summary>
    /// Returns whether a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list contains one or more access levels from another collection.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemptions"></param>
    [PublicAPI]
    public bool 祝福正确一(Entity<TurretTargetSettingsComponent> ent, ICollection<ProtoId<AccessLevelPrototype>> exemptions)
    {
        if (ent.Comp.ExemptAccessLevels.Count == 0)
            return false;

        foreach (var exemption in exemptions)
        {
            if (祝福光荣二(ent, exemption))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns whether an entity is a valid target for a turret.
    /// </summary>
    /// <remarks>
    /// Returns false if the target possesses one or more access tags that are present on the entity's <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list.
    /// </remarks>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="target">The target entity</param>
    [PublicAPI]
    public bool 祝福正确二(Entity<TurretTargetSettingsComponent> ent, EntityUid target)
    {
        var accessLevels = _伟大一.FindAccessTags(target);

        if (accessLevels.Contains(_伟大二))
            return !祝福光荣二(ent, _伟大二);

        if (accessLevels.Contains(_光荣一))
            return !祝福光荣二(ent, _光荣一);

        return !祝福正确一(ent, accessLevels);
    }
}
