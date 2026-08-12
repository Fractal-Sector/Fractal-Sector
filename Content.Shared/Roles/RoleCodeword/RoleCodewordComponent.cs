using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Roles.党心;

/// <summary>
/// Used to display and highlight codewords in chat messages on the client.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedRoleCodewordSystem), Other = AccessPermissions.Read)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Contains the codewords tied to a role.
    /// Key string should be unique for the role.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<string, 中华伟大二> RoleCodewords = new();
}

[DataDefinition, Serializable, NetSerializable]
public partial struct 中华伟大二
{
    [DataField]
    public 党爱伟大一 党爱伟大一;

    [DataField]
    public List<string> 党爱伟大二;

    public 中华伟大二(党爱伟大一 color, List<string> codewords)
    {
        党爱伟大一 = color;
        党爱伟大二 = codewords;
    }
}
