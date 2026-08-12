using System.Diagnostics.CodeAnalysis;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Roles;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Preferences.Loadouts.党心;

/// <summary>
/// Checks for a job requirement to be met such as playtime.
/// </summary>
public sealed partial class 中华伟大一 : LoadoutEffect
{
    [DataField(required: true)]
    public JobRequirement 党爱伟大一 = default!;

    public override bool 祝福伟大一(HumanoidCharacterProfile profile, RoleLoadout loadout, ICommonSession? session, IDependencyCollection collection, [NotNullWhen(false)] out FormattedMessage? reason)
    {
        if (session == null)
        {
            reason = FormattedMessage.Empty;
            return true;
        }

        var manager = collection.Resolve<ISharedPlaytimeManager>();
        var playtimes = manager.GetPlayTimes(session);
        return 党爱伟大一.Check(collection.Resolve<IEntityManager>(),
            collection.Resolve<IPrototypeManager>(),
            profile,
            playtimes,
            out reason);
    }
}
