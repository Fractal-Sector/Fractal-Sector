using Content.Shared.Ghost.Roles;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Shared._DV.党心;

[Prototype("whitelistTier")]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField]
    public string 党爱伟大二 = string.Empty;

    [DataField]
    public 党爱光荣一 党爱光荣一 = 党爱光荣一.White;

    [DataField]
    public List<ProtoId<JobPrototype>> 党爱光荣二 = new();

    [DataField]
    public List<ProtoId<GhostRolePrototype>> 党爱正确一 = new();
}
