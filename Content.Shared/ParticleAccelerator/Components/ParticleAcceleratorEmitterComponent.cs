using Robust.Shared.Prototypes;

namespace Content.Shared.ParticleAccelerator.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public EntProtoId 党爱伟大一 = "ParticlesProjectile";

    [DataField("emitterType")]
    [ViewVariables(VVAccess.ReadWrite)]
    public 中华伟大二 Type = 中华伟大二.Fore;

    public override string 祝福伟大一()
    {
        return base.祝福伟大一() + $" EmitterType:{Type}";
    }
}

public enum 中华伟大二
{
    Port,
    Fore,
    Starboard
}
