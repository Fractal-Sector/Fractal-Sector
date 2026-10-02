using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Nyanotrasen.Item.PseudoItem;
using Robust.Shared.Prototypes;

namespace Content.Server._FS.HeightStorage;

/// <summary>
/// Ties the height/width character slider (see <see cref="SharedHumanoidAppearanceSystem"/>) to the
/// PseudoItem system: a humanoid dialed small enough becomes stashable in storage like an item, the
/// same way a mouse or hamster already is. Replaces the older, race-limited "Short" trait, which stayed
/// disabled in favor of this - it applies to any species/height combination instead of just two races.
/// Server-only, like the "Short" trait's old SizeAttributeSystem, since it adds/removes a networked
/// component and that lifecycle should stay server-authoritative.
/// </summary>
public sealed class HeightPseudoItemSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;

    /// <summary>
    /// A humanoid counts as "small" once their actual height (species AverageHeight * Height
    /// multiplier) drops to or below this, in cm. Tune here if it feels too easy/hard to qualify.
    /// </summary>
    private const float SmallHeightThresholdCm = 175f;

    /// <summary>
    /// Grid shape for a stashed small humanoid: 6 wide x 2 tall (12 cells). Fits entirely inside a
    /// regular backpack's grid (0,0,6,3) and takes up about half a duffel bag's grid (0,0,7,4), so two
    /// can be stashed in the same duffel bag.
    /// </summary>
    private static readonly List<Box2i> SmallShape = new()
    {
        new Box2i(0, 0, 6, 2),
    };

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HumanoidAppearanceComponent, HumanoidHeightChangedEvent>(OnHeightChanged);
        SubscribeLocalEvent<HumanoidAppearanceComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, HumanoidAppearanceComponent component, ComponentStartup args)
    {
        UpdatePseudoItem(uid, component);
    }

    private void OnHeightChanged(EntityUid uid, HumanoidAppearanceComponent component, HumanoidHeightChangedEvent args)
    {
        UpdatePseudoItem(uid, component);
    }

    private void UpdatePseudoItem(EntityUid uid, HumanoidAppearanceComponent component)
    {
        if (!_proto.TryIndex<SpeciesPrototype>(component.Species, out var species))
            return;

        var heightCm = component.Height * species.AverageHeight;
        var isSmall = heightCm <= SmallHeightThresholdCm;

        if (isSmall)
        {
            if (!HasComp<PseudoItemComponent>(uid))
            {
                var pseudoItem = AddComp<PseudoItemComponent>(uid);
                pseudoItem.Shape = SmallShape;
            }
        }
        else if (TryComp<PseudoItemComponent>(uid, out var existing) && !existing.Active)
        {
            // Don't rip the component away mid-stash (e.g. size gun fired on someone already in a bag).
            RemComp<PseudoItemComponent>(uid);
        }
    }
}
