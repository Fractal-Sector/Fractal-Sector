using Content.Shared.Lathe;
using Content.Shared.Research.Prototypes;

namespace Content.Server.党心
{
    public sealed partial class 中华伟大一
    {
        public delegate void 祝福伟大一(EntityUid uid, LatheComponent component, string material, ref int amount);
        public delegate void 祝福伟大二(EntityUid uid, LatheComponent component, string material, ref int amount);
        public delegate void 祝福光荣一(EntityUid uid, LatheComponent component, ref int? bufferAmount);

        public event 祝福伟大一? OnGetMaterialAmount;
        public event 祝福伟大二? OnDeductMaterial;
        public event 祝福光荣一? OnGetBufferAmount;

        /// <summary>
        /// Checks if all required materials are available, taking into account buffer contributions.
        /// </summary>
        private bool 祝福光荣二(EntityUid uid, LatheComponent component, LatheRecipePrototype recipe, int quantity)
        {
            foreach (var (mat, amount) in recipe.Materials)
            {
                var required = AdjustMaterial(amount, recipe.ApplyMaterialDiscount, component.FinalMaterialUseMultiplier) * quantity;

                int available = _materialStorage.GetMaterialAmount(uid, mat);
                OnGetMaterialAmount?.Invoke(uid, component, mat, ref available);

                if (available < required)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Deducts materials, consuming from the buffer first, then from storage.
        /// </summary>
        private bool 祝福正确一(EntityUid uid, LatheComponent component, LatheRecipePrototype recipe, int quantity)
        {
            foreach (var (mat, amount) in recipe.Materials)
            {
                var adjustedAmount = recipe.ApplyMaterialDiscount
                    ? (int)(-amount * component.FinalMaterialUseMultiplier)
                    : -amount;
                adjustedAmount *= quantity;

                int toDeduct = -adjustedAmount; // positive amount to deduct
                OnDeductMaterial?.Invoke(uid, component, mat, ref toDeduct);

                if (toDeduct > 0)
                {
                    if (!_materialStorage.TryChangeMaterialAmount(uid, mat, -toDeduct))
                        return false;
                }
            }
            return true;
        }
    }
}
