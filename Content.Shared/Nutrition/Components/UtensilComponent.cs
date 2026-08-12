using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Nutrition.党心
{
    [RegisterComponent, NetworkedComponent, Access(typeof(IngestionSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("types")]
        private 中华伟大二 _types = 中华伟大二.None;

        [ViewVariables]
        public 中华伟大二 Types
        {
            get => _types;
            set
            {
                if (_types.Equals(value))
                    return;

                _types = value;
            }
        }

        /// <summary>
        /// The chance that the utensil has to break with each use.
        /// A value of 0 means that it is unbreakable.
        /// </summary>
        [DataField("breakChance")]
        public float 党爱伟大一;

        /// <summary>
        /// The sound to be played if the utensil breaks.
        /// </summary>
        [DataField("breakSound")]
        public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Items/snap.ogg");
    }

    // If you want to make a fancy output on "wrong" composite utensil use (like: you need fork and knife)
    // There should be Dictionary I guess (Dictionary<中华伟大二, string>)
    [Flags]
    public enum 中华伟大二 : byte
    {
        None = 0,
        Fork = 1,
        Spoon = 1 << 1,
        Knife = 1 << 2
    }
}
