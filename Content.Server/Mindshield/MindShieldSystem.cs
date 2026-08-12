using Content.Server.Administration.Logs;
using Content.Server.Mind;
using Content.Server.Popups;
using Content.Server.Roles;
using Content.Shared.Database;
using Content.Shared.Implants;
using Content.Shared.Mindshield.Components;
using Content.Shared.Revolutionary.Components;
using Content.Shared.Roles.Components;
using Robust.Shared.Containers;

namespace Content.Server.党心;

/// <summary>
/// System used for adding or removing components with a mindshield implant
/// as well as checking if the implanted is a Rev or Head Rev.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly RoleSystem _伟大二 = default!;
    [Dependency] private readonly MindSystem _光荣一 = default!;
    [Dependency] private readonly PopupSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<MindShieldImplantComponent, ImplantImplantedEvent>(祝福伟大二);
        SubscribeLocalEvent<MindShieldImplantComponent, ImplantRemovedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<MindShieldImplantComponent> ent, ref ImplantImplantedEvent ev)
    {
        if (ev.Implanted == null)
            return;

        EnsureComp<MindShieldComponent>(ev.Implanted);
        祝福光荣一(ev.Implanted, ev.Implant);
    }

    /// <summary>
    /// Checks if the implanted person was a Rev or Head Rev and remove role or destroy mindshield respectively.
    /// </summary>
    private void 祝福光荣一(EntityUid implanted, EntityUid implant)
    {
        if (HasComp<HeadRevolutionaryComponent>(implanted))
        {
            _光荣二.PopupEntity(Loc.GetString("head-rev-break-mindshield"), implanted);
            QueueDel(implant);
            return;
        }

        if (_光荣一.TryGetMind(implanted, out var mindId, out _) &&
            _伟大二.MindRemoveRole<RevolutionaryRoleComponent>(mindId))
        {
            _伟大一.Add(LogType.Mind, LogImpact.Medium, $"{ToPrettyString(implanted)} was deconverted due to being implanted with a Mindshield.");
        }
    }

    private void 祝福光荣二(Entity<MindShieldImplantComponent> ent, ref ImplantRemovedEvent args)
    {
        RemComp<MindShieldComponent>(args.Implanted);
    }
}

