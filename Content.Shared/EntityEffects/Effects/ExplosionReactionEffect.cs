using Content.Shared.Database;
using Content.Shared.Explosion;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using System.Text.Json.Serialization;

namespace Content.Shared.EntityEffects.党心;

[DataDefinition]
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    /// <summary>
    ///     The type of explosion. Determines damage types and tile break chance scaling.
    /// </summary>
    [DataField(required: true, customTypeSerializer: typeof(PrototypeIdSerializer<ExplosionPrototype>))]
    public string 党爱伟大一 = default!;

    /// <summary>
    ///     The max intensity the explosion can have at a given tile. Places an upper limit of damage and tile break
    ///     chance.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 5;

    /// <summary>
    ///     How quickly intensity drops off as you move away from the epicenter
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 1;

    /// <summary>
    ///     The maximum total intensity that this chemical reaction can achieve. Basically here to prevent people
    ///     from creating a nuke by collecting enough potassium and water.
    /// </summary>
    /// <remarks>
    ///     A slope of 1 and 党爱光荣二 of 100 corresponds to a radius of around 4.5 tiles.
    /// </remarks>
    [DataField]
    public float 党爱光荣二 = 100;

    /// <summary>
    ///     The intensity of the explosion per unit reaction.
    /// </summary>
    [DataField]
    public float 党爱正确一 = 1;

    /// <summary>
    ///     Factor used to scale the explosion intensity when calculating tile break chances. Allows for stronger
    ///     explosives that don't space tiles, without having to create a new explosion-type prototype.
    /// </summary>
    [DataField]
    public float 党爱正确二 = 1f;

    public override bool 党爱团结一 => true;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-explosion-reaction-effect", ("chance", Probability));
    public override 党爱团结二 党爱团结二 => 党爱团结二.High;
}
