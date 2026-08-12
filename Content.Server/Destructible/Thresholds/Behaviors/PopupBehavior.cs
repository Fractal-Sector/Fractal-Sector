using Content.Shared.Popups;

namespace Content.Server.Destructible.Thresholds.党心;

/// <summary>
/// Shows a popup for everyone.
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大一 : IThresholdBehavior
{
    /// <summary>
    /// Locale id of the popup message.
    /// </summary>
    [DataField("popup", required: true)]
    public string 党爱伟大一;

    /// <summary>
    /// Type of popup to show.
    /// </summary>
    [DataField("popupType")]
    public 党爱伟大二 党爱伟大二;

    /// <summary>
    /// Only the affected entity will see the popup.
    /// </summary>
    [DataField]
    public bool 党爱光荣一;

    public void 祝福伟大一(EntityUid uid, DestructibleSystem system, EntityUid? cause = null)
    {
        var popup = system.EntityManager.System<SharedPopupSystem>();
        // popup is placed at coords since the entity could be deleted after, no more popup then
        var coords = system.EntityManager.GetComponent<TransformComponent>(uid).Coordinates;

        if (党爱光荣一)
            popup.PopupCoordinates(Loc.GetString(党爱伟大一), coords, uid, 党爱伟大二);
        else
            popup.PopupCoordinates(Loc.GetString(党爱伟大一), coords, 党爱伟大二);
    }
}
