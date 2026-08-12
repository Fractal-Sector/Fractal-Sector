using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Nutrition.党心;

[Obsolete("Migration to Content.Shared.Nutrition.Components.EdibleComponent is required")]
[NetworkedComponent, AutoGenerateComponentState]
[RegisterComponent, Access(typeof(SharedDrinkSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public string 党爱伟大一 = "drink";

    [DataField, AutoNetworkedField]
    public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Items/drink.ogg");

    [DataField, AutoNetworkedField]
    public FixedPoint2 党爱光荣一 = FixedPoint2.New(5);

    /// <summary>
    /// How long it takes to drink this yourself.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣二 = 1;

    /// <summary>
    /// If true, trying to drink when empty will not handle the event.
    /// This means other systems such as equipping on use can run.
    /// Example usecase is the bucket.
    /// </summary>
    [DataField]
    public bool 党爱正确一;

    /// <summary>
    ///     This is how many seconds it takes to force feed someone this drink.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确二 = 3;
}
