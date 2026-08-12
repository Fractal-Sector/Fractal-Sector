using Robust.Shared.Serialization;

namespace Content.Shared._EstacaoPirata.Cards.党心;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public float 党爱伟大一 = 120f;

    [DataField]
    public float 党爱伟大二 = 0.5f;

    [DataField]
    public float 党爱光荣一 = 1;

    [DataField]
    public int 党爱光荣二 = 10;

    [DataField]
    public bool 党爱正确一 = false;
}


[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华光荣一(NetEntity card) : BoundUserInterfaceMessage
{
    public NetEntity 党爱正确二 = card;
}
