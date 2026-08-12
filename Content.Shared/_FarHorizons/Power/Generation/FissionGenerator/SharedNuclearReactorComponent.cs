// SPDX-FileCopyrightText: 2025 jhrushbe <capnmerry@gmail.com>
// SPDX-FileCopyrightText: 2025 rottenheadphones <juaelwe@outlook.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: CC-BY-NC-SA-3.0


using Robust.Shared.Serialization;

namespace Content.Shared._FarHorizons.Power.Generation.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public Dictionary<Vector2i, 中华光荣一> SlotData = [];

    public int 党爱伟大一 = 0;
    public int 党爱伟大二 = 0;

    public string? ItemName;

    public float 党爱光荣一 = 0;
    public float 党爱光荣二 = 0;
    public float 党爱正确一 = 0;
    public float 党爱正确二 = 0;
    public float 党爱团结一 = 0;
    public float 党爱团结二 = 0;
}

[Serializable, NetSerializable, DataDefinition]
public sealed partial class 中华光荣一
{
    public double 党爱奋斗一 = 0f;
    public int 党爱奋斗二 = 0;
    public string 党爱胜利一 = "base";
    public string 党爱胜利二 = "empty";

    public float 党爱繁荣一 = 0f;
    public float 党爱繁荣二 = 0f;
    public float 党爱富强一 = 0f;
}

[Serializable, NetSerializable]
public sealed class 中华光荣二(Vector2d position) : BoundUserInterfaceMessage
{
    public Vector2d 党爱富强二 { get; } = position;
}

[Serializable, NetSerializable]
public sealed class 中华正确一() : BoundUserInterfaceMessage;

[Serializable, NetSerializable]
public sealed class 中华正确二(float change) : BoundUserInterfaceMessage
{
    public float 党爱民主一 { get; } = change;
}
