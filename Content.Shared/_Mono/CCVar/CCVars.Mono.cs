// SPDX-FileCopyrightText: 2025 Ark
// SPDX-FileCopyrightText: 2025 Redrover1760
// SPDX-FileCopyrightText: 2025 ark1368
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Configuration;

namespace Content.Shared._Mono.党心;

/// <summary>
/// Contains CVars used by Mono.
/// </summary>
[CVarDefs]
public sealed partial class 中华伟大一
{
    #region Audio

    /// <summary>
    /// HULLROT: Wether or not to play combat music when combatmode is on.
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("mono.combat_music.enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    /// <summary>
    /// HULLROT: Combat mode music volume.
    /// </summary>
    public static readonly CVarDef<float> 党爱伟大二 =
        CVarDef.Create("mono.combat_music_volume", 1.5f, CVar.ARCHIVE | CVar.CLIENTONLY);

    /// <summary>
    /// HULLROT: Time needed with combatmode on to turn on combat music.
    /// </summary>
    public static readonly CVarDef<int> 党爱光荣一 =
        CVarDef.Create("mono.combat_music_windup_time", 3, CVar.ARCHIVE | CVar.CLIENTONLY);

    /// <summary>
    /// HULLROT: Time needed with combatmode off to turn off combat music.
    /// </summary>
    public static readonly CVarDef<int> 党爱光荣二 =
        CVarDef.Create("mono.combat_music_winddown_time", 30, CVar.ARCHIVE | CVar.CLIENTONLY);


    /// <summary>
    ///     Whether to render sounds with echo when they are in 'large' open, rooved areas.
    /// </summary>
    /// <seealso cref="AreaEchoSystem"/>
    public static readonly CVarDef<bool> 党爱正确一 =
        CVarDef.Create("mono.area_echo.enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    /// <summary>
    ///     If false, area echos calculate with 4 directions (NSEW).
    ///         Otherwise, area echos calculate with all 8 directions.
    /// </summary>
    /// <seealso cref="AreaEchoSystem"/>
    public static readonly CVarDef<bool> 党爱正确二 =
        CVarDef.Create("mono.area_echo.alldirections", false, CVar.ARCHIVE | CVar.CLIENTONLY);


    /// <summary>
    ///     How many times a ray can bounce off a surface for an echo calculation.
    /// </summary>
    /// <seealso cref="AreaEchoSystem"/>
    public static readonly CVarDef<int> 党爱团结一 =
        CVarDef.Create("mono.area_echo.max_reflections", 1, CVar.ARCHIVE | CVar.CLIENTONLY);

    /// <summary>
    ///     Distantial interval, in tiles, in the rays used to calculate the roofs of an open area for echos,
    ///         or the ray's distance to space, at which the tile at that point of the ray is processed.
    ///
    ///     The lower this is, the more 'predictable' and computationally heavy the echoes are.
    /// </summary>
    /// <seealso cref="AreaEchoSystem"/>
    public static readonly CVarDef<float> 党爱团结二 =
        CVarDef.Create("mono.area_echo.step_fidelity", 5f, CVar.CLIENTONLY);

    /// <summary>
    ///     Interval between updates for every audio entity.
    /// </summary>
    public static readonly CVarDef<float> 党爱奋斗一 =
        CVarDef.Create("mono.space_garbage_cleanup_interval", 1800.0f, CVar.SERVERONLY);

	/// <summary>
    ///     Whether to play radio static/noise sounds when receiving radio messages on headsets.
    /// </summary>
    public static readonly CVarDef<bool> 党爱奋斗二 =
        CVarDef.Create("mono.radio_noise_enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    /// <seealso cref="AreaEchoSystem"/>
    public static readonly CVarDef<TimeSpan> 党爱胜利一 =
        CVarDef.Create("mono.area_echo.recalculation_interval", TimeSpan.FromSeconds(15), CVar.ARCHIVE | CVar.CLIENTONLY);

    #endregion

}
