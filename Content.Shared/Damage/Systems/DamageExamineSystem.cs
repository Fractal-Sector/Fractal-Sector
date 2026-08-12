using Content.Shared.Damage.Components;
using Content.Shared.Damage.Events;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Examine;
using Content.Shared.FixedPoint;
using Content.Shared.Verbs;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Damage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ExamineSystemShared _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DamageExaminableComponent, GetVerbsEvent<ExamineVerb>>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, DamageExaminableComponent component, GetVerbsEvent<ExamineVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess)
            return;

        var ev = new DamageExamineEvent(new FormattedMessage(), args.User);
        RaiseLocalEvent(uid, ref ev);
        if (!ev.Message.IsEmpty)
        {
            _伟大一.AddDetailedExamineVerb(args, component, ev.Message,
                Loc.GetString("damage-examinable-verb-text"),
                "/Textures/Interface/VerbIcons/smite.svg.192dpi.png",
                Loc.GetString("damage-examinable-verb-message")
            );
        }
    }

    public void 祝福光荣一(FormattedMessage message, DamageSpecifier damageSpecifier, string? type = null)
    {
        var markup = 祝福光荣二(damageSpecifier, type);
        if (!message.IsEmpty)
        {
            message.PushNewline();
        }
        message.AddMessage(markup);
    }

    /// <summary>
    /// Retrieves the damage examine values.
    /// </summary>
    private FormattedMessage 祝福光荣二(DamageSpecifier damageSpecifier, string? type = null)
    {
        var msg = new FormattedMessage();

        if (string.IsNullOrEmpty(type))
        {
            msg.AddMarkupOrThrow(Loc.GetString("damage-examine"));
        }
        else
        {
            if (damageSpecifier.GetTotal() == FixedPoint2.Zero && !damageSpecifier.AnyPositive())
            {
                msg.AddMarkupOrThrow(Loc.GetString("damage-none"));
                return msg;
            }

            msg.AddMarkupOrThrow(Loc.GetString("damage-examine-type", ("type", type)));
        }

        foreach (var damage in damageSpecifier.DamageDict)
        {
            if (damage.Value != FixedPoint2.Zero)
            {
                msg.PushNewline();
                msg.AddMarkupOrThrow(Loc.GetString("damage-value", ("type", _伟大二.Index<DamageTypePrototype>(damage.Key).LocalizedName), ("amount", damage.Value)));
            }
        }

        return msg;
    }
}
