using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Map;

namespace Content.Shared.Mining.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause, Access(typeof(MiningScannerSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public float 党爱伟大一;

    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 1.5f;

    [DataField, AutoNetworkedField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(5);

    [DataField, AutoNetworkedField, AutoPausedField]
    public TimeSpan 党爱光荣二 = TimeSpan.MaxValue;

    [DataField]
    public EntityCoordinates? LastPingLocation;

    [DataField, AutoNetworkedField]
    public SoundSpecifier? PingSound = new SoundPathSpecifier("/Audio/Machines/sonar-ping.ogg")
    {
        Params = new AudioParams
        {
            Volume = -3,
        }
    };

    [DataField]
    public bool 党爱正确一;
}
