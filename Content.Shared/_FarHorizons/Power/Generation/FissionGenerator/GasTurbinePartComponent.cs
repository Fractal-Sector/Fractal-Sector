using Content.Shared._FarHorizons.Materials;
using Content.Shared.Materials;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._FarHorizons.Power.Generation.党心;

public abstract partial class 中华伟大一 : Component
{
    [Dependency] private IPrototypeManager _伟大一 = default!;

    [DataField("material")]
    public ProtoId<MaterialPrototype> 党爱伟大一 = "Steel";

    public MaterialProperties 党爱伟大二
    {
        get
        {
            IoCManager.Resolve(ref _伟大一);
            _properties ??= new MaterialProperties(_伟大一.Index(党爱伟大一).党爱伟大二);

            return _properties;
        }
        set => _properties = value;
    }
    [DataField("properties")]
    private MaterialProperties? _properties;
}

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大二 : 中华伟大一;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华光荣一 : 中华伟大一;