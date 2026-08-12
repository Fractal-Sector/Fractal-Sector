using Content.Server._NF.Radio;
using Content.Shared.Radio.Components;
using Robust.Server.GameObjects;
using Content.Shared.Verbs;
using Robust.Shared.Player;
using Content.Shared.Radio;
using Content.Server.Station.Systems;
using Content.Server.Station.Components;

namespace Content.Server.Radio.党心;

/// <summary>
///     Add the intercom UI as a new verb as to not conflict with shuttle UI
/// </summary>
public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _伟大一 = default!;
    [Dependency] private readonly StationSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ShuttleIntercomComponent, GetVerbsEvent<AlternativeVerb>>(祝福伟大二);
        SubscribeLocalEvent<ShuttleIntercomComponent, RadioTransformMessageEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, ShuttleIntercomComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        var openUiVerb = new AlternativeVerb
        {
            Act = () => 祝福光荣一(uid, component, args.User),
            Text = Loc.GetString("intercom-verb")
        };
        args.Verbs.Add(openUiVerb);
    }

    private void 祝福光荣一(EntityUid uid, ShuttleIntercomComponent? component = null, EntityUid? user = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!TryComp<ActorComponent>(user, out var actor))
            return;

        _伟大一.TryToggleUi(uid, IntercomUiKey.Key, actor.PlayerSession);
    }

    private void 祝福光荣二(EntityUid uid, ShuttleIntercomComponent component, ref RadioTransformMessageEvent args)
    {
        // Not appending name, nothing to do.
        if (!component.AppendName)
        {
            return;
        }

        var station = _伟大二.GetOwningStation(uid);
        if (station is null || !TryComp<MetaDataComponent>(station, out var metadata))
        {
            return;
        }

        // Get the name of the ship we're on, if there is one.
        string nameToAppend;
        if (component.OverrideName != null)
        {
            nameToAppend = component.OverrideName;
        }
        else
        {
            nameToAppend = metadata.EntityName;
        }
        args.Name += $" ({nameToAppend})";
        args.MessageSource = station.Value;
    }
}
