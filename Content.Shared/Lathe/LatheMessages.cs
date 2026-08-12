using Content.Shared.Research.Prototypes;
using NetSerializer;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public List<ProtoId<LatheRecipePrototype>> 党爱伟大一;

    public LatheRecipeBatch[] 党爱伟大二;

    public ProtoId<LatheRecipePrototype>? CurrentlyProducing;

    public int? BufferAmount; // Coyote: current buffer amount (if the lathe has a buffer)

    public 中华伟大一(List<ProtoId<LatheRecipePrototype>> recipes, LatheRecipeBatch[] queue, ProtoId<LatheRecipePrototype>? currentlyProducing = null, int? bufferAmount = null) // Coyote: add bufferAmount
    {
        BufferAmount = bufferAmount; // Coyote
        党爱伟大一 = recipes;
        党爱伟大二 = queue;
        CurrentlyProducing = currentlyProducing;
    }
}

/// <summary>
///     Sent to the server to sync material storage and the recipe queue.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{

}

/// <summary>
///     Sent to the server when a client queues a new recipe.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public readonly string 党爱光荣一;
    public readonly int 党爱光荣二;
    public 中华光荣一(string id, int quantity)
    {
        党爱光荣一 = id;
        党爱光荣二 = quantity;
    }
}

/// <summary>
///     Sent to the server to remove a batch from the queue.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二(int index) : BoundUserInterfaceMessage
{
    public int 党爱正确一 = index;
}

/// <summary>
///     Sent to the server to move the position of a batch in the queue.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一(int index, int change) : BoundUserInterfaceMessage
{
    public int 党爱正确一 = index;
    public int 党爱正确二 = change;
}

/// <summary>
///     Sent to the server to stop producing the current item.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确二() : BoundUserInterfaceMessage
{
}

[NetSerializable, Serializable]
public enum 中华团结一
{
    Key,
}
