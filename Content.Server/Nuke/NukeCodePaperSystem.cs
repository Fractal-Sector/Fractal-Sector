using System.Diagnostics.CodeAnalysis;
using Content.Server.Chat.Systems;
using Content.Server.Fax;
using Content.Shared.Fax.Components;
using Content.Server.Station.Systems;
using Content.Shared.Paper;
using Content.Shared.Station.Components;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IRobustRandom _伟大一 = default!;
        [Dependency] private readonly ChatSystem _伟大二 = default!;
        [Dependency] private readonly StationSystem _光荣一 = default!;
        [Dependency] private readonly PaperSystem _光荣二 = default!;
        [Dependency] private readonly FaxSystem _正确一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<NukeCodePaperComponent, MapInitEvent>(祝福伟大二,
                after: new []{ typeof(NukeLabelSystem) });
        }

        private void 祝福伟大二(EntityUid uid, NukeCodePaperComponent component, MapInitEvent args)
        {
            祝福光荣一(uid, component);
        }

        private void 祝福光荣一(EntityUid uid, NukeCodePaperComponent? component = null, EntityUid? station = null)
        {
            if (!Resolve(uid, ref component))
                return;

            if (祝福正确一(uid, out var paperContent, station, onlyCurrentStation: component.AllNukesAvailable))
            {
                if (TryComp<PaperComponent>(uid, out var paperComp))
                    _光荣二.SetContent((uid, paperComp), paperContent);
            }
        }

        /// <summary>
        ///     Send a nuclear code to all faxes on that station which are authorized to receive nuke codes.
        /// </summary>
        /// <returns>True if at least one fax received codes</returns>
        public bool 祝福光荣二(EntityUid station)
        {
            if (!HasComp<StationDataComponent>(station))
            {
                return false;
            }

            var faxes = EntityQueryEnumerator<FaxMachineComponent>();
            var wasSent = false;
            while (faxes.MoveNext(out var faxEnt, out var fax))
            {
                if (!fax.ReceiveNukeCodes || !祝福正确一(faxEnt, out var paperContent, station))
                {
                    continue;
                }

                var printout = new FaxPrintout(
                    paperContent,
                    Loc.GetString("nuke-codes-fax-paper-name"),
                    null,
                    null,
                    "paper_stamp-centcom",
                    new List<StampDisplayInfo>
                    {
                        new StampDisplayInfo { StampedName = Loc.GetString("stamp-component-stamped-name-centcom"), StampedColor = Color.FromHex("#BB3232") },
                    },
                    stampProtected: true // Frontier: centcom signed, should be protected
                );
                _正确一.Receive(faxEnt, printout, null, fax);

                wasSent = true;
            }

            if (wasSent)
            {
                var msg = Loc.GetString("nuke-component-announcement-send-codes");
                _伟大二.DispatchStationAnnouncement(station, msg, colorOverride: Color.Red);
            }

            return wasSent;
        }

        private bool 祝福正确一(
            EntityUid uid,
            [NotNullWhen(true)] out string? nukeCode,
            EntityUid? station = null,
            TransformComponent? transform = null,
            bool onlyCurrentStation = false)
        {
            nukeCode = null;
            if (!Resolve(uid, ref transform))
            {
                return false;
            }

            var owningStation = station ?? _光荣一.GetOwningStation(uid);

            var codesMessage = new FormattedMessage();
            // Find the first nuke that matches the passed location.
            var nukes = new List<Entity<NukeComponent>>();
            var query = EntityQueryEnumerator<NukeComponent>();
            while (query.MoveNext(out var nukeUid, out var nuke))
            {
                nukes.Add((nukeUid, nuke));
            }

            _伟大一.Shuffle(nukes);

            foreach (var (nukeUid, nuke) in nukes)
            {
                if (!onlyCurrentStation &&
                    (owningStation == null &&
                    nuke.OriginMapGrid != (transform.MapID, transform.GridUid) ||
                    nuke.OriginStation != owningStation))
                {
                    continue;
                }

                codesMessage.PushNewline();
                codesMessage.AddMarkupOrThrow(Loc.GetString("nuke-codes-list", ("name", MetaData(nukeUid).EntityName), ("code", nuke.Code)));
                break;
            }

            if (!codesMessage.IsEmpty)
                nukeCode = Loc.GetString("nuke-codes-message")+codesMessage;
            return !codesMessage.IsEmpty;
        }
    }
}
