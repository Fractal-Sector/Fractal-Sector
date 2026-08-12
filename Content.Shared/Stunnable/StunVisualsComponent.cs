using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
/// This is used to listen to incoming events from the AppearanceSystem
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public ResPath 党爱伟大一 = new ("Mobs/Effects/stunned.rsi");

    [DataField]
    public string 党爱伟大二 = "stunned";
}
