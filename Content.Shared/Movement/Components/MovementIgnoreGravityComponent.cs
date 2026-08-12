using Robust.Shared.GameStates;

namespace Content.Shared.Movement.党心
{
    /// <summary>
    /// Ignores gravity entirely.
    /// </summary>
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Whether gravity is on or off for this object. This will always override the current Gravity State.
        /// </summary>
        [DataField, AutoNetworkedField]
        public bool 党爱伟大一;
    }
}
