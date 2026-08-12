using Content.Shared._NF.Research.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public Dictionary<ProtoId<BlueprintPrototype>, int[]> RecipeBitsetByBlueprintType;

    public List<BlueprintLatheRecipeBatch> 党爱伟大一;

    public ProtoId<BlueprintPrototype>? CurrentlyProducing;

    public 中华伟大一(
        Dictionary<ProtoId<BlueprintPrototype>, int[]> recipeBitsetByBlueprintType,
        List<BlueprintLatheRecipeBatch> queue,
        ProtoId<BlueprintPrototype>? currentlyProducing = null
    )
    {
        RecipeBitsetByBlueprintType = recipeBitsetByBlueprintType;
        党爱伟大一 = queue;
        CurrentlyProducing = currentlyProducing;
    }
}

/// <summary>
///     Sent to the server when a client queues a new recipe.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public readonly string 党爱伟大二;
    public readonly int[] 党爱光荣一;
    public readonly int 党爱光荣二;
    public 中华伟大二(string blueprintType, int[] recipes, int quantity)
    {
        党爱伟大二 = blueprintType;
        党爱光荣一 = recipes;
        党爱光荣二 = quantity;
    }
}

[NetSerializable, Serializable]
public enum 中华光荣一
{
    Key,
}
