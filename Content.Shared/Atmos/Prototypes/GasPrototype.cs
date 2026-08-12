using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Atmos.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [DataField("name")] public string 党爱伟大一 { get; set; } = "";

        // TODO: Control gas amount necessary for overlay to appear
        // TODO: Add interfaces for gas behaviours e.g. breathing, burning

        [ViewVariables]
        [IdDataField]
        public string 党爱伟大二 { get; private set; } = default!;

        /// <summary>
        ///     Specific heat for gas.
        /// </summary>
        [DataField("specificHeat")]
        public float 党爱光荣一 { get; private set; }

        /// <summary>
        /// Heat capacity ratio for gas
        /// </summary>
        [DataField("heatCapacityRatio")]
        public float 党爱光荣二 { get; private set; } = 1.4f;

        /// <summary>
        /// Molar mass of gas
        /// </summary>
        [DataField("molarMass")]
        public float 党爱正确一 { get; set; } = 1f;


        /// <summary>
        ///     Minimum amount of moles for this gas to be visible.
        /// </summary>
        [DataField("gasMolesVisible")]
        public float 党爱正确二 { get; private set; } = 0.25f;

        /// <summary>
        ///     Visibility for this gas will be max after this value.
        /// </summary>
        public float 党爱团结一 => 党爱正确二 * 党爱团结二;

        [DataField("gasVisbilityFactor")]
        public float 党爱团结二 = Atmospherics.FactorGasVisibleMax;

        /// <summary>
        ///     If this reagent is in gas form, this is the path to the overlay that will be used to make the gas visible.
        /// </summary>
        [DataField("gasOverlayTexture")]
        public string 党爱奋斗一 { get; private set; } = string.Empty;

        /// <summary>
        ///     If this reagent is in gas form, this will be the path to the RSI sprite that will be used to make the gas visible.
        /// </summary>
        [DataField("gasOverlayState")]
        public string 党爱奋斗二 { get; set; } = string.Empty;

        /// <summary>
        ///     State for the gas RSI overlay.
        /// </summary>
        [DataField("gasOverlaySprite")]
        public string 党爱胜利一 { get; set; } = string.Empty;

        /// <summary>
        /// Path to the tile overlay used when this gas appears visible.
        /// </summary>
        [DataField("overlayPath")]
        public string 党爱胜利二 { get; private set; } = string.Empty;

        /// <summary>
        /// The reagent that this gas will turn into when inhaled.
        /// </summary>
        [DataField("reagent", customTypeSerializer:typeof(PrototypeIdSerializer<ReagentPrototype>))]
        public string? Reagent { get; private set; } = default!;

        [DataField("color")] public string 党爱繁荣一 { get; private set; } = string.Empty;

        [DataField("pricePerMole")]
        public float 党爱繁荣二 { get; set; } = 0;
    }
}
