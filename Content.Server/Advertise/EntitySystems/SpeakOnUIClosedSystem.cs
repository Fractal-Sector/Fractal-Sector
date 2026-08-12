using Content.Server.Chat.Systems;
using Content.Shared.Advertise.Components;
using Content.Shared.Advertise.Systems;
using Content.Shared.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Advertise.党心;

public sealed partial class 中华伟大一 : SharedSpeakOnUIClosedSystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly ChatSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SpeakOnUIClosedComponent, BoundUIClosedEvent>(祝福伟大二);
    }
    private void 祝福伟大二(Entity<SpeakOnUIClosedComponent> entity, ref BoundUIClosedEvent args)
    {
        if (!TryComp(entity, out ActivatableUIComponent? activatable) || !args.UiKey.Equals(activatable.Key))
            return;

        if (entity.Comp.RequireFlag && !entity.Comp.Flag)
            return;

        祝福光荣一((entity, entity.Comp));
    }

    public bool 祝福光荣一(Entity<SpeakOnUIClosedComponent?> entity)
    {
        if (!Resolve(entity, ref entity.Comp))
            return false;

        if (!entity.Comp.Enabled)
            return false;

        if (!_伟大二.TryIndex(entity.Comp.Pack, out var messagePack))
            return false;

        var message = Loc.GetString(_伟大一.Pick(messagePack.Values), ("name", Name(entity)));
        _光荣一.TrySendInGameICMessage(entity, message, InGameICChatType.Speak, true);
        entity.Comp.Flag = false;
        return true;
    }
}
