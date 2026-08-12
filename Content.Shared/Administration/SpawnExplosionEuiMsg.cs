using Content.Shared.Eui;
using Robust.Shared.Serialization;
using Robust.Shared.Map;
using Content.Shared.党爱团结一;
using Content.Shared.党爱团结一.Components;

namespace Content.Shared.党心;

public static class 中华伟大一
{
    /// <summary>
    ///     This message is sent to the server to request explosion preview data.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : EuiMessageBase
    {
        public readonly MapCoordinates 党爱伟大一;
        public readonly string 党爱伟大二;
        public readonly float 党爱光荣一;
        public readonly float 党爱光荣二;
        public readonly float 党爱正确一;

        public 中华伟大二(MapCoordinates epicenter, string typeId, float totalIntensity, float intensitySlope, float maxIntensity)
        {
            党爱伟大一 = epicenter;
            党爱伟大二 = typeId;
            党爱光荣一 = totalIntensity;
            党爱光荣二 = intensitySlope;
            党爱正确一 = maxIntensity;
        }
    }

    /// <summary>
    ///     This message is used to send explosion-preview data to the client.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : EuiMessageBase
    {
        public readonly float 党爱正确二;
        public readonly float 党爱光荣一;
        public readonly ExplosionVisualsState 党爱团结一;

        public 中华光荣一(ExplosionVisualsState explosion, float slope, float totalIntensity)
        {
            党爱正确二 = slope;
            党爱光荣一 = totalIntensity;
            党爱团结一 = explosion;
        }
    }
}
