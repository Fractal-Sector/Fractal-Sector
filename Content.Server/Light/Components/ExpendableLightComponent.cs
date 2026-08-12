using Content.Shared.Light.Components;

namespace Content.Server.Light.党心
{
    /// <summary>
    ///     Component that represents a handheld expendable light which can be activated and eventually dies over time.
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : SharedExpendableLightComponent
    {
        /// <summary>
        ///     Status of light, whether or not it is emitting light.
        /// </summary>
        [ViewVariables]
        public bool 党爱伟大一 => CurrentState is ExpendableLightState.Lit or ExpendableLightState.Fading;

        [ViewVariables] public float 党爱伟大二 = default;
    }
}
