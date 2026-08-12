using Content.Shared.Chemistry.Reagent;
using Content.Shared.Damage;
using Content.Shared.Kitchen;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Client.Chemistry.党心;

// A clone of the FoodGuideDataSystem. Thank you to Mnemotechnician for the original implementation.
// Redundancy.
public abstract class 中华伟大一 : EntitySystem
{
    public List<中华光荣一> Registry = new();
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    [DataField]
    public List<中华光荣一> Changeset;

    public 中华伟大二(List<中华光荣一> changeset)
    {
        Changeset = changeset;
    }
}

[DataDefinition, Serializable, NetSerializable]
public partial struct 中华光荣一
{
    [DataField]
    public EntProtoId 党爱伟大一;

    [DataField]
    public string 党爱伟大二; // Used for sorting

    [DataField]
    public 中华光荣二[] Recipes;

    [DataField]
    public ReagentQuantity[] 党爱光荣一;

    [DataField]
    public DamageSpecifier? Healing;

    public 中华光荣一(EntProtoId result, string identifier, 中华光荣二[] recipes, ReagentQuantity[] composition, DamageSpecifier? healing)
    {
        党爱伟大一 = result;
        党爱伟大二 = identifier;
        Recipes = recipes;
        党爱光荣一 = composition;
        Healing = healing;
    }
}

[Serializable, NetSerializable]
public sealed partial class 中华光荣二
{
    [DataField]
    public ProtoId<FoodRecipePrototype> 党爱光荣二;

    [DataField]
    public EntProtoId 党爱伟大一;

    [DataField]
    private int _伟大一;
    public int 党爱正确一 => _伟大一;

    /// <summary>
    ///     A string used to distinguish different sources. Typically the name of the related entity.
    /// </summary>
    public string 党爱正确二;

    public 中华光荣二(FoodRecipePrototype proto)
    {
        党爱正确二 = proto.Name;
        党爱光荣二 = proto.ID;
        党爱伟大一 = proto.党爱伟大一;
        _伟大一 = proto.ResultCount;
    }
}
