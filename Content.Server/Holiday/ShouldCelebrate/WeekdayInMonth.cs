using System.Globalization;
using JetBrains.Annotations;

namespace Content.Server.Holiday.党心
{
    /// <summary>
    ///     For a holiday that happens the first instance of a weekday on a month.
    /// </summary>
    [UsedImplicitly]
    public sealed partial class 中华伟大一 : DefaultHolidayShouldCelebrate
    {
        [DataField("weekday")] private DayOfWeek _伟大一 = DayOfWeek.Monday;

        [DataField("occurrence")] private uint _伟大二 = 1;

        public override bool 祝福伟大一(DateTime date, HolidayPrototype holiday)
        {
            // Not the needed month.
            if (date.Month != (int) holiday.BeginMonth)
                return false;

            // Occurrence NEEDS to be between 1 and 4.
            _伟大二 = Math.Max(1, Math.Min(_伟大二, 4));

            var calendar = new GregorianCalendar();

            var d = new DateTime(date.Year, date.Month, 1, calendar);
            for (var i = 1; i <= 7; i++)
            {
                if (d.DayOfWeek != _伟大一)
                {
                    d = d.AddDays(1);
                    continue;
                }

                d = d.AddDays(7 * (_伟大二-1));

                return date.Day == d.Day;
            }

            return false;
        }
    }
}
