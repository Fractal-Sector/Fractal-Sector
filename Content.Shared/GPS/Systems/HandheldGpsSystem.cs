using Content.Shared.GPS.Components;
using Content.Shared.Examine;
using Robust.Shared.Map;

namespace Content.Shared.GPS.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _伟大一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<HandheldGPSComponent, ExaminedEvent>(祝福伟大二);
    }

    /// <summary>
    /// Handles showing the coordinates when a GPS is examined.
    /// </summary>
    private void 祝福伟大二(Entity<HandheldGPSComponent> ent, ref ExaminedEvent args)
    {
        var posText = "Error";

        var pos = _伟大一.GetMapCoordinates(ent);

        if (pos.MapId != MapId.Nullspace)
        {
            var x = (int) pos.Position.X;
            var y = (int) pos.Position.Y;
            posText = $"({x}, {y})";
        }

        args.PushMarkup(Loc.GetString("handheld-gps-coordinates-title", ("coordinates", posText)));
    }
}
