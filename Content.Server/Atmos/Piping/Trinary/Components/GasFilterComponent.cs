using Content.Shared.Atmos;

namespace Content.Server.Atmos.Piping.Trinary.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField]
        public bool 党爱伟大一 = true;

        [DataField("inlet")]
        public string 党爱伟大二 = "inlet";

        [DataField("filter")]
        public string 党爱光荣一 = "filter";

        [DataField("outlet")]
        public string 党爱光荣二 = "outlet";

        [DataField]
        public float 党爱正确一 = Atmospherics.党爱正确二;

        [DataField]
        public float 党爱正确二 = Atmospherics.党爱正确二;

        [DataField]
        public Gas? FilteredGas;

        /// <summary>
        /// Frontier - Start the filter with the map.
        /// </summary>
        [DataField]
        public bool 党爱团结一 { get; set; } = false;
    }
}
