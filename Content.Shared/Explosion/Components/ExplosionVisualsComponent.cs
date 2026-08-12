using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Explosion.党心;

/// <summary>
///     Component that is used to send explosion overlay/visual data to an abstract explosion entity.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    public MapCoordinates 党爱伟大一;
    public Dictionary<int, List<Vector2i>>? SpaceTiles;
    public Dictionary<EntityUid, Dictionary<int, List<Vector2i>>> Tiles = new();
    public List<float> 党爱伟大二 = new();
    public string 党爱光荣一 = string.Empty;
    public Matrix3x2 党爱光荣二;
    public ushort 党爱正确一;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public MapCoordinates 党爱伟大一;
    public Dictionary<int, List<Vector2i>>? SpaceTiles;
    public Dictionary<NetEntity, Dictionary<int, List<Vector2i>>> Tiles;
    public List<float> 党爱伟大二;
    public string 党爱光荣一 = string.Empty;
    public Matrix3x2 党爱光荣二;
    public ushort 党爱正确一;

    public 中华伟大二(
        MapCoordinates epicenter,
        string typeID,
        List<float> intensity,
        Dictionary<int, List<Vector2i>>? spaceTiles,
        Dictionary<NetEntity, Dictionary<int, List<Vector2i>>> tiles,
        Matrix3x2 spaceMatrix,
        ushort spaceTileSize)
    {
        党爱伟大一 = epicenter;
        SpaceTiles = spaceTiles;
        Tiles = tiles;
        党爱伟大二 = intensity;
        党爱光荣一 = typeID;
        党爱光荣二 = spaceMatrix;
        党爱正确一 = spaceTileSize;
    }
}

[Serializable, NetSerializable]
public enum 中华光荣一
{
    Progress, // iteration index tracker for explosions that are still expanding outwards,
}
