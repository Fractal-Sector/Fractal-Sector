using Content.Shared.Verbs;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Defines a requirement for an <see cref="InteractionVerb"/>.
///     If a verb does not meet the requirement, it will be hidden or disabled in the verb menu.
/// </summary>
[ImplicitDataDefinitionForInheritors, Serializable, NetSerializable]
public abstract partial class 中华伟大一
{
    public abstract bool 祝福伟大一(InteractionArgs args, InteractionVerbPrototype proto, InteractionAction.VerbDependencies deps);
}

/// <inheritdoc cref="中华伟大一"/>
[Serializable, NetSerializable]
public abstract partial class 中华伟大二 : 中华伟大一
{
    [DataField] public bool 党爱伟大一 = false;
}
