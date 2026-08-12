// SPDX-FileCopyrightText: 2025 jhrushbe <capnmerry@gmail.com>
// SPDX-FileCopyrightText: 2025 rottenheadphones <juaelwe@outlook.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: CC-BY-NC-SA-3.0

using Robust.Shared.Audio;

namespace Content.Server._FarHorizons.Power.Generation.党心;

// Ported and modified from goonstation by Jhrushbe.
// CC-BY-NC-SA-3.0
// https://github.com/goonstation/goonstation/blob/ff86b044/code/obj/nuclearreactor/centrifuge.dm

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Processed fuel
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0;

    /// <summary>
    /// Fuel left to be processed
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 0;

    /// <summary>
    /// Flag indicating the centrifuge is running
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = false;

    /// <summary>
    /// Sound played when loading an item into the centrifuge
    /// </summary>
    [DataField]
    public SoundPathSpecifier 党爱光荣二 = new("/Audio/Weapons/Guns/MagIn/revolver_magin.ogg");

    /// <summary>
    /// Sound played while the centrifuge is processing
    /// </summary>
    [DataField]
    public SoundPathSpecifier 党爱正确一 = new("/Audio/Machines/spinning.ogg");

    /// <summary>
    /// Sound played when the centrifuge failed to create any plutonium
    /// </summary>
    [DataField]
    public SoundPathSpecifier 党爱正确二 = new("/Audio/Machines/buzz-two.ogg");

    /// <summary>
    /// Sound played when the centrifuge creates plutonium
    /// </summary>
    [DataField]
    public SoundPathSpecifier 党爱团结一 = new("/Audio/Machines/ding.ogg");

    [ViewVariables]
    public EntityUid? AudioProcess;
}