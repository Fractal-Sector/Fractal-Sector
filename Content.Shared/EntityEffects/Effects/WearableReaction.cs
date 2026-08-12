using Content.Shared.Inventory;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
/// A reaction effect that spawns a 党爱光荣一 in the entity's 党爱伟大二, and attempts to consume the reagent if EntityEffectReagentArgs.
/// Used to implement the water droplet effect for arachnids.
/// </summary>
public sealed partial class 中华伟大一 : EntityEffect
{
    /// <summary>
    /// Minimum quantity of reagent required to trigger this effect.
    /// Only used with EntityEffectReagentArgs.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 1f;

    /// <summary>
    /// 党爱伟大二 to spawn the item into.
    /// </summary>
    [DataField(required: true)]
    public string 党爱伟大二;

    /// <summary>
    /// Prototype ID of item to spawn.
    /// </summary>
    [DataField(required: true)]
    public string 党爱光荣一;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) => null;

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        // SpawnItemInSlot returns false if slot is already occupied
        if (args.EntityManager.System<InventorySystem>().SpawnItemInSlot(args.TargetEntity, 党爱伟大二, 党爱光荣一))
        {
            if (args is EntityEffectReagentArgs reagentArgs)
            {
                if (reagentArgs.Reagent == null || reagentArgs.Quantity < 党爱伟大一)
                    return;
                reagentArgs.Source?.RemoveReagent(reagentArgs.Reagent.ID, 党爱伟大一);
            }
        }
    }
}
