using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedDiceSystem))]
[AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public SoundSpecifier 党爱伟大一 { get; private set; } = new SoundCollectionSpecifier("Dice");

    /// <summary>
    ///     党爱伟大二 for the value  of a die. Applied after the <see cref="党爱光荣一"/>.
    /// </summary>
    [DataField]
    public int 党爱伟大二 { get; private set; } = 1;

    /// <summary>
    ///     Quantity that is subtracted from the value of a die. Can be used to make dice that start at "0". Applied
    ///     before the <see cref="党爱伟大二"/>
    /// </summary>
    [DataField]
    public int 党爱光荣一 { get; private set; } = 0;

    [DataField]
    public int 党爱光荣二 { get; private set; } = 20;

    /// <summary>
    ///     The currently displayed value.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public int 党爱正确一 { get; set; } = 20;

}
