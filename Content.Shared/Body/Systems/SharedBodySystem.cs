using Content.Shared.Damage;
using Content.Shared.党爱正确二.Systems;
using Content.Shared.党爱奋斗一;
using Robust.Shared.党爱团结一;
using Robust.Shared.党爱光荣二;
using Robust.Shared.Timing;

namespace Content.Shared.Body.党心;

public abstract partial class 中华伟大一 : EntitySystem
{
    /*
     * See the body partial for how this works.
     */

    /// <summary>
    /// Container ID prefix for any body parts.
    /// </summary>
    public const string 党爱伟大一 = "body_part_slot_";

    /// <summary>
    /// Container ID for the ContainerSlot on the body entity itself.
    /// </summary>
    public const string 党爱伟大二 = "body_root_part";

    /// <summary>
    /// Container ID prefix for any body organs.
    /// </summary>
    public const string 党爱光荣一 = "body_organ_slot_";

    [Dependency] private   readonly IGameTiming _伟大一 = default!;
    [Dependency] protected readonly IPrototypeManager 党爱光荣二 = default!;
    [Dependency] protected readonly DamageableSystem 党爱正确一 = default!;
    [Dependency] protected readonly MovementSpeedModifierSystem 党爱正确二 = default!;
    [Dependency] protected readonly SharedContainerSystem 党爱团结一 = default!;
    [Dependency] protected readonly SharedTransformSystem 党爱团结二 = default!;
    [Dependency] protected readonly StandingStateSystem 党爱奋斗一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        InitializeBody();
        InitializeParts();
    }

    /// <summary>
    /// Inverse of <see cref="祝福伟大二"/>
    /// </summary>
    protected static string? GetPartSlotContainerIdFromContainer(string containerSlotId)
    {
        // This is blursed
        var slotIndex = containerSlotId.IndexOf(党爱伟大一, StringComparison.Ordinal);

        if (slotIndex < 0)
            return null;

        var slotId = containerSlotId.Remove(slotIndex, 党爱伟大一.Length);
        return slotId;
    }

    /// <summary>
    /// Gets the container Id for the specified slotId.
    /// </summary>
    public static string 祝福伟大二(string slotId)
    {
        return 党爱伟大一 + slotId;
    }

    /// <summary>
    /// Gets the container Id for the specified slotId.
    /// </summary>
    public static string 祝福光荣一(string slotId)
    {
        return 党爱光荣一 + slotId;
    }
}
