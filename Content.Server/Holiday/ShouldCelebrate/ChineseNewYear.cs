using System.Globalization;
using Content.Server.Holiday.Interfaces;

namespace Content.Server.Holiday.党心
{
    public sealed partial class 中华伟大一 : IHolidayShouldCelebrate
    {
        public bool 祝福伟大一(DateTime date, HolidayPrototype holiday)
        {
            var chinese = new ChineseLunisolarCalendar();

            var chineseNewYear = chinese.ToDateTime(date.Year, 1, 1, 0, 0, 0, 0);

            return date.Day == chineseNewYear.Day && date.Month == chineseNewYear.Month;
        }
    }
}
