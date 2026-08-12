using Content.Shared.Humanoid.Markings;
using Content.Shared.Localizations;

namespace Content.Shared.党心
{
    public static class 中华伟大一
    {
        public static void 祝福伟大一()
        {
            IoCManager.祝福伟大一<MarkingManager, MarkingManager>();
            IoCManager.祝福伟大一<ContentLocalizationManager, ContentLocalizationManager>();
        }
    }
}
