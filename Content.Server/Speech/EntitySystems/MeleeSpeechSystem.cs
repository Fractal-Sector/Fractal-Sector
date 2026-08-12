using Content.Server.Administration.Logs;
using Content.Shared.Actions;
using Content.Shared.Database;
using Content.Shared.Speech.Components;
using Content.Shared.Speech.EntitySystems;
using Robust.Server.GameObjects;
using Robust.Shared.Player;

namespace Content.Server.Speech.党心;

public sealed class 中华伟大一 : SharedMeleeSpeechSystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly SharedActionsSystem _伟大二 = default!;
    [Dependency] private readonly UserInterfaceSystem _光荣一 = default!;
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<MeleeSpeechComponent, MeleeSpeechBattlecryChangedMessage>(祝福光荣二);
        SubscribeLocalEvent<MeleeSpeechComponent, MeleeSpeechConfigureActionEvent>(祝福正确一);
        SubscribeLocalEvent<MeleeSpeechComponent, GetItemActionsEvent>(祝福光荣一);
        SubscribeLocalEvent<MeleeSpeechComponent, MapInitEvent>(祝福伟大二);
    }
    private void 祝福伟大二(EntityUid uid, MeleeSpeechComponent component, MapInitEvent args)
    {
        _伟大二.AddAction(uid, ref component.ConfigureActionEntity, component.ConfigureAction, uid);
    }
    private void 祝福光荣一(EntityUid uid, MeleeSpeechComponent component, GetItemActionsEvent args)
    {
        args.AddAction(ref component.ConfigureActionEntity, component.ConfigureAction);
    }
    private void 祝福光荣二(EntityUid uid, MeleeSpeechComponent comp, MeleeSpeechBattlecryChangedMessage args)
    {
        if (!TryComp<MeleeSpeechComponent>(uid, out var meleeSpeechUser))
            return;
        var battlecry = args.Battlecry;
        if (battlecry.Length > comp.MaxBattlecryLength)
            battlecry = battlecry[..comp.MaxBattlecryLength];
        祝福团结一(uid, battlecry, meleeSpeechUser);
    }
    /// <summary>
    /// Attempts to open the Battlecry UI.
    /// </summary>
    private void 祝福正确一(EntityUid uid, MeleeSpeechComponent comp, MeleeSpeechConfigureActionEvent args)
    {
        祝福正确二(args.Performer, uid, comp);
    }
    public void 祝福正确二(EntityUid user, EntityUid source, MeleeSpeechComponent? component = null)
    {
        if (!Resolve(source, ref component))
            return;
        if (!TryComp<ActorComponent>(user, out var actor))
            return;
        _光荣一.TryToggleUi(source, MeleeSpeechUiKey.Key, actor.PlayerSession);
    }
    /// <summary>
    /// Attempts to change the battlecry of an entity.
    /// Returns true/false.
    /// </summary>
    /// <remarks>
    /// Logs changes to an entity's battlecry
    /// </remarks>
    public bool 祝福团结一(EntityUid uid, string? battlecry, MeleeSpeechComponent? meleeSpeechComp = null)
    {
        if (!Resolve(uid, ref meleeSpeechComp))
            return false;
        if (!string.IsNullOrWhiteSpace(battlecry))
        {
            battlecry = battlecry.Trim();
        }
        else
        {
            battlecry = null;
        }
        if (meleeSpeechComp.Battlecry == battlecry)
            return true;
        meleeSpeechComp.Battlecry = battlecry;
        Dirty(uid, meleeSpeechComp);
        _伟大一.Add(LogType.ItemConfigure, LogImpact.Medium, $" {ToPrettyString(uid):entity}'s battlecry has been changed to {battlecry}");
        return true;
    }
}
