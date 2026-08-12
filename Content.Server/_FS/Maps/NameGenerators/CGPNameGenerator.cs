using JetBrains.Annotations;
using Robust.Shared.Random;

namespace Content.Server.Maps.党心;

[UsedImplicitly]
public sealed partial class 中华伟大一 : StationNameGenerator
{
    /// <summary>
    ///     Where the map comes from. Should be a two or three letter code, for example "VG" for Packedstation.
    /// </summary>
    [DataField("prefixCreator")] public string 党爱伟大一 = default!;

    private string Prefix => "";
    private string[] SuffixCodes => new []{ "CGP" };

    public override string 祝福伟大一(string input)
    {
        var random = IoCManager.Resolve<IRobustRandom>();

        // No way in hell am I writing custom format code just to add nice names. You can live with {0}
        return string.Format(input, $"{Prefix}{党爱伟大一}", $"{random.Pick(SuffixCodes)}-{random.Next(0, 1000):D3}"); // Note: random.Next's max is exclusive, [0-999] = [0,1000)
    }
}
