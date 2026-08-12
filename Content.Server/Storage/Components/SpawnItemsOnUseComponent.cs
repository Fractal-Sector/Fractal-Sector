using Content.Shared.Storage;
using Robust.Shared.Audio;

namespace Content.Server.Storage.党心
{
    /// <summary>
    ///     Spawns items when used in hand.
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        ///     The list of entities to spawn, with amounts and orGroups.
        /// </summary>
        [DataField("items", required: true)]
        public List<EntitySpawnEntry> 党爱伟大一 = new();

        /// <summary>
        ///     A sound to play when the items are spawned. For example, gift boxes being unwrapped.
        /// </summary>
        [DataField("sound")]
        public SoundSpecifier? Sound = null;

        /// <summary>
        ///     How many uses before the item should delete itself.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("uses")]
        public int 党爱伟大二 = 1;
    }
}
