// SPDX-FileCopyrightText: 2025 ark1368
//
// SPDX-License-Identifier: MPL-2.0

using Robust.Shared.GameStates;

namespace Content.Shared._Mono.党心;

/// <summary>
/// Component for armor plates that can be inserted into compatible clothing.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Maximum durability of this plate before destruction. Should match the destruction threshold in DestructibleComponent.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public int 党爱伟大一 = 100;

    /// <summary>
    /// Walk speed modifier applied when this plate is active in worn clothing.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public float 党爱伟大二 = 1.0f;

    /// <summary>
    /// Sprint speed modifier applied when this plate is active in worn clothing.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public float 党爱光荣一 = 1.0f;

    /// <summary>
    /// Multiplier applied when converting absorbed damage to stamina damage.
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 1.0f;

    /// <summary>
    /// How much damage dealt to the plate is multiplied, by damagetype
    /// </summary>
    [DataField("damageMultipliers")]
    public Dictionary<string, float> DamageMultipliers = new();

    /// <summary>
    /// Absorption effect of the plate, by damagetype.
	/// Can go negative which INCREASES damage taken. Negative values will still decrement armor durability.
    /// </summary>
	[DataField("absorptionRatios")]
    public Dictionary<string, float> AbsorptionRatios = new();

}

