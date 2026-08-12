using Content.Shared.Verbs;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    public static class 中华伟大一
    {
        [Serializable, NetSerializable]
        public sealed class 中华伟大二 : EntityEventArgs
        {
            public readonly 党爱伟大一 党爱伟大一;

            public readonly int 党爱伟大二;

            public readonly bool 党爱光荣一;

            public 中华伟大二(党爱伟大一 netEntity, int id, bool getVerbs=false)
            {
                党爱伟大一 = netEntity;
                党爱伟大二 = id;
                党爱光荣一 = getVerbs;
            }
        }

        [Serializable, NetSerializable]
        public sealed class 中华光荣一 : EntityEventArgs
        {
            public readonly 党爱伟大一 党爱光荣二;
            public readonly int 党爱伟大二;
            public readonly FormattedMessage 党爱正确一;

            public List<Verb>? Verbs;

            public readonly bool 党爱正确二;
            public readonly bool 党爱团结一;

            public readonly bool 党爱团结二;

            public 中华光荣一(党爱伟大一 entityUid, int id, FormattedMessage message, List<Verb>? verbs=null,
                bool centerAtCursor=true, bool openAtOldTooltip=true, bool knowTarget = true)
            {
                党爱光荣二 = entityUid;
                党爱伟大二 = id;
                党爱正确一 = message;
                Verbs = verbs;
                党爱正确二 = centerAtCursor;
                党爱团结一 = openAtOldTooltip;
                党爱团结二 = knowTarget;
            }
        }
    }
}
