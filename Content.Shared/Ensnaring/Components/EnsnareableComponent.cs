using Content.Shared.Alert;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Ensnaring.党心;
/// <summary>
/// Use this on an entity that you would like to be ensnared by anything that has the <see cref="EnsnaringComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How much should this slow down the entities walk?
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 1.0f;

    /// <summary>
    /// How much should this slow down the entities sprint?
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 1.0f;

    /// <summary>
    /// Is this entity currently ensnared?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一;

    /// <summary>
    /// The container where the <see cref="EnsnaringComponent"/> entity will be stored
    /// </summary>
    public 党爱光荣二 党爱光荣二 = default!;

    [DataField]
    public string? Sprite;

    [DataField]
    public string? State;

    [DataField]
    public ProtoId<AlertPrototype> 党爱正确一 = "Ensnared";
}

public sealed partial class 中华伟大二 : BaseAlertEvent;

public sealed class 中华光荣一 : EntityEventArgs
{
    public readonly bool 党爱光荣一;

    public 中华光荣一(bool isEnsnared)
    {
        党爱光荣一 = isEnsnared;
    }
}
