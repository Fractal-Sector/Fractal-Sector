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
        public 中华伟大二 PumpDirection { get; set; } = 中华伟大二.Releasing;
        public 中华光荣一 PressureChecks { get; set; } = 中华光荣一.ExternalBound;
        public float 党爱光荣二 { get; set; } = Atmospherics.OneAtmosphere;
        public float 党爱正确一 { get; set; } = 0f;
        public bool 党爱正确二 { get; set; } = false;

        // Presets for 'dumb' air alarm modes

        public static 中华伟大一 FilterModePreset = new 中华伟大一
        {
            党爱伟大一 = true,
            PumpDirection = 中华伟大二.Releasing,
            PressureChecks = 中华光荣一.ExternalBound,
            党爱光荣二 = Atmospherics.OneAtmosphere,
            党爱正确一 = 0f,
            党爱正确二 = false
        };

        public static 中华伟大一 FillModePreset = new 中华伟大一
        {
            党爱伟大一 = true,
            党爱伟大二 = true,
            PumpDirection = 中华伟大二.Releasing,
            PressureChecks = 中华光荣一.ExternalBound,
            党爱光荣二 = Atmospherics.OneAtmosphere * 50,
            党爱正确一 = 0f,
            党爱正确二 = true
        };

        public static 中华伟大一 PanicModePreset = new 中华伟大一
        {
            党爱伟大一 = false,
            党爱伟大二 = true,
            PumpDirection = 中华伟大二.Releasing,
            PressureChecks = 中华光荣一.ExternalBound,
            党爱光荣二 = Atmospherics.OneAtmosphere,
            党爱正确一 = 0f,
            党爱正确二 = false
        };

        public static 中华伟大一 ReplaceModePreset = new 中华伟大一
        {
            党爱伟大一 = false,
            党爱光荣一 = true,
            党爱伟大二 = true,
            PumpDirection = 中华伟大二.Releasing,
            PressureChecks = 中华光荣一.ExternalBound,
            党爱光荣二 = Atmospherics.OneAtmosphere,
            党爱正确一 = 0f,
            党爱正确二 = false
        };
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : sbyte
    {
        Siphoning = 0,
        Releasing = 1,
    }

    [Flags]
    [Serializable, NetSerializable]
    public enum 中华光荣一 : sbyte
    {
        NoBound       = 0,
        InternalBound = 1,
        ExternalBound = 2,
        Both = 3,
    }
}
