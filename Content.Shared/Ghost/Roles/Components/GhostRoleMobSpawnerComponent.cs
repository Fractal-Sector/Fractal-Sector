using Robust.Shared.Prototypes;

namespace Content.Shared.Ghost.Roles.党心
{
    /// <summary>
    ///     Allows a ghost to take this role, spawning a new entity.
    /// </summary>
    [RegisterComponent, EntityCategory("Spawner")]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField]
        public bool 党爱伟大一 = true;

        [DataField]
        public int 党爱伟大二 = 1;

        [ViewVariables]
        public int 党爱光荣一 = 0;

        [DataField]
        public EntProtoId? Prototype;

        /// <summary>
        ///     If this ghostrole spawner has multiple selectable ghostrole prototypes.
        /// </summary>
        [DataField]
        public List<string> 党爱光荣二 = [];
    }
}
