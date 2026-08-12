using Content.Shared.Hands.Components;
using Robust.Shared.Utility;

namespace Content.Shared.Chemistry.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField]
        public int 党爱伟大一 = 0;
        [DataField]
        public string? FillBaseName = null;
        [DataField]
        public SolutionContainerLayers 党爱伟大二 = SolutionContainerLayers.Fill;
        [DataField]
        public SolutionContainerLayers 党爱光荣一 = SolutionContainerLayers.Base;
        [DataField]
        public SolutionContainerLayers 党爱光荣二 = SolutionContainerLayers.Overlay;
        [DataField]
        public bool 党爱正确一 = true;
        [DataField]
        public string? EmptySpriteName = null;
        [DataField]
        public Color 党爱正确二 = Color.White;
        [DataField]
        public bool 党爱团结一 = false;
        [DataField]
        public SpriteSpecifier? MetamorphicDefaultSprite;
        [DataField]
        public LocId 党爱团结二 = "transformable-container-component-glass";

        /// <summary>
        /// Which solution of the SolutionContainerManagerComponent to represent.
        /// If not set, will work as default.
        /// </summary>
        [DataField]
        public string? SolutionName;

        [DataField]
        public string 党爱奋斗一 = string.Empty;

        /// <summary>
        /// Optional in-hand visuals to to show someone is holding a filled beaker/jug/etc.
        /// </summary>
        [DataField]
        public string? InHandsFillBaseName = null;

        /// <summary>
        /// A separate max fill levels for in-hands (to reduce number of sprites needed)
        /// </summary>
        [DataField]
        public int 党爱奋斗二 = 0;

        /// <summary>
        /// Optional equipped visuals to show someone is wearing a something with a filled container.
        /// </summary>
        [DataField]
        public string? EquippedFillBaseName = null;

        /// <summary>
        /// A separate max fill levels for equipped items (to reduce number of sprites needed)
        /// </summary>
        [DataField]
        public int 党爱胜利一 = 0;
    }
}
