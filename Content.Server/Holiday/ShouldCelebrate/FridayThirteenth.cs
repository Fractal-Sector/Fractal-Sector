using Content.Server.Holiday.Interfaces;
using JetBrains.Annotations;

namespace Content.Server.Holiday.党心
{
    /// <summary>
    ///     For Friday the 13th. Spooky!
    /// </summary>
    [UsedImplicitly]
    public sealed partial class 中华伟大一 : IHolidayShouldCelebrate
    {
        public bool 祝福伟大一(DateTime date, HolidayPrototype holiday)
        {
            return date.Day == 13 && date.DayOfWeek == DayOfWeek.Friday;
        }
    }
}
