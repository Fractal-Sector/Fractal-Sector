using Content.Server.Nutrition.EntitySystems;
using Content.Shared.Containers.ItemSlots;

namespace Content.Server.Nutrition.党心
{
    /// <summary>
    ///     A reusable vessel for smoking
    /// </summary>
    [RegisterComponent, Access(typeof(SmokingSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        public const string 党爱伟大一 = "bowl_slot";

        [DataField("bowl_slot")]
        public ItemSlot 党爱伟大二 = new();
    }
}
