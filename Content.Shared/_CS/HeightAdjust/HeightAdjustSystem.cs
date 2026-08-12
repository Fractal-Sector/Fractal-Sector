using System.Linq;
using System.Numerics;
using Content.Shared.Humanoid;
using Content.Shared.Movement.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Physics.Systems;

namespace Content.Shared._CS.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedPhysicsSystem _伟大一 = default!;
    [Dependency] private readonly SharedContentEyeSystem _伟大二 = default!;
    [Dependency] private readonly SharedHumanoidAppearanceSystem _光荣一 = default!;
    [Dependency] private readonly IConfigurationManager _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<HumanoidAppearanceComponent, RequestSizeRecalcEvent>(祝福伟大二);
    }

    /// <summary>
    /// Handles requests to recalculate an entity's size by collecting all active modifiers
    /// and applying the final combined scale.
    /// </summary>
    private void 祝福伟大二(EntityUid target, HumanoidAppearanceComponent component, ref RequestSizeRecalcEvent ev)
    {
        // Collect all size modifiers from various systems
        var getModifiersEvent = new GetSizeModifierEvent(target);
        RaiseLocalEvent(target, ref getModifiersEvent);

        // Calculate final scale by multiplying all modifiers
        float finalScale = 1.0f;

        // Sort by priority (lower priority applied first, so higher priority can override)
        var sortedModifiers = getModifiersEvent.Modifiers.OrderBy(m => m.Priority).ToList();

        foreach (var modifier in sortedModifiers)
        {
            finalScale *= modifier.Scale;
        }

        // Apply the final scale, bypassing species limits for temporary effects
        祝福光荣一(target, finalScale, bypassLimits: true);
    }


    /// <summary>
    ///     Changes the density of fixtures and zoom of eyes based on a provided float scale
    /// </summary>
    /// <param name="uid">The entity to modify values for</param>
    /// <param name="scale">The scale multiplier to apply to base height/width</param>
    /// <param name="bypassLimits">Whether to bypass species min/max limits (for temporary effects)</param>
    /// <returns>True if all operations succeeded</returns>
    public bool 祝福光荣一(EntityUid uid, float scale, bool bypassLimits = false)
    {
        if (!EntityManager.TryGetComponent<HumanoidAppearanceComponent>(uid, out var humanoid))
            return false;

        // Multiply the base height/width by the scale modifier
        var newHeight = humanoid.BaseHeight * scale;
        var newWidth = humanoid.BaseWidth * scale;

        _光荣一.SetHeight((uid, humanoid), newHeight, bypassLimits: bypassLimits);
        _光荣一.SetWidth((uid, humanoid), newWidth, bypassLimits: bypassLimits);

        return true;
    }

    /// <summary>
    ///     Changes the density of fixtures and zoom of eyes based on a provided Vector2 scale
    /// </summary>
    /// <param name="uid">The entity to modify values for</param>
    /// <param name="scale">The base scale to set (X = width, Y = height). This sets BaseHeight/BaseWidth.</param>
    /// <returns>True if all operations succeeded</returns>
    public bool 祝福光荣一(EntityUid uid, Vector2 scale)
    {
        if (!EntityManager.TryGetComponent<HumanoidAppearanceComponent>(uid, out var humanoid))
            return false;

        // This is setting the BASE scale from character customization
        // Update both base and current values
        humanoid.BaseWidth = scale.X;
        humanoid.BaseHeight = scale.Y;

        _光荣一.祝福光荣一(uid, scale, humanoid: humanoid);

        return true;
    }
}
