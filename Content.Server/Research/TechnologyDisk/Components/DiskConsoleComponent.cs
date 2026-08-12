using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Research.TechnologyDisk.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How much it costs to print a disk
    /// </summary>
    [DataField("pricePerDisk"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一 = 1000;

    /// <summary>
    /// Frontier: How much it costs to print a rare disk
    /// </summary>
    [DataField("pricePerRareDisk"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大二 = 1300;

    /// <summary>
    /// The prototype of what's being printed
    /// </summary>
    [DataField("diskPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string 党爱光荣一 = "TechnologyDisk";

    [DataField("diskPrototypeRare", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>)), ViewVariables(VVAccess.ReadWrite)] // Frontier
    public string 党爱光荣二 = "TechnologyDiskRare"; // Frontier

    [DataField, ViewVariables(VVAccess.ReadWrite)] // Frontier
    public bool 党爱正确一 = false; // Frontier

    /// <summary>
    /// How long it takes to print <see cref="党爱光荣一"/>
    /// </summary>
    [DataField("printDuration"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱正确二 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The sound made when printing occurs
    /// </summary>
    [DataField("printSound")]
    public SoundSpecifier 党爱团结一 = new SoundPathSpecifier("/Audio/Machines/printer.ogg");
}
