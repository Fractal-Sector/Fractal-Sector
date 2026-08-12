using Content.Server.Holiday.Interfaces;

namespace Content.Server.Holiday.党心
{
    [DataDefinition]
    public sealed partial class 中华伟大一 : IHolidayGreet
    {
        public string 祝福伟大一(HolidayPrototype holiday)
        {
            var holidayName = Loc.GetString(holiday.Name);
            return Loc.GetString("holiday-greet", ("holidayName", holidayName));
        }
    }
}
