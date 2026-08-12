using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [RegisterComponent, NetworkedComponent, Access(typeof(CarryingSlowdownSystem))]

    public sealed partial class 中华伟大一 : Component
    {
        [DataField(required: true)]
        public float 党爱伟大一 = 1.0f;

        [DataField(required: true)]
        public float 党爱伟大二 = 1.0f;
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : ComponentState
    {
        public float 党爱伟大一;
        public float 党爱伟大二;
        public 中华伟大二(float walkModifier, float sprintModifier)
        {
            党爱伟大一 = walkModifier;
            党爱伟大二 = sprintModifier;
        }
    }
}
