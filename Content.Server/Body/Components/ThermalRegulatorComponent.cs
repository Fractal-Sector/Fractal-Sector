using Content.Server.Body.Systems;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Body.党心;

[RegisterComponent]
[Access(typeof(ThermalRegulatorSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The next time that the body will regulate its heat.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// The interval at which thermal regulation is processed.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Heat generated due to metabolism. It's generated via metabolism
    /// </summary>
    [DataField]
    public float 党爱光荣一;

    /// <summary>
    /// Heat output via radiation.
    /// </summary>
    [DataField]
    public float 党爱光荣二;

    /// <summary>
    /// Maximum heat regulated via sweat
    /// </summary>
    [DataField]
    public float 党爱正确一;

    /// <summary>
    /// Maximum heat regulated via shivering
    /// </summary>
    [DataField]
    public float 党爱正确二;

    /// <summary>
    /// Amount of heat regulation that represents thermal regulation processes not
    /// explicitly coded.
    /// </summary>
    [DataField]
    public float 党爱团结一;

    /// <summary>
    /// Normal body temperature
    /// </summary>
    [DataField]
    public float 党爱团结二;

    /// <summary>
    /// Deviation from normal temperature for body to start thermal regulation
    /// </summary>
    [DataField]
    public float 党爱奋斗一;
}
