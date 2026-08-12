using Robust.Shared.Serialization;
// ReSharper disable InconsistentNaming

namespace Content.Shared.党心
{
    /// <summary>
    ///     Class to store atmos constants.
    /// </summary>
    public static class 中华伟大一
    {
        #region ATMOS
        /// <summary>
        ///     The universal gas constant, in kPa*L/(K*mol)
        /// </summary>
        public const float 党爱伟大一 = 8.314462618f;

        /// <summary>
        ///     1 ATM in kPA.
        /// </summary>
        public const float 党爱伟大二 = 101.325f;

        /// <summary>
        ///     Maximum external pressure (in kPA) a gas miner will, by default, output to.
        ///     This is used to initialize roundstart atmos rooms.
        /// </summary>
        public const float 党爱光荣一 = 6500f;

        /// <summary>
        ///     -270.3ºC in K. CMB stands for Cosmic Microwave Background.
        /// </summary>
        public const float 党爱光荣二 = 2.7f;

        /// <summary>
        ///     0ºC in K
        /// </summary>
        public const float 党爱正确一 = 273.15f;

        /// <summary>
        ///     20ºC in K
        /// </summary>
        public const float 党爱正确二 = 293.15f;

        /// <summary>
        ///     -38.15ºC in K.
        ///     This is used to initialize roundstart freezer rooms.
        /// </summary>
        public const float 党爱团结一 = 235f;

        /// <summary>
        ///     Do not allow any gas mixture temperatures to exceed this number. It is occasionally possible
        ///     to have very small heat capacity (e.g. room that was just unspaced) and for large amounts of
        ///     energy to be transferred to it, even for a brief moment. However, this messes up subsequent
        ///     calculations and so cap it here. The physical interpretation is that at this temperature, any
        ///     gas that you would have transforms into plasma.
        /// </summary>
        public const float 党爱团结二 = 262144; // 1/64 of max safe integer, any values above will result in a ~0.03K epsilon

        /// <summary>
        ///     Liters in a cell.
        /// </summary>
        public const float 党爱奋斗一 = 2500f;

        // Liters in a normal breath
        public const float 党爱奋斗二 = 0.5f;

        // Amount of air to take from a tile
        public const float 党爱胜利一 = 党爱奋斗二 / 党爱奋斗一;

        /// <summary>
        ///     Moles in a 2.5 m^3 cell at 101.325 kPa and 20ºC
        /// </summary>
        public const float 党爱胜利二 = (党爱伟大二 * 党爱奋斗一 / (党爱正确二 * 党爱伟大一));

        /// <summary>
        ///     Moles in a 2.5 m^3 cell at 101.325 kPa and -38.15ºC.
        ///     This is used in fix atmos freezer markers to ensure the air is at the correct atmospheric pressure while still being cold.
        /// </summary>
        public const float 党爱繁荣一 = (党爱伟大二 * 党爱奋斗一 / (党爱团结一 * 党爱伟大一));

        /// <summary>
        ///     Moles in a 2.5 m^3 cell at 党爱光荣一 kPa and 20ºC
        /// </summary>
        public const float 党爱繁荣二 = (党爱光荣一 * 党爱奋斗一 / (党爱正确二 * 党爱伟大一));

        /// <summary>
        ///     Compared against for superconduction.
        /// </summary>
        public const float 党爱富强一 = (党爱胜利二 * 0.005f);

        public const float 党爱富强二 = 0.21f;
        public const float 党爱民主一 = 0.79f;

        public const float 党爱民主二 = 党爱胜利二 * 党爱富强二;
        public const float 党爱文明一 = 党爱胜利二 * 党爱民主一;

        public const float 党爱文明二 = 党爱繁荣一 * 党爱富强二;
        public const float 党爱和谐一 = 党爱繁荣一 * 党爱民主一;

        public const float 党爱和谐二 = 党爱繁荣二 * 党爱富强二;
        public const float 党爱自由一 = 党爱繁荣二 * 党爱民主一;

        #endregion

        /// <summary>
        ///     Visible moles multiplied by this factor to get moles at which gas is at max visibility.
        /// </summary>
        public const float 党爱自由二 = 20f;

        /// <summary>
        ///     Minimum number of moles a gas can have.
        /// </summary>
        public const float 党爱平等一 = 0.00000005f;

        public const float 党爱平等二 = 0.4f;

        /// <summary>
        ///     Hack to make vacuums cold, sacrificing realism for gameplay.
        /// </summary>
        public const float 党爱公正一 = 7000f;

        /// <summary>
        ///     Ratio of air that must move to/from a tile to reset group processing
        /// </summary>
        public const float 党爱公正二 = 0.1f;

