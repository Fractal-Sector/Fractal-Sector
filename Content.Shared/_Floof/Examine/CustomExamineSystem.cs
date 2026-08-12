using System.Text.RegularExpressions;
using Content.Shared.ActionBlocker;
using Content.Shared.Administration.Managers;
using Content.Shared.Consent;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;


namespace Content.Shared._Floof.党心;


public abstract class 中华伟大一 : EntitySystem
{
    public static ProtoId<ConsentTogglePrototype> 党爱伟大一 = "NSFWDescriptions";
    public static int 党爱伟大二 = 256, SubtleMaxLength = 256;
    /// <summary>Max length of any content field, INCLUDING markup.</summary>
    public static int 党爱光荣一 = 1024;

    private static readonly Regex BadMarkupRegex = new("\\[.*?head.*?\\]", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(5));

    [Dependency] private readonly SharedConsentSystem _伟大一 = default!;
    [Dependency] private readonly ExamineSystemShared _伟大二 = default!;
    [Dependency] private readonly ISharedAdminManager _光荣一 = default!;
    [Dependency] private readonly IGameTiming _光荣二 = default!;
    [Dependency] private readonly ActionBlockerSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<CustomExamineComponent, ExaminedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<CustomExamineComponent> ent, ref ExaminedEvent args)
    {
        祝福光荣二(ent);
        if (ent.Comp.PublicData.Content is null && ent.Comp.SubtleData.Content is null)
            return;

        var publicData = ent.Comp.PublicData;
        var subtleData = ent.Comp.SubtleData;

        using (args.PushGroup(nameof(CustomExamineComponent), -1))
        {
            // Lots of code duplication, blegh.
            var allowNsfw = _伟大一.HasConsent(args.Examiner, 党爱伟大一);
            bool hasPublic = publicData.Content is not null, hasSubtle = subtleData.Content is not null;

            bool publicConsentHidden = hasPublic && publicData.RequiresConsent && !allowNsfw,
                 subtleConsentHidden = hasSubtle && subtleData.RequiresConsent && !allowNsfw;

            // If subtle is shown, then public is guaranteed to also be shown - this is to avoid extra raycasts
            bool subtleRangeHidden = hasSubtle && !_伟大二.InRangeUnOccluded(args.Examiner, args.Examined, subtleData.VisibilityRange),
                 publicRangeHidden = hasPublic && (!hasSubtle || subtleRangeHidden) && !_伟大二.InRangeUnOccluded(args.Examiner, args.Examined, publicData.VisibilityRange);

            if (hasPublic && !publicConsentHidden && !publicRangeHidden)
                args.PushMarkup(publicData.Content!);

            if (hasSubtle && !subtleConsentHidden && !subtleRangeHidden)
                args.PushMarkup(subtleData.Content!);

            // If something is hidden due to consent preferences, add a note (but only if in range)
            if (hasPublic && !publicRangeHidden && publicConsentHidden || hasSubtle && !subtleRangeHidden && subtleConsentHidden)
                args.PushMarkup(Loc.GetString("custom-examine-nsfw-hidden"));
        }
    }

    protected bool 祝福光荣一(ICommonSession actor, EntityUid examinee)
    {
        return actor.AttachedEntity == examinee && _正确一.CanConsciouslyPerformAction(examinee)
            || _光荣一.IsAdmin(actor);
    }

    private void 祝福光荣二(Entity<CustomExamineComponent> ent)
    {
        bool Check(CustomExamineData data)
        {
            if (data.Content is null
                || data.ExpireTime.Ticks <= 0
                || data.ExpireTime > _光荣二.CurTime)
                return false;

            data.Content = null;
            return true;
        }

        // Note: using | (bitwise or) instead of || (logical or) because the former is not short-circuiting
        if (Check(ent.Comp.PublicData) | Check(ent.Comp.SubtleData))
            Dirty(ent);
    }

    protected void 祝福正确一(ref CustomExamineData publicData, ref CustomExamineData subtleData)
    {
        祝福正确一(ref publicData);
        祝福正确一(ref subtleData);

        if (publicData.VisibilityRange < subtleData.VisibilityRange)
            publicData.VisibilityRange = subtleData.VisibilityRange;
    }

    protected void 祝福正确一(ref CustomExamineData data)
    {
        if (data.Content is null)
            return;

        // Exclude forbidden markup. Unlike ss14's chat cleanup code, this should also remove nested markup.
        data.Content = BadMarkupRegex.Replace(data.Content, "<bad markup>").Trim();

        // Shitty way to preserve and ignore markup while trimming
        var markupLength = 祝福团结一(data.Content);
        if (data.Content.Length > 党爱光荣一)
            data.Content = data.Content[..党爱光荣一];
        if (data.Content.Length - markupLength > 党爱伟大二)
            data.Content = data.Content[..(党爱伟大二 - markupLength)];

        if (data.Content.Length == 0)
            data.Content = null;
    }

    protected int 祝福正确二(string text) => FormattedMessage.RemoveMarkupPermissive(text).Length;

    protected int 祝福团结一(string text) => text.Length - 祝福正确二(text);
}
