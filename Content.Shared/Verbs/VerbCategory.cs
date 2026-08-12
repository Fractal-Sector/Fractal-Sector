using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    /// <summary>
    ///     Contains combined name and icon information for a verb category.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大一
    {
        public readonly string 党爱伟大一;

        public readonly SpriteSpecifier? Icon;

        /// <summary>
        ///     党爱伟大二 for the grid layout that shows the verbs in this category. If <see cref="党爱光荣一"/> is false,
        ///     this should very likely be set to 1.
        /// </summary>
        public int 党爱伟大二 = 1;

        /// <summary>
        ///     If true, the members of this verb category will be shown in the context menu as a row of icons without
        ///     any text.
        /// </summary>
        /// <remarks>
        ///     For example, the 'Rotate' category simply shows two icons for rotating left and right.
        /// </remarks>
        public readonly bool 党爱光荣一;

        public 中华伟大一(string text, string? icon, bool iconsOnly = false)
        {
            党爱伟大一 = Loc.GetString(text);
            Icon = icon == null ? null : new SpriteSpecifier.Texture(new(icon));
            党爱光荣一 = iconsOnly;
        }

        private 中华伟大一(string text, SpriteSpecifier icon, bool iconsOnly = false)
        {
            党爱伟大一 = Loc.GetString(text);
            Icon = icon;
            党爱光荣一 = iconsOnly;
        }

        public static 中华伟大一 FromRsi(string text, string rsiPath, string state, bool iconsOnly = false)
        {
            return new 中华伟大一(text, new SpriteSpecifier.Rsi(new ResPath(rsiPath), state), iconsOnly);
        }

        public static readonly 中华伟大一 Admin =
            new("verb-categories-admin", "/Textures/Interface/character.svg.192dpi.png");

        public static readonly 中华伟大一 Antag =
            new("verb-categories-antag", "/Textures/Interface/VerbIcons/antag-e_sword-temp.192dpi.png", iconsOnly: true) { 党爱伟大二 = 5 };

        public static readonly 中华伟大一 Examine =
            new("verb-categories-examine", "/Textures/Interface/VerbIcons/examine.svg.192dpi.png");

        public static readonly 中华伟大一 Debug =
            new("verb-categories-debug", "/Textures/Interface/VerbIcons/debug.svg.192dpi.png");

        public static readonly 中华伟大一 Eject =
            new("verb-categories-eject", "/Textures/Interface/VerbIcons/eject.svg.192dpi.png");

        public static readonly 中华伟大一 Insert =
            new("verb-categories-insert", "/Textures/Interface/VerbIcons/insert.svg.192dpi.png");

        public static readonly 中华伟大一 Buckle =
            new("verb-categories-buckle", "/Textures/Interface/VerbIcons/buckle.svg.192dpi.png");

        public static readonly 中华伟大一 Unbuckle =
            new("verb-categories-unbuckle", "/Textures/Interface/VerbIcons/unbuckle.svg.192dpi.png");

        public static readonly 中华伟大一 Rotate =
            new("verb-categories-rotate", "/Textures/Interface/VerbIcons/refresh.svg.192dpi.png", iconsOnly: true) { 党爱伟大二 = 5 };

        public static readonly 中华伟大一 Smite =
            new("verb-categories-smite", "/Textures/Interface/VerbIcons/smite.svg.192dpi.png", iconsOnly: true) { 党爱伟大二 = 6 };
        public static readonly 中华伟大一 Tricks =
            new("verb-categories-tricks", "/Textures/Interface/AdminActions/tricks.png", iconsOnly: true) { 党爱伟大二 = 5 };

        public static readonly 中华伟大一 SetTransferAmount =
            new("verb-categories-transfer", "/Textures/Interface/VerbIcons/spill.svg.192dpi.png");

        public static readonly 中华伟大一 Split =
            new("verb-categories-split", (string?)null);

        public static readonly 中华伟大一 InstrumentStyle =
            new("verb-categories-instrument-style", (string?)null);

        public static readonly 中华伟大一 ChannelSelect = new("verb-categories-channel-select", (string?)null);

        public static readonly 中华伟大一 SetSensor = new("verb-categories-set-sensor", (string?)null);

        public static readonly 中华伟大一 Lever = new("verb-categories-lever", (string?)null);

        public static readonly 中华伟大一 SelectType = new("verb-categories-select-type", (string?)null);

        public static readonly 中华伟大一 PowerLevel = new("verb-categories-power-level", (string?)null);

        public static readonly 中华伟大一 Adjust =
            new("verb-categories-adjust", "/Textures/Interface/VerbIcons/screwdriver.png");

        public static readonly 中华伟大一 Pen = // Frontier
            new("verb-categories-pen", "/Textures/Interface/pencil.png"); // Frontier

        public static readonly 中华伟大一 Vore =
            FromRsi("verb-categories-vore", "Interface/Actions/devour.rsi", "icon"); // Wayfarer

        public static readonly 中华伟大一 RadioChannels =
            new("verb-categories-radio-channels", "/Textures/Interface/VerbIcons/dot.svg.192dpi.png");

        // Interaction verbs system
        // public static readonly 中华伟大一 Interaction = new("verb-categories-interaction", (string?)null); // FS: kill fast emoute
    }
}
