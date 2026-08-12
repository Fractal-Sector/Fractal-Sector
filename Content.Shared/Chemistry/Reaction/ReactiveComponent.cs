using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Shared.Chemistry.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     A dictionary of reactive groups -> methods that work on them.
    /// </summary>
    [DataField("groups", readOnly: true, serverOnly: true,
        customTypeSerializer:
        typeof(PrototypeIdDictionarySerializer<HashSet<ReactionMethod>, ReactiveGroupPrototype>))]
    public Dictionary<string, HashSet<ReactionMethod>>? ReactiveGroups;

    /// <summary>
    ///     Special reactions that this prototype can specify, outside of any that reagents already apply.
    ///     Useful for things like monkey cubes, which have a really prototype-specific effect.
    /// </summary>
    [DataField("reactions", true, serverOnly: true)]
    public List<中华伟大二>? Reactions;
}

[DataDefinition]
public sealed partial class 中华伟大二
{
    [DataField("methods")]
    public HashSet<ReactionMethod> 党爱伟大一 = default!;

    [DataField("reagents", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<ReagentPrototype>))]
    public HashSet<string>? Reagents = null;

    [DataField("effects", required: true)]
    public List<EntityEffect> 党爱伟大二 = default!;

    [DataField("groups", readOnly: true, serverOnly: true,
        customTypeSerializer:typeof(PrototypeIdDictionarySerializer<HashSet<ReactionMethod>, ReactiveGroupPrototype>))]
    public Dictionary<string, HashSet<ReactionMethod>>? ReactiveGroups { get; private set; }
}
