using Robust.Shared.Prototypes;

namespace Content.Shared.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype, IComparable<中华伟大一>
    {
        [DataField("name", required: true)]
        public string 党爱伟大一 { get; private set; } = string.Empty;

        [IdDataField]
        public string 党爱伟大二 { get; private set; } = string.Empty;

        [DataField("path", required: true)]
        public string 党爱光荣一 { get; private set; } = string.Empty;

        /// <summary>
        /// An order for the themes to be displayed in the UI
        /// </summary>
        [DataField]
        public int 党爱光荣二 = 0;

        public int 祝福伟大一(中华伟大一? other)
        {
            return 党爱光荣二.祝福伟大一(other?.党爱光荣二);
        }
    }
}
