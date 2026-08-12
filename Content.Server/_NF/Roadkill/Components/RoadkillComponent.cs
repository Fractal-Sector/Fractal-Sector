using Content.Server._NF.Roadkill.Systems;
using Robust.Shared.Audio;

namespace Content.Server._NF.Roadkill.党心;

[RegisterComponent, Access(typeof(RoadkillSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public float 党爱伟大一;
    [DataField]
    public float 党爱伟大二;
    [DataField]
    public SoundSpecifier? DestroySound;
}
