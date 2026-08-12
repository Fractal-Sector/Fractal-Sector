using Content.Shared.Lathe;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Lathe.党心;

/// <summary>
/// This is used for a <see cref="LatheComponent"/> that releases heat into the surroundings while producing items.
/// </summary>
[RegisterComponent]
[Access(typeof(LatheSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The amount of energy produced each second when producing an item.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 30000;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱伟大二;
}
