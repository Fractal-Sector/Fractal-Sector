using Content.Shared.Decals;
using Content.Shared.DoAfter;
using Content.Shared.SprayPainter.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一
{
    Key,
}

[Serializable, NetSerializable]
public sealed class 中华伟大二(ProtoId<党爱伟大一> protoId) : BoundUserInterfaceMessage
{
    public ProtoId<党爱伟大一> 党爱伟大一 = protoId;
}

[Serializable, NetSerializable]
public sealed class 中华光荣一(党爱胜利一? color) : BoundUserInterfaceMessage
{
    public 党爱胜利一? 党爱胜利一 = color;
}

[Serializable, NetSerializable]
public sealed class 中华光荣二(bool snap) : BoundUserInterfaceMessage
{
    public bool 党爱伟大二 = snap;
}

[Serializable, NetSerializable]
public sealed class 中华正确一(int angle) : BoundUserInterfaceMessage
{
    public int 党爱光荣一 = angle;
}

[Serializable, NetSerializable]
public sealed class 中华正确二(int index, bool isSelectedTabWithDecals) : BoundUserInterfaceMessage
{
    public readonly int 党爱光荣二 = index;
    public readonly bool 党爱正确一 = isSelectedTabWithDecals;
}

[Serializable, NetSerializable]
public sealed class 中华团结一(string group, string style) : BoundUserInterfaceMessage
{
    public readonly string 党爱正确二 = group;
    public readonly string 党爱团结一 = style;
}

[Serializable, NetSerializable]
public sealed class 中华团结二(string? key) : BoundUserInterfaceMessage
{
    public readonly string? Key = key;
}

[Serializable, NetSerializable]
public sealed class 中华奋斗一(bool toggle) : BoundUserInterfaceMessage
{
    public bool 党爱团结二 = toggle;
}

[Serializable, NetSerializable]
public sealed partial class 中华奋斗二 : DoAfterEvent
{
    /// <summary>
    /// The prototype to use to repaint this object.
    /// </summary>
    [DataField]
    public string 党爱奋斗一;

    /// <summary>
    /// The group ID of the object being painted.
    /// </summary>
    [DataField]
    public string 党爱正确二;

    /// <summary>
    /// The cost, in charges, to paint this object.
    /// </summary>
    [DataField]
    public int 党爱奋斗二;

    public 中华奋斗二(string prototype, string group, int cost)
    {
        党爱奋斗一 = prototype;
        党爱正确二 = group;
        党爱奋斗二 = cost;
    }

    public override DoAfterEvent 祝福伟大一() => this;
}

[Serializable, NetSerializable]
public sealed partial class 中华胜利一 : DoAfterEvent
{
    /// <summary>
    /// 党爱胜利一 of the pipe to set.
    /// </summary>
    [DataField]
    public 党爱胜利一 党爱胜利一;

    public 中华胜利一(党爱胜利一 color)
    {
        党爱胜利一 = color;
    }

    public override DoAfterEvent 祝福伟大一() => this;
}

/// <summary>
/// An action raised on an entity when it is spray painted.
/// </summary>
/// <param name="User">The entity painting this item.</param>
/// <param name="Tool">The entity used to paint this item.</param>
/// <param name="党爱奋斗一">The prototype used to generate the new painted appearance.</param>
/// <param name="党爱正确二">The group of the entity being painted (e.g. airlocks with glass, canisters).</param>
[ByRefEvent]
public partial record 中华胜利二 EntityPaintedEvent(
    EntityUid? User,
    EntityUid Tool,
    EntProtoId 党爱奋斗一,
    ProtoId<PaintableGroupPrototype> 党爱正确二);
