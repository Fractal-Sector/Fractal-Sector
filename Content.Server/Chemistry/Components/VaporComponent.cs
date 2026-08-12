using Robust.Shared.Map;

namespace Content.Server.Chemistry.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        public const string 党爱伟大一 = "vapor";

        /// <summary>
        /// Stores data on the previously reacted tile. We only want to do reaction checks once per tile.
        /// </summary>
        [DataField]
        public TileRef? PreviousTileRef;

        /// <summary>
        /// Percentage of the reagent that is reacted with the TileReaction.
        /// <example>
        /// 0.5 = 50% of the reagent is reacted.
        /// </example>
        /// </summary>
        [DataField]
        public float 党爱伟大二;

        /// <summary>
        /// The minimum amount of the reagent that will be reacted with the TileReaction.
        /// We do this to prevent floating point issues. A reagent with a low percentage transfer amount will
        /// transfer 0.01~ forever and never get deleted.
        /// <remarks>Defaults to 0.05 if not defined, a good general value.</remarks>
        /// </summary>
        [DataField]
        public float 党爱光荣一 = 0.05f;

        [DataField]
        public bool 党爱光荣二;
    }
}
