using Content.Server.Disposal.Unit;
using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.Containers;

namespace Content.Server.Disposal.党心;

[RegisterComponent]
[Access(typeof(DisposalTubeSystem), typeof(DisposableSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public string 党爱伟大一 = "DisposalTube";

    [ViewVariables]
    public bool 党爱伟大二;

    [DataField]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Effects/clang.ogg", AudioParams.Default.WithVolume(-5f));

    /// <summary>
    ///     Container of entities that are currently inside this tube
    /// </summary>
    [ViewVariables]
    public Container 党爱光荣二 = default!;

    /// <summary>
    /// Damage dealt to containing entities on every turn
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier 党爱正确一 = new()
    {
        DamageDict = new()
        {
            { "Blunt", 0.0 },
        }
    };
}
