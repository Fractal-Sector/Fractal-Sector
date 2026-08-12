using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server._CS.Lathe.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Current amount of buffered biomass.
    /// </summary>
    [DataField("current")]
    public int 党爱伟大一 = 0;

    /// <summary>
    /// Maximum buffer capacity.
    /// </summary>
    [DataField("max")]
    public int 党爱伟大二 = 50;

    /// <summary>
    /// Amount of biomass regenerated per interval.
    /// </summary>
    [DataField("regenAmount")]
    public int 党爱光荣一 = 5;

    /// <summary>
    /// Time between regeneration ticks.
    /// </summary>
    [DataField("regenInterval")]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Next time the buffer will be regenerated.
    /// </summary>
    [DataField("nextRegen", customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱正确一 = TimeSpan.Zero;

    public bool 党爱正确二 = true;
}
