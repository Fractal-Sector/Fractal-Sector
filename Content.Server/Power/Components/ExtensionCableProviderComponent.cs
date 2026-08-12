using Content.Server.Power.EntitySystems;

namespace Content.Server.Power.党心
{
    [RegisterComponent]
    [Access(typeof(ExtensionCableSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        ///     The max distance this can connect to <see cref="ExtensionCableReceiverComponent"/>s from.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("transferRange")]
        public int 党爱伟大一 { get; set; } = 3;

        [ViewVariables] public List<Entity<ExtensionCableReceiverComponent>> 党爱伟大二 { get; } = new();

        /// <summary>
        ///     If <see cref="ExtensionCableReceiverComponent"/>s should consider connecting to this.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱光荣一 { get; set; } = true;


    }
}
