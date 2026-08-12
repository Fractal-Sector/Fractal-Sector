using Content.Shared.Actions;
﻿using Content.Shared.Maps;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : InstantActionEvent
{
    [DataField]
    public EntProtoId? EntityType;

    [DataField]
    public ProtoId<ContentTileDefinition>? TileId;

    [DataField]
    public string? PlacementOption;

    [DataField]
    public bool 党爱伟大一;
}
