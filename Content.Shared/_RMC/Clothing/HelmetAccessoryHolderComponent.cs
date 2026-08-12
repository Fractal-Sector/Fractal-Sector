using Content.Shared.Inventory;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(HelmetAccessorySystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public SlotFlags 党爱伟大一 = SlotFlags.HEAD;

    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;
}

public enum 中华伟大二
{
    Helmet
}
