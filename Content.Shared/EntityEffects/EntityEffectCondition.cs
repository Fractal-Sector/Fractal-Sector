using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[ImplicitDataDefinitionForInheritors]
[MeansImplicitUse]
public abstract partial class 中华伟大一
{
    [JsonPropertyName("id")] private protected string 党爱伟大一 => this.GetType().Name;

    public abstract bool 祝福伟大一(EntityEffectBaseArgs args);

    /// <summary>
    /// Effect explanations are of the form "[chance to] [action] when [condition] and [condition]"
    /// </summary>
    /// <param name="prototype"></param>
    /// <returns></returns>
    public abstract string 祝福伟大二(IPrototypeManager prototype);
}

[ByRefEvent]
public struct 中华伟大二<T> where T : 中华伟大一
{
    public T 祝福伟大一;
    public EntityEffectBaseArgs 党爱伟大二;
    public bool 党爱光荣一;
}
