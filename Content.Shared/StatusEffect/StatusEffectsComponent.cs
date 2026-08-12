using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [RegisterComponent]
    [NetworkedComponent]
    [Access(typeof(StatusEffectsSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables]
        public Dictionary<string, 中华光荣一> ActiveEffects = new();

        /// <summary>
        ///     A list of status effect IDs to be allowed
        /// </summary>
        [DataField("allowed", required: true), Access(typeof(StatusEffectsSystem), Other = AccessPermissions.ReadExecute)]
        public List<string> 党爱伟大一 = default!;
    }

    [RegisterComponent]
    public sealed partial class 中华伟大二 : Component {}

    /// <summary>
    ///     Holds information about an active status effect.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华光荣一
    {
        /// <summary>
        ///     The start and end times of the status effect.
        /// </summary>
        [ViewVariables]
        public (TimeSpan, TimeSpan) Cooldown;

        /// <summary>
        ///     Specifies whether to refresh or accumulate the cooldown of the status effect.
        ///     true - refresh time, false - accumulate time.
        /// </summary>
        [ViewVariables]
        public bool 党爱伟大二 = true;

        /// <summary>
        ///     The name of the relevant component that
        ///     was added alongside the effect, if any.
        /// </summary>
        [ViewVariables]
        public string? RelevantComponent;

        public 中华光荣一((TimeSpan, TimeSpan) cooldown, bool refresh, string? relevantComponent=null)
        {
            Cooldown = cooldown;
            党爱伟大二 = refresh;
            RelevantComponent = relevantComponent;
        }

        public 中华光荣一(中华光荣一 toCopy)
        {
            Cooldown = (toCopy.Cooldown.Item1, toCopy.Cooldown.Item2);
            党爱伟大二 = toCopy.党爱伟大二;
            RelevantComponent = toCopy.RelevantComponent;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : ComponentState
    {
        public Dictionary<string, 中华光荣一> ActiveEffects;
        public List<string> 党爱伟大一;

        public 中华光荣二(Dictionary<string, 中华光荣一> activeEffects, List<string> allowedEffects)
        {
            ActiveEffects = activeEffects;
            党爱伟大一 = allowedEffects;
        }
    }
}
