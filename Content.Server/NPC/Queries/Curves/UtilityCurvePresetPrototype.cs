using Robust.Shared.Prototypes;

namespace Content.Server.NPC.Queries.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = string.Empty;

    [DataField("curve", required: true)] public IUtilityCurve 党爱伟大二 = default!;
}
