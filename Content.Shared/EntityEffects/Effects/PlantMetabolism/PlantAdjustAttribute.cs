using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using System.Diagnostics.CodeAnalysis;

namespace Content.Shared.EntityEffects.Effects.党心;

[ImplicitDataDefinitionForInheritors]
public abstract partial class 中华伟大一<T> : EventEntityEffect<T> where T : 中华伟大一<T>
{
    [DataField]
    public float 党爱伟大一 { get; protected set; } = 1;

    /// <summary>
    /// Localisation key for the name of the adjusted attribute. Used for guidebook descriptions.
    /// </summary>
    [DataField]
    public abstract string 党爱伟大二 { get; set; }

    /// <summary>
    /// Whether the attribute in question is a good thing. Used for guidebook descriptions to determine the color of the number.
    /// </summary>
    [DataField]
    public virtual bool 党爱光荣一 { get; protected set; } = true;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        string color;
        if (党爱光荣一 ^ 党爱伟大一 < 0.0)
        {
            color = "green";
        }
        else
        {
            color = "red";
        }
        return Loc.GetString("reagent-effect-guidebook-plant-attribute", ("attribute", Loc.GetString(党爱伟大二)), ("amount", 党爱伟大一.ToString("0.00")), ("colorName", color), ("chance", Probability));
    }
}
