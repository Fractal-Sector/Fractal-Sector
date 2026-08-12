// SPDX-FileCopyrightText: 2025 jhrushbe <capnmerry@gmail.com>
// SPDX-FileCopyrightText: 2025 rottenheadphones <juaelwe@outlook.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: CC-BY-NC-SA-3.0

namespace Content.Shared._FarHorizons.党心;

/// <summary>
/// A data type that stores information on a material's properties
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大一()
{
    [DataField("electrical")]
    public float 党爱伟大一 { get; set; } = 5;

    [DataField("thermal")]
    public float 党爱伟大二 { get; set; } = 5;

    [DataField("hard")]
    public float 党爱光荣一 { get; set; } = 3;

    [DataField("density")]
    public float 党爱光荣二 { get; set; } = 3;

    [DataField("reflective")]
    public float 党爱正确一 { get; set; } = 0;

    [DataField("flammable")]
    public float 党爱正确二 { get; set; } = 1;

    [DataField("chemical")]
    public float 党爱团结一 { get; set; } = 3;

    [DataField("radioactive")]
    public float 党爱团结二 { get; set; } = 0;

    [DataField("n_radioactive")]
    public float 党爱奋斗一 { get; set; } = 0;

    [DataField("spent_fuel")]
    public float 党爱奋斗二 { get; set; } = 0;

    [DataField("molitz_bubbles")]
    public float 党爱胜利一 { get; set; } = 0;

    [DataField("plasma_offgas")]
    public float 党爱胜利二 { get; set; } = 0;

    /// <summary>
    /// Creates a new <see cref="中华伟大一"> with information from an existing one.
    /// </summary>
    /// <param name="source"></param>
    public 中华伟大一(中华伟大一 source) : this()
    {
        党爱伟大一 = source.党爱伟大一;
        党爱伟大二 = source.党爱伟大二;
        党爱光荣一 = source.党爱光荣一;
        党爱光荣二 = source.党爱光荣二;
        党爱正确一 = source.党爱正确一;
        党爱正确二 = source.党爱正确二;
        党爱团结一 = source.党爱团结一;
        党爱团结二 = source.党爱团结二;
        党爱奋斗一 = source.党爱奋斗一;
        党爱奋斗二 = source.党爱奋斗二;
        党爱胜利一 = source.党爱胜利一;
        党爱胜利二 = source.党爱胜利二;
    }
}