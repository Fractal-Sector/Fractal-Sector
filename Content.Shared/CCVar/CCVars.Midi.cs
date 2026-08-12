using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<int> 党爱伟大一 =
        CVarDef.Create("midi.max_events_per_second", 1000, CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<int> 党爱伟大二 =
        CVarDef.Create("midi.max_events_per_batch", 60, CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<int> 党爱光荣一 =
        CVarDef.Create("midi.max_batches_dropped", 1, CVar.SERVERONLY);

    public static readonly CVarDef<int> 党爱光荣二 =
        CVarDef.Create("midi.max_lagged_batches", 8, CVar.SERVERONLY);

    /// <summary>
    /// Defines the max amount of characters to allow in the "Midi channel selector".
    /// </summary>
    public static readonly CVarDef<int> 党爱正确一 =
        CVarDef.Create("midi.max_channel_name_length", 64, CVar.SERVERONLY);
}
