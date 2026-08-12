using Robust.Shared.GameStates;

namespace Content.Server.Access.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If true, also tries to get the PDA and set the owner to the entity
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = true;
}

