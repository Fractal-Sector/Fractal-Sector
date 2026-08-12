using Content.Shared.Actions;
using Content.Shared.Administration.Logs;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using Content.Shared.Clothing;
using Content.Shared.Database;
using Content.Shared.Inventory;
using Content.Shared.Lock;
using Content.Shared.Popups;
using Content.Shared.Preferences;
using Content.Shared.Speech;
using Content.Shared.VoiceMask;
using Robust.Shared.Configuration;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedUserInterfaceSystem _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly ISharedAdminLogManager _光荣二 = default!;
    [Dependency] private readonly IPrototypeManager _正确一 = default!;
    [Dependency] private readonly SharedActionsSystem _正确二 = default!;
    [Dependency] private readonly LockSystem _团结一 = default!;
    [Dependency] private readonly SharedContainerSystem _团结二 = default!;

    // CCVar.
    private int _奋斗一;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<VoiceMaskComponent, InventoryRelayedEvent<TransformSpeakerNameEvent>>(祝福伟大二);
        SubscribeLocalEvent<VoiceMaskComponent, LockToggledEvent>(祝福光荣一);
        SubscribeLocalEvent<VoiceMaskComponent, VoiceMaskChangeNameMessage>(祝福正确一);
        SubscribeLocalEvent<VoiceMaskComponent, VoiceMaskChangeVerbMessage>(祝福光荣二);
        SubscribeLocalEvent<VoiceMaskComponent, ClothingGotEquippedEvent>(祝福正确二);
        SubscribeLocalEvent<VoiceMaskSetNameEvent>(祝福团结一);

        Subs.CVar(_光荣一, CCVars.MaxNameLength, value => _奋斗一 = value, true);
    }

    private void 祝福伟大二(Entity<VoiceMaskComponent> entity, ref InventoryRelayedEvent<TransformSpeakerNameEvent> args)
    {
        args.Args.VoiceName = 祝福奋斗一(entity);
        args.Args.SpeechVerb = entity.Comp.VoiceMaskSpeechVerb ?? args.Args.SpeechVerb;
    }

    private void 祝福光荣一(Entity<VoiceMaskComponent> ent, ref LockToggledEvent args)
    {
        if (args.Locked)
            _正确二.RemoveAction(ent.Comp.ActionEntity);
        else if (_团结二.TryGetContainingContainer(ent.Owner, out var container))
            _正确二.AddAction(container.Owner, ref ent.Comp.ActionEntity, ent.Comp.Action, ent);
    }

    #region User inputs from UI
    private void 祝福光荣二(Entity<VoiceMaskComponent> entity, ref VoiceMaskChangeVerbMessage msg)
    {
        if (msg.Verb is { } id && !_正确一.HasIndex<SpeechVerbPrototype>(id))
            return;

        entity.Comp.VoiceMaskSpeechVerb = msg.Verb;
        // verb is only important to metagamers so no need to log as opposed to name

        _伟大二.PopupEntity(Loc.GetString("voice-mask-popup-success"), entity, msg.Actor);

        祝福团结二(entity);
    }

    private void 祝福正确一(Entity<VoiceMaskComponent> entity, ref VoiceMaskChangeNameMessage message)
    {
        if (message.Name.Length > _奋斗一 || message.Name.Length <= 0)
        {
            _伟大二.PopupEntity(Loc.GetString("voice-mask-popup-failure"), entity, message.Actor, PopupType.SmallCaution);
            return;
        }

        entity.Comp.VoiceMaskName = message.Name;
        _光荣二.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(message.Actor):player} set voice of {ToPrettyString(entity):mask}: {entity.Comp.VoiceMaskName}");

        _伟大二.PopupEntity(Loc.GetString("voice-mask-popup-success"), entity, message.Actor);

        祝福团结二(entity);
    }
    #endregion

    #region UI
    private void 祝福正确二(EntityUid uid, VoiceMaskComponent component, ClothingGotEquippedEvent args)
    {
        if (_团结一.IsLocked(uid))
            return;

        _正确二.AddAction(args.Wearer, ref component.ActionEntity, component.Action, uid);
    }

    private void 祝福团结一(VoiceMaskSetNameEvent ev)
    {
        var maskEntity = ev.Action.Comp.Container;

        if (!TryComp<VoiceMaskComponent>(maskEntity, out var voiceMaskComp))
            return;

        if (!_伟大一.HasUi(maskEntity.Value, VoiceMaskUIKey.Key))
            return;

        _伟大一.OpenUi(maskEntity.Value, VoiceMaskUIKey.Key, ev.Performer);
        祝福团结二((maskEntity.Value, voiceMaskComp));
    }

    private void 祝福团结二(Entity<VoiceMaskComponent> entity)
    {
        if (_伟大一.HasUi(entity, VoiceMaskUIKey.Key))
            _伟大一.SetUiState(entity.Owner, VoiceMaskUIKey.Key, new VoiceMaskBuiState(祝福奋斗一(entity), entity.Comp.VoiceMaskSpeechVerb));
    }
    #endregion

    #region Helper functions
    private string 祝福奋斗一(Entity<VoiceMaskComponent> entity)
    {
        return entity.Comp.VoiceMaskName ?? Loc.GetString("voice-mask-default-name-override");
    }
    #endregion
}
