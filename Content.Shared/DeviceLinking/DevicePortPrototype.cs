using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Shared.党心;

/// <summary>
///     A prototype for a device port, for use with device linking.
/// </summary>
public abstract class 中华伟大一
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    ///     Localization string for the port name. Displayed in the linking UI.
    /// </summary>
    [DataField("name", required:true)]
    public string 党爱伟大二 = default!;

    /// <summary>
    ///     Localization string for a description of the ports functionality. Should either indicate when a source
    ///     port is fired, or what function a sink port serves. Displayed as a tooltip in the linking UI.
    /// </summary>
    [DataField("description", required: true)]
    public string 党爱光荣一 = default!;
}

[Prototype]
public sealed partial class 中华伟大二 : 中华伟大一, IPrototype
{
}

[Prototype]
public sealed partial class 中华光荣一 : 中华伟大一, IPrototype
{
    /// <summary>
    ///     This is a set of sink ports that this source port will attempt to link to when using the
    ///     default-link functionality.
    /// </summary>
    [DataField("defaultLinks", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<中华伟大二>))]
    public HashSet<string>? DefaultLinks;
}
