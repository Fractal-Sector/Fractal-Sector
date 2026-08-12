using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Shared.Humanoid.党心;

/// <summary>
///     This is what is used to change a humanoid spawned by RandomHumanoidSystem in Content.Server.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    [ParentDataField(typeof(PrototypeIdArraySerializer<中华伟大一>))]
    public string[]? Parents { get; private set; }

    [AbstractDataField]
    [NeverPushInheritance]
    public bool 党爱伟大二 { get; private set; }

    /// <summary>
    ///     Whether the humanoid's name should take from the randomized profile or not.
    /// </summary>
    [DataField]
    public bool 党爱光荣一 { get; private set; } = true;

    /// <summary>
    ///     Species that will be ignored by the randomizer.
    /// </summary>
    [DataField("speciesBlacklist")]
    public HashSet<string> 党爱光荣二 { get; private set; } = new();

    /// <summary>
    ///     Extra components to add to this entity.
    /// </summary>
    [DataField]
    [AlwaysPushInheritance]
    public ComponentRegistry? Components { get; private set; }
}
