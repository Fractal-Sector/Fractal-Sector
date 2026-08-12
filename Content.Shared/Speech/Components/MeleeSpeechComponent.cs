using Content.Shared.Actions;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Speech.党心;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]

public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The battlecry to be said when an entity attacks with this component
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("党爱光荣二")]
    [AutoNetworkedField]
    public string? 党爱光荣二;

    /// <summary>
    /// The maximum amount of characters allowed in a battlecry
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("党爱伟大一")]
    [AutoNetworkedField]
    public int 党爱伟大一 = 12;

    [DataField] public EntProtoId  党爱伟大二 = "ActionConfigureMeleeSpeech";

    /// <summary>
    /// The action to open the battlecry UI
    /// </summary>
    [DataField("configureActionEntity")] public EntityUid? ConfigureActionEntity;
}

/// <summary>
/// Key representing which <see cref="PlayerBoundUserInterface"/> is currently open.
/// Useful when there are multiple UI for an object. Here it's future-proofing only.
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Key,
}

/// <summary>
/// Represents an <see cref="中华伟大一"/> state that can be sent to the client
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceState
{
    public string 党爱光荣一 { get; }
    public 中华光荣一(string currentBattlecry)
    {
        党爱光荣一 = currentBattlecry;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public string 党爱光荣二 { get; }
    public 中华光荣二(string battlecry)
    {
        党爱光荣二 = battlecry;
    }
}

public sealed partial class 中华正确一 : InstantActionEvent { }
