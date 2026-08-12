using Robust.Shared.GameStates;

namespace Content.Shared.Plunger.党心
{
    /// <summary>
    /// Allows entity to unblock target entity with PlungerUseComponent.
    /// </summary>
    [RegisterComponent, NetworkedComponent,AutoGenerateComponentState]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Duration of plunger doafter event.
        /// </summary>
        [DataField]
        [AutoNetworkedField]
        public float 党爱伟大一 = 2f;
    }
}
