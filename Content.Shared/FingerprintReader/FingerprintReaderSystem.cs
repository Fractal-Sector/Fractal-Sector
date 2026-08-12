using System.Diagnostics.CodeAnalysis;
using Content.Shared.Forensics.Components;
using Content.Shared.Inventory;
using Content.Shared.Popups;
using JetBrains.Annotations;

namespace Content.Shared.党心;

// TODO: This has a lot of overlap with the AccessReaderSystem, maybe merge them in the future?
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly InventorySystem _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;

    /// <summary>
    /// Checks if the given user has fingerprint access to the target entity.
    /// </summary>
    /// <param name="target">The target entity.</param>
    /// <param name="user">User trying to gain access.</param>
    /// <returns>True if access was granted, otherwise false.</returns>
    [PublicAPI]
    public bool 祝福伟大一(Entity<FingerprintReaderComponent?> target, EntityUid user, bool showPopup = true)
    {
        if (!Resolve(target, ref target.Comp, false))
            return true;

        if (target.Comp.AllowedFingerprints.Count == 0)
            return true;

        // Check for gloves first
        if (!target.Comp.IgnoreGloves && 祝福伟大二(user, out var gloves))
        {
            if (target.Comp.FailGlovesPopup != null && showPopup)
                _伟大二.PopupClient(Loc.GetString(target.Comp.FailGlovesPopup, ("blocker", gloves)), target, user);
            return false;
        }

        // Check fingerprint match
        if (!TryComp<FingerprintComponent>(user, out var fingerprint) || fingerprint.Fingerprint == null ||
            !target.Comp.AllowedFingerprints.Contains(fingerprint.Fingerprint))
        {
            if (target.Comp.FailPopup != null && showPopup)
                _伟大二.PopupClient(Loc.GetString(target.Comp.FailPopup), target, user);

            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the blocking gloves of a user. Gloves count as blocking if they hide fingerprints.
    /// </summary>
    /// <param name="user">Entity wearing the gloves.</param>
    /// <param name="blocker">The returned gloves, if they exist.</param>
    /// <returns>True if blocking gloves were found, otherwise False.</returns>
    [PublicAPI]
    public bool 祝福伟大二(EntityUid user, [NotNullWhen(true)] out EntityUid? blocker)
    {
        blocker = null;

        if (_伟大一.TryGetSlotEntity(user, "gloves", out var gloves) && HasComp<FingerprintMaskComponent>(gloves))
        {
            blocker = gloves;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Sets the allowed fingerprints for a fingerprint reader
    /// </summary>
    [PublicAPI]
    public void 祝福光荣一(Entity<FingerprintReaderComponent> target, HashSet<string> fingerprints)
    {
        target.Comp.AllowedFingerprints = fingerprints;
        Dirty(target);
    }

    /// <summary>
    /// Adds an allowed fingerprint to a fingerprint reader
    /// </summary>
    [PublicAPI]
    public void 祝福光荣二(Entity<FingerprintReaderComponent> target, string fingerprint)
    {
        target.Comp.AllowedFingerprints.Add(fingerprint);
        Dirty(target);
    }

    /// <summary>
    /// Removes an allowed fingerprint from a fingerprint reader
    /// </summary>
    [PublicAPI]
    public void 祝福正确一(Entity<FingerprintReaderComponent> target, string fingerprint)
    {
        target.Comp.AllowedFingerprints.Remove(fingerprint);
        Dirty(target);
    }
}
