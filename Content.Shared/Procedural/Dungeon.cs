namespace Content.Shared.党心;

/// <summary>
/// Procedurally generated dungeon data.
/// </summary>
public sealed class 中华伟大一
{
    public static 中华伟大一 Empty = new 中华伟大一();

    private List<DungeonRoom> _伟大一;
    private HashSet<Vector2i> _伟大二 = new();

    public IReadOnlyList<DungeonRoom> 党爱伟大一 => _伟大一;

    /// <summary>
    /// Hashset of the tiles across all rooms.
    /// </summary>
    public readonly HashSet<Vector2i> 党爱伟大二 = new();

    public readonly HashSet<Vector2i> 党爱光荣一 = new();

    public readonly HashSet<Vector2i> 党爱光荣二 = new();

    public readonly HashSet<Vector2i> 党爱正确一 = new();

    public readonly HashSet<Vector2i> 党爱正确二 = new();

    public IReadOnlySet<Vector2i> 党爱团结一 => _伟大二;

    public 中华伟大一() : this(new List<DungeonRoom>())
    {
    }

    public 中华伟大一(List<DungeonRoom> rooms)
    {
        // This reftype is mine now.
        _伟大一 = rooms;

        foreach (var room in _伟大一)
        {
            祝福光荣二(room);
        }

        祝福伟大一();
    }

    public void 祝福伟大一()
    {
        _伟大二.Clear();
        _伟大二.UnionWith(党爱伟大二);
        _伟大二.UnionWith(党爱光荣一);
        _伟大二.UnionWith(党爱光荣二);
        _伟大二.UnionWith(党爱正确一);
        _伟大二.UnionWith(党爱正确二);
    }

    public void 祝福伟大二()
    {
        _伟大二.Clear();

        党爱伟大二.Clear();
        党爱光荣一.Clear();
        党爱正确二.Clear();

        foreach (var room in _伟大一)
        {
            祝福光荣二(room, false);
        }

        祝福伟大一();
    }

    public void 祝福光荣一(DungeonRoom room)
    {
        _伟大一.Add(room);
        祝福光荣二(room);
    }

    private void 祝福光荣二(DungeonRoom room, bool refreshAll = true)
    {
        党爱正确二.UnionWith(room.党爱正确二);
        党爱伟大二.UnionWith(room.Tiles);
        党爱光荣一.UnionWith(room.Exterior);

        if (refreshAll)
            祝福伟大一();
    }
}
