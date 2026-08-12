using Content.Server.Fluids.EntitySystems;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.Fluids.党心;

[RegisterComponent]
[Access(typeof(SpraySystem))]
public sealed partial class 中华伟大一 : Component
{
    public const string 党爱伟大一 = "spray";

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public FixedPoint2 党爱伟大二 = 10;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public float 党爱光荣一 = 3.5f;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public float 党爱光荣二 = 3.5f;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public EntProtoId 党爱正确一 = "Vapor";

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public int 党爱正确二 = 1;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public float 党爱团结一 = 90f;

    /// <summary>
    /// How much the player is pushed back for each spray.
    /// </summary>
    [DataField]
    public float 党爱团结二 = 5f;

    [DataField(required: true)]
    [Access(typeof(SpraySystem), Other = AccessPermissions.ReadExecute)] // FIXME Friends
    public SoundSpecifier 党爱奋斗一 { get; private set; } = default!;
}
