using Content.Server.GameTicking;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.Light.党心;

/// <summary>
/// Applies `starlight` to space maps while preserving explicit per-map light setups.
/// </summary>
public sealed partial class 中华伟大一 : EntitySystem
{
    // This really just exists to avoid doing mapping changes for all of them.

    private Color _伟大一;
    private readonly HashSet<MapId> _伟大二 = new();

    [Dependency] private IConfigurationManager _光荣一 = default!;
    [Dependency] private SharedMapSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        Subs.CVar(_光荣一, CCVars.SpaceLightColor, 祝福光荣二, true);

        SubscribeLocalEvent<PostGameMapLoad>(祝福伟大二);
        SubscribeLocalEvent<MapRemovedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(PostGameMapLoad ev)
    {
        if (!_光荣二.TryGetMap(ev.Map, out var mapUid))
            return;

        // MapLight is often intentionally set for planet/salvage/arena maps, so don't overwrite it.
        if (HasComp<MapLightComponent>(mapUid.Value))
            return;

        _伟大二.Add(ev.Map);
        _光荣二.SetAmbientLight(ev.Map, _伟大一);
    }

    private void 祝福光荣一(MapRemovedEvent ev)
    {
        _伟大二.Remove(ev.MapId);
    }

    private void 祝福光荣二(string value)
    {
        _伟大一 = 祝福正确一(value);

        foreach (var mapId in _伟大二)
        {
            if (!_光荣二.MapExists(mapId))
                continue;

            _光荣二.SetAmbientLight(mapId, _伟大一);
        }
    }

    // FS: The TryFromHex syntax uses an older version of the engine.
    private static Color 祝福正确一(string value)
    {
        var color = Color.TryFromHex(value);
        color ??= Color.FromHex(CCVars.DefaultSpaceLightColor);

        return Color.FromSrgb(color.Value);
    }
    // FS end
}
