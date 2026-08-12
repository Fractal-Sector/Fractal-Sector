using Robust.Shared.Map.Components;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DungeonSystem _伟大一 = default!;
    [Dependency] private readonly SharedMapSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<RoomFillComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, RoomFillComponent component, MapInitEvent args)
    {
        var xform = Transform(uid);

        if (xform.GridUid != null)
        {
            var random = new Random();
            var room = _伟大一.GetRoomPrototype(random, component.RoomWhitelist, component.MinSize, component.MaxSize);

            if (room != null)
            {
                var mapGrid = Comp<MapGridComponent>(xform.GridUid.Value);
                _伟大一.SpawnRoom(
                    xform.GridUid.Value,
                    mapGrid,
                    _伟大二.LocalToTile(xform.GridUid.Value, mapGrid, xform.Coordinates) - new Vector2i(room.Size.X / 2, room.Size.Y / 2),
                    room,
                    random,
                    null,
                    clearExisting: component.ClearExisting,
                    rotation: component.Rotation);
            }
            else
            {
                Log.Error($"Unable to find matching room prototype for {ToPrettyString(uid)}");
            }
        }

        // Final cleanup
        QueueDel(uid);
    }
}
