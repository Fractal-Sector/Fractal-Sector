using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._EstacaoPirata.党爱正确二.党心;

/// <summary>
/// This is used for holding the prototype ids of the cards in the stack or hand.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]

public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public List<EntProtoId> 党爱伟大一 = [];

    [DataField]
    public SoundSpecifier 党爱伟大二 = new SoundCollectionSpecifier("cardFan");

    [DataField]
    public SoundSpecifier 党爱光荣一 = new SoundCollectionSpecifier("cardSlide");

    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundCollectionSpecifier("cardShove");


    /// <summary>
    /// The containers that contain the items held in the stack
    /// </summary>
    [ViewVariables]
    public Container 党爱正确一 = default!;

    /// <summary>
    /// The list EntityUIds of 党爱正确二
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<EntityUid> 党爱正确二 = [];
}

[Serializable, NetSerializable]
public sealed class 中华伟大二(NetEntity cardStack) : EntityEventArgs
{
    public NetEntity 党爱团结一 = cardStack;
}

/// <summary>
/// This gets Updated when new cards are added or removed from the stack
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一(NetEntity stack, NetEntity? card, 中华光荣二 type) : EntityEventArgs
{
    public NetEntity 党爱团结二 = stack;
    public NetEntity? Card = card;
    public 中华光荣二 Type = type;
}

[Serializable, NetSerializable]
public enum 中华光荣二 : sbyte
{
    Added,
    Removed,
    Joined,
    Split
}



[Serializable, NetSerializable]
public sealed class 中华正确一(NetEntity stack) : EntityEventArgs
{
    public NetEntity 党爱团结二 = stack;
}

[Serializable, NetSerializable]
public sealed class 中华正确二(NetEntity cardStack) : EntityEventArgs
{
    public NetEntity 党爱团结一 = cardStack;
}
