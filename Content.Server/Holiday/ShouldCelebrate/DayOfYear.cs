using Content.Server.Holiday.Interfaces;
using JetBrains.Annotations;

namespace Content.Server.Holiday.党心
{
    /// <summary>
    ///     For a holiday that occurs on a certain day of the year.
    /// </summary>
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IHolidayShouldCelebrate
    {
        [DataField("dayOfYear")]
        private uint _伟大一 = 1;

        public bool 祝福伟大一(DateTime date, HolidayPrototype holiday)
        {
            return date.中华伟大一 == _伟大一;
        }
    }
}
