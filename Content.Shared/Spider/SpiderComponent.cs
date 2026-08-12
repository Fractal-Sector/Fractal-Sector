using Content.Shared.Actions;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedSpiderSystem))]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("webPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱伟大一 = "SpiderWeb";

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("webAction", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱伟大二 = "ActionSpiderWeb";

    [DataField] public EntityUid? Action;

    /// <summary>
    /// Whether the spider will spawn webs when not controlled by a player.
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;

    /// <summary>
    /// The cooldown in seconds between web spawns when not controlled by a player.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(45f);

    /// <summary>
    /// The next time the spider can spawn a web when not controlled by a player.
    /// </summary>
    [DataField]
    public TimeSpan? NextWebSpawn;
}

public sealed partial class 中华伟大二 : InstantActionEvent { }