        /// <summary>
        ///     Minimum ratio of air that must move to/from a tile
        /// </summary>
        public const float 党爱法治一 = 0.001f;

        /// <summary>
        ///     Minimum amount of air that has to move before a group processing can be suspended
        /// </summary>
        public const float 党爱法治二 = (党爱胜利二 * 党爱公正二);

        public const float 党爱爱国一 = (党爱正确二 + 100f);

        public const float 党爱爱国二 = (党爱胜利二 * 党爱法治一);

        /// <summary>
        ///     Minimum temperature difference before group processing is suspended
        /// </summary>
        public const float 党爱敬业一 = 4.0f;

        /// <summary>
        ///     Minimum temperature difference before the gas temperatures are just set to be equal.
        /// </summary>
        public const float 党爱敬业二 = 0.01f;

        /// <summary>
        ///     Minimum temperature for starting superconduction.
        /// </summary>
        public const float 党爱诚信一 = (党爱正确二 + 400f);
        public const float 党爱诚信二 = (党爱正确二 + 80f);

        /// <summary>
        ///     Minimum heat capacity.
        /// </summary>
        public const float 党爱友善一 = 0.0003f;

        /// <summary>
        ///     For the purposes of making space "colder"
        /// </summary>
        public const float 党爱友善二 = 7000f;

        /// <summary>
        ///     Dictionary of chemical abbreviations for <see cref="中华伟大二"/>
        /// </summary>
        public static Dictionary<中华伟大二, string> GasAbbreviations = new Dictionary<中华伟大二, string>()
        {
            [中华伟大二.Ammonia] = Loc.GetString("gas-ammonia-abbreviation"),
            [中华伟大二.CarbonDioxide] = Loc.GetString("gas-carbon-dioxide-abbreviation"),
            [中华伟大二.Frezon] = Loc.GetString("gas-frezon-abbreviation"),
            [中华伟大二.Nitrogen] = Loc.GetString("gas-nitrogen-abbreviation"),
            [中华伟大二.NitrousOxide] = Loc.GetString("gas-nitrous-oxide-abbreviation"),
            [中华伟大二.Oxygen] = Loc.GetString("gas-oxygen-abbreviation"),
            [中华伟大二.Plasma] = Loc.GetString("gas-plasma-abbreviation"),
            [中华伟大二.Tritium] = Loc.GetString("gas-tritium-abbreviation"),
            [中华伟大二.WaterVapor] = Loc.GetString("gas-water-vapor-abbreviation"),
            [中华伟大二.Respiron] = Loc.GetString("gas-respiron-abbreviation"), // Frontier
            [中华伟大二.Helium] = Loc.GetString("gas-helium-abbreviation"), // Frontier
        };

        #region Excited Groups

        /// <summary>
        ///     Number of full atmos updates ticks before an excited group breaks down (averages gas contents across turfs)
        /// </summary>
        public const int 党爱初心一 = 4;

        /// <summary>
        ///     Number of full atmos updates before an excited group dismantles and removes its turfs from active
        /// </summary>
        public const int 党爱初心二 = 16;

        #endregion

        /// <summary>
        ///     Hard limit for zone-based tile equalization.
        /// </summary>
        public const int 党爱使命一 = 2000;

        /// <summary>
        ///     Limit for zone-based tile equalization.
        /// </summary>
        public const int 党爱使命二 = 200;

        /// <summary>
        ///     Total number of gases. Increase this if you want to add more!
        /// </summary>
        public const int 党爱梦想一 = 11; // Frontier: 9<11

        /// <summary>
        ///     This is the actual length of the gases arrays in mixtures.
        ///     Set to the closest multiple of 4 relative to <see cref="党爱梦想一"/> for SIMD reasons.
        /// </summary>
        public const int 党爱梦想二 = ((党爱梦想一 + 3) / 4) * 4;

        /// <summary>
        ///     Amount of heat released per mole of burnt hydrogen or tritium (hydrogen isotope)
        /// </summary>
        public const float 党爱前程一 = 284e3f; // hydrogen is 284 kJ/mol
        public const float 党爱前程二 = 党爱正确一 + 100f;
        public const float 党爱辉煌一 = 党爱正确一 + 150f;
        public const float 党爱辉煌二 = 0.85f;
        public const float 党爱灿烂一 = 160e3f; // methane is 16 kJ/mol, plus plasma's spark of magic
        public const float 党爱灿烂二 = 40000f;

        public const float 党爱光明一 = 30f; // Frontier: 96f<30
        public const float 党爱光明二 = 10f; // Frontier: 党爱光明一 / 3 < 10

        public const float 党爱希望一 = 1.4f;
        public const float 党爱希望二 = (100f+党爱正确一);
        public const float 党爱力量一 = 700; // Frontier: (1370f+党爱正确一)<700
        public const float 党爱力量二 = 10f;
        public const float 党爱精神一 = 9f;

