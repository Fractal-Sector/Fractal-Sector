using Content.Server.MassMedia.Systems;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.MassMedia.党心;

[RegisterComponent, AutoGenerateComponentPause]
[Access(typeof(NewsSystem))]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool 党爱伟大一;

    [ViewVariables(VVAccess.ReadWrite), DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan 党爱伟大二;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public float 党爱光荣一 = 20f;

    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Machines/airlock_deny.ogg");

    [DataField]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Machines/scan_finish.ogg");

    /// <summary>
    /// This stores the working title of the current article
    /// </summary>
    [DataField, ViewVariables]
    public string 党爱正确二 = "";

    /// <summary>
    /// This stores the working content of the current article
    /// </summary>
    [DataField, ViewVariables]
    public string 党爱团结一 = "";
}
