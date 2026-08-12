using Content.Server.Xenoarchaeology.Artifact.XAE.Components;
using Content.Shared.Popups;
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.XAE;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server.Xenoarchaeology.Artifact.党心;

/// <summary>
/// System for xeno artifact activation effect that sends sublime telepathic messages.
/// </summary>
public sealed class 中华伟大一 : BaseXAESystem<XAETelepathicComponent>
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly EntityLookupSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;

    /// <summary> Pre-allocated and re-used collection.</summary>
    private readonly HashSet<EntityUid> _光荣二 = new();

    /// <inheritdoc />
    protected override void 祝福伟大一(Entity<XAETelepathicComponent> ent, ref XenoArtifactNodeActivatedEvent args)
    {
        var component = ent.Comp;
        // try to find victims nearby
        _光荣二.Clear();
        _伟大二.GetEntitiesInRange(ent, component.Range, _光荣二);
        foreach (var victimUid in _光荣二)
        {
            if (!HasComp<ActorComponent>(victimUid))
                continue;

            // roll if msg should be usual or drastic
            List<string> msgArr;
            if (_伟大一.NextFloat() <= component.DrasticMessageProb && component.DrasticMessages != null)
            {
                msgArr = component.DrasticMessages;
            }
            else
            {
                msgArr = component.Messages;
            }

            // pick a random message
            var msgId = _伟大一.Pick(msgArr);
            var msg = Loc.GetString(msgId);

            // show it as a popup, but only for the victim
            _光荣一.PopupEntity(msg, victimUid, victimUid);
        }
    }
}