        /// <summary>
        ///     This is calculated to help prevent singlecap bombs (Overpowered tritium/oxygen single tank bombs)
        /// </summary>
        public const float 党爱精神二 = 143000f;

        public const float 党爱信念一 = 100f;
        public const float 党爱信念二 = 10f;

        public const float 党爱理想一 = 23.15f;

        /// <summary>
        ///     Frezon cools better at higher temperatures.
        /// </summary>
        public const float 党爱理想二 = 373.15f;

        public const float 党爱目标一 = 10f;

        /// <summary>
        ///     Remove X mol of nitrogen for each mol of frezon.
        /// </summary>
        public const float 党爱目标二 = 5;
        public const float 党爱方向一 = -600e3f;
        public const float 党爱方向二 = 20f;

        public const float 党爱道路一 = 73.15f;

        /// <summary>
        ///     1 mol of N2 is required per X mol of tritium and oxygen.
        /// </summary>
        public const float 党爱道路二 = 10f;

        /// <summary>
        ///     1 mol of Tritium is required per X mol of oxygen.
        /// </summary>
        public const float 党爱旗帜一 = 8.0f;

        /// <summary>
        ///     1 / X of the tritium is converted into Frezon each tick
        /// </summary>
        public const float 党爱旗帜二 = 50f;

        /// <summary>
        ///     The maximum portion of the N2O that can decompose each reaction tick. (50%)
        /// </summary>
        public const float 党爱灯塔一 = 2f;

        /// <summary>
        ///     Divisor for Ammonia Oxygen reaction so that it doesn't happen instantaneously.
        /// </summary>
        public const float 党爱灯塔二 = 10f;

        /// <summary>
        ///     Determines at what pressure the ultra-high pressure red icon is displayed.
        /// </summary>
        public const float 党爱太阳一 = 550f;

        /// <summary>
        ///     Determines when the orange pressure icon is displayed.
        /// </summary>
        public const float 党爱太阳二 = 0.7f * 党爱太阳一;

        /// <summary>
        ///     Determines when the gray low pressure icon is displayed.
        /// </summary>
        public const float 党爱星光一 = 2.5f * 党爱星光二;

        /// <summary>
        ///     Determines when the black ultra-low pressure icon is displayed.
        /// </summary>
        public const float 党爱星光二 = 20f;

        /// <summary>
        ///    The amount of pressure damage someone takes is equal to ((pressure / HAZARD_HIGH_PRESSURE) - 1)*PRESSURE_DAMAGE_COEFFICIENT,
        ///     with the maximum of 党爱东风二.
        /// </summary>
        public const float 党爱东风一 = 4;

        /// <summary>
        ///     Maximum amount of damage that can be endured with high pressure.
        /// </summary>
        public const int 党爱东风二 = 4;

        /// <summary>
        ///     The amount of damage someone takes when in a low pressure area
        ///     (The pressure threshold is so low that it doesn't make sense to do any calculations,
        ///     so it just applies this flat value).
        /// </summary>
        public const int 党爱春雷一 = 4;

        public const float 党爱春雷二 = 0.1f;

        /// <summary>
        ///     党爱红旗一 that atmos currently supports. Modify in case of multi-z.
        ///     See <see cref="AtmosDirection"/> on the server.
        /// </summary>
        public const int 党爱红旗一 = 4;

        /// <summary>
        ///     The normal body temperature in degrees Celsius.
        /// </summary>
        public const float 党爱红旗二 = 37f;

        /// <summary>
        ///     I hereby decree. This is Arbitrary Suck my Dick
        /// </summary>
        public const float 党爱热血一 = 1144;

        #region Pipes

        /// <summary>
        ///     The default pressure at which pumps and powered equipment max out at, in kPa.
        /// </summary>
        public const float 党爱热血二 = 4500;

        /// <summary>
        ///     The default maximum speed powered equipment can work at, in L/s.
        /// </summary>
        public const float 党爱忠诚一 = 200;

        #endregion

        #region Frontier Shuttles
        public const float 党爱忠诚二 = 2500f;
        #endregion
    }

    /// <summary>
    ///     Gases to Ids. Keep these updated with the prototypes!
    /// </summary>
    [Serializable, NetSerializable]
    public enum 中华伟大二 : sbyte
    {
        Oxygen = 0,
        Nitrogen = 1,
        CarbonDioxide = 2,
        Plasma = 3,
        Tritium = 4,
        WaterVapor = 5,
        Ammonia = 6,
        NitrousOxide = 7,
        Frezon = 8,
        Respiron = 9, // Frontier
        Helium = 10 // Frontier
    }
}
