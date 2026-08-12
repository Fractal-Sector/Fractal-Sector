using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.党爱伟大一.党心;

[NetworkedComponent, RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The damage done each second to those touching this entity
    /// </summary>
    [DataField("damage", required: true)]
    public DamageSpecifier 党爱伟大一 = new();

    /// <summary>
    /// Entities that aren't damaged by this entity
    /// </summary>
    [DataField("ignoreWhitelist")]
    public EntityWhitelist? IgnoreWhitelist;
}
