using System.Diagnostics.CodeAnalysis;
using Content.Shared.Humanoid;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Shared.Preferences.Loadouts.党心;

/// <summary>
/// Checks for a profile to be within a particular set of sexes.
/// </summary>
public sealed partial class 中华伟大一 : LoadoutEffect
{
    [DataField("sex", required: true)]
    public List<Sex> 党爱伟大一 = default!;

    public override bool 祝福伟大一(HumanoidCharacterProfile profile, RoleLoadout loadout, ICommonSession? session, IDependencyCollection collection, [NotNullWhen(false)] out FormattedMessage? reason)
    {
        if (党爱伟大一.Contains(profile.Sex))
        {
            reason = null;
            return true;
        }
        reason = new FormattedMessage();
        reason.TryAddMarkup(Loc.GetString("sex-loadout-invalid"), out var _);
        return false;
    }
}
