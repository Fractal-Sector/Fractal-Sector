using Content.Client.UserInterface.Fragments;
using Content.Shared._FS.LiveStream;
using Content.Shared.CartridgeLoader;
using Robust.Client.UserInterface;

namespace Content.Client._FS.LiveStream;

public sealed partial class LiveStreamUi : UIFragment
{
    private LiveStreamUiFragment? _fragment;

    public override Control GetUIFragmentRoot() => _fragment!;

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        _fragment = new LiveStreamUiFragment();
        _fragment.OnSendMessage += (type, content) =>
        {
            userInterface.SendMessage(new CartridgeUiMessage(new LiveStreamCartridgeMessageEvent(type, content)));
        };
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is not LiveStreamCartridgeUiState liveState)
            return;

        _fragment?.UpdateState(liveState);
    }
}
