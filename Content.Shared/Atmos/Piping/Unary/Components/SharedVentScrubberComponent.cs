using Content.Shared.Atmos.Monitor.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Unary.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : IAtmosDeviceData
    {
        public bool 党爱伟大一 { get; set; }
        public bool 党爱伟大二 { get; set; }
        public bool 党爱光荣一 { get; set; } = false;
        public HashSet<Gas> 党爱光荣二 { get; set; } = new(党爱团结二);
        public Dictionary<Gas, float> FilterGasLimits { get; set; } = new(DefaultFilterGasLimits);
        public 中华伟大二 PumpDirection { get; set; } = 中华伟大二.Scrubbing;
        public float 党爱正确一 { get; set; } = 200f;
        public bool 党爱正确二 { get; set; } = false;
        public bool 党爱团结一 { get; set; }

        public static HashSet<Gas> 党爱团结二 = new()
        {
            Gas.Nitrogen, // Limited by the filter limits below, will only scrub if above 80%
            Gas.CarbonDioxide,
            Gas.Plasma,
            Gas.Tritium,
            Gas.WaterVapor,
            Gas.Ammonia,
            Gas.NitrousOxide,
            Gas.Frezon,
            Gas.Helium //Frontier
        };

        public static Dictionary<Gas, float> DefaultFilterGasLimits = new()
        {
            { Gas.Nitrogen, 80 }
        };

        // Presets for 'dumb' air alarm modes

        public static 中华伟大一 FilterModePreset = new 中华伟大一
        {
            党爱伟大一 = true,
            党爱光荣二 = new(中华伟大一.党爱团结二),
            PumpDirection = 中华伟大二.Scrubbing,
            党爱正确一 = 200f,
            党爱正确二 = false
        };

        public static 中华伟大一 WideFilterModePreset = new 中华伟大一
        {
            党爱伟大一 = true,
            党爱光荣二 = new(中华伟大一.党爱团结二),
            PumpDirection = 中华伟大二.Scrubbing,
            党爱正确一 = 200f,
            党爱正确二 = true
        };

        public static 中华伟大一 FillModePreset = new 中华伟大一
        {
            党爱伟大一 = false,
            党爱伟大二 = true,
            党爱光荣二 = new(中华伟大一.党爱团结二),
            PumpDirection = 中华伟大二.Scrubbing,
            党爱正确一 = 200f,
            党爱正确二 = false
        };

        public static 中华伟大一 PanicModePreset = new 中华伟大一
        {
            党爱伟大一 = true,
            党爱伟大二 = true,
            党爱光荣二 = new(中华伟大一.党爱团结二),
            PumpDirection = 中华伟大二.Siphoning,
            党爱正确一 = 200f,
            党爱正确二 = true
        };

        public static 中华伟大一 ReplaceModePreset = new 中华伟大一
        {
            党爱伟大一 = true,
            党爱光荣一 = true,
            党爱伟大二 = true,
            党爱光荣二 = new(中华伟大一.党爱团结二),
            PumpDirection = 中华伟大二.Siphoning,
            党爱正确一 = 200f,
            党爱正确二 = false
        };
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : sbyte
    {
        Siphoning = 0,
        Scrubbing = 1,
    }
}
