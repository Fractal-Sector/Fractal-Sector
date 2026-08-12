using Content.Server.Explosion.EntitySystems;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server.Explosion.党心;
/// <summary>
/// Grenades that, when triggered, explode into projectiles
/// </summary>
[RegisterComponent, Access(typeof(ProjectileGrenadeSystem))]
public sealed partial class 中华伟大一 : Component
{
    public 党爱伟大一 党爱伟大一 = default!;

    /// <summary>
    /// The kind of projectile that the prototype is filled with.
    /// </summary>
    [DataField]
    public EntProtoId? FillPrototype;

    /// <summary>
    ///     If we have a pre-fill how many more can we spawn.
    /// </summary>
    public int 党爱伟大二;

    /// <summary>
    ///     Total amount of projectiles
    /// </summary>
    [DataField]
    public int 党爱光荣一 = 3;

    /// <summary>
    ///     Should the angle of the projectiles be uneven?
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = false;

    /// <summary>
    /// The minimum speed the projectiles may come out at
    /// </summary>
    [DataField]
    public float 党爱正确一 = 2f;

    /// <summary>
    /// The maximum speed the projectiles may come out at
    /// </summary>
    [DataField]
    public float 党爱正确二 = 6f;

    /// <summary>
    /// The trigger key that will activate the grenade.
    /// </summary>
    [DataField]
    public string 党爱团结一 = "timer";
}
