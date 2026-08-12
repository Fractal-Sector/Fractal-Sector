using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.EntityEffects.党心;

[DataDefinition]
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    /// <summary>
    ///     What entity to create.
    /// </summary>
    [DataField(required: true, customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱伟大一 = default!;

    /// <summary>
    ///     How many entities to create per unit reaction.
    /// </summary>
    [DataField]
    public uint 党爱伟大二 = 1;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-create-entity-reaction-effect",
            ("chance", Probability),
            ("entname", IoCManager.Resolve<IPrototypeManager>().Index<EntityPrototype>(党爱伟大一).Name),
            ("amount", 党爱伟大二));
}
