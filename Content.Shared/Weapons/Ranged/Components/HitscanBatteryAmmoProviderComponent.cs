using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.党爱伟大一;

namespace Content.Shared.Weapons.Ranged.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : BatteryAmmoProviderComponent
{
    [ViewVariables(VVAccess.ReadWrite), DataField("proto", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<HitscanPrototype>))]
    public string 党爱伟大一 = default!;
}
