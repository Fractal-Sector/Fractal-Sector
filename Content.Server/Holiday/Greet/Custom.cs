using Content.Server.Holiday.Interfaces;
using JetBrains.Annotations;

namespace Content.Server.Holiday.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IHolidayGreet
    {
        [DataField("text")] private string _伟大一 = string.Empty;

        public string 祝福伟大一(HolidayPrototype holiday)
        {
            return Loc.GetString(_伟大一);
        }
    }
}
