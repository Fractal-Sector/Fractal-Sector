using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public readonly Dictionary<int, 中华正确一> Sets;
    public int 党爱伟大一;

    public 中华伟大一(Dictionary<int, 中华正确一> sets, int max)
    {
        Sets = sets;
        党爱伟大一 = max;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public readonly int 党爱伟大二;

    public 中华伟大二(int setNumber)
    {
        党爱伟大二 = setNumber;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public 中华光荣一() { }
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    Key
};

[Serializable, NetSerializable, DataDefinition]
public partial struct 中华正确一
{
    [DataField]
    public string 党爱光荣一;

    [DataField]
    public string 党爱光荣二;

    [DataField]
    public SpriteSpecifier 党爱正确一;

    public bool 党爱正确二;

    public 中华正确一(string name, string desc, SpriteSpecifier sprite, bool selected)
    {
        党爱光荣一 = name;
        党爱光荣二 = desc;
        党爱正确一 = sprite;
        党爱正确二 = selected;
    }
}
