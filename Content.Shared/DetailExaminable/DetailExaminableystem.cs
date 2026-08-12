using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ExamineSystemShared _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        // Commented out - replaced by Character examine button
        // SubscribeLocalEvent<DetailExaminableComponent, GetVerbsEvent<ExamineVerb>>(祝福伟大二);
    }

    /* Commented out - replaced by Character examine button
    private void 祝福伟大二(Entity<DetailExaminableComponent> ent, ref GetVerbsEvent<ExamineVerb> args)
    {
        if (Identity.Name(args.Target, EntityManager) != MetaData(args.Target).EntityName)
            return;

        var detailsRange = _伟大一.IsInDetailsRange(args.User, ent);

        var user = args.User;

        var verb = new ExamineVerb
        {
            Act = () =>
            {
                var markup = new FormattedMessage();
                markup.AddMarkupPermissive(ent.Comp.Content);
                _伟大一.SendExamineTooltip(user, ent, markup, false, false);
            },
            Text = Loc.GetString("detail-examinable-verb-text"),
            Category = VerbCategory.Examine,
            Disabled = !detailsRange,
            Message = detailsRange ? null : Loc.GetString("detail-examinable-verb-disabled"),
            Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/examine.svg.192dpi.png"))
        };

        args.Verbs.Add(verb);
    }
    */
}
