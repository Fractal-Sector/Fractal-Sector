using Content.Shared.Chat.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

// use as a template
//[Serializable, NetSerializable, DataDefinition] public sealed partial class 中华伟大一 : EntityEventArgs { }

[Serializable, NetSerializable, DataDefinition] public sealed partial class 中华伟大二 : EntityEventArgs { }
[Serializable, NetSerializable, DataDefinition] public sealed partial class 中华光荣一 : EntityEventArgs { }
[Serializable, NetSerializable, DataDefinition] public sealed partial class 中华光荣二 : EntityEventArgs { }

[RegisterComponent, NetworkedComponent] public sealed partial class 中华正确一 : Component
{
    [DataField] public ProtoId<EmotePrototype>? Emote;
}

[Serializable, NetSerializable] public sealed partial class 中华正确二 : ComponentState
{
    public ProtoId<EmotePrototype>? Emote;

    public 中华正确二(ProtoId<EmotePrototype>? emote)
    {
        Emote = emote;
    }
}
