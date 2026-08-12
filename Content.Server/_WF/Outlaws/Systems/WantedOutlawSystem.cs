using Content.Server.CriminalRecords.Systems;
using Content.Server.Radio.EntitySystems;
using Content.Server.StationRecords.Systems;
using Content.Shared.Clothing;
using Content.Shared.Forensics.Components;
using Content.Shared.GameTicking;
using Content.Shared.Inventory;
using Content.Shared.Preferences.Loadouts;
using Content.Shared.Radio;
using Content.Shared.Security;

namespace Content.Server._WF.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component;

public sealed class 中华伟大二 : EntitySystem
{
    [Dependency] private readonly InventorySystem _伟大一 = default!;
    [Dependency] private readonly StationRecordsSystem _伟大二 = default!;
    [Dependency] private readonly CriminalRecordsSystem _光荣一 = default!;
    [Dependency] private readonly RadioSystem _光荣二 = default!;

    [ValidatePrototypeId<RadioChannelPrototype>]
    private const string CgpChannel = "Nfsd";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<中华伟大一, PlayerSpawnCompleteEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, 中华伟大一 component, PlayerSpawnCompleteEvent args)
    {
        if (args.JobId is not { } jobId)
            return;

        _伟大一.TryGetSlotEntity(uid, "id", out var idUid);
        TryComp<FingerprintComponent>(uid, out var fingerprint);
        TryComp<DnaComponent>(uid, out var dna);

        if (_伟大二.TryCreateSectorRecord(uid, idUid, args.Profile, jobId, fingerprint?.Fingerprint, dna?.DNA) is not { } key)
            return;

        var reason = args.Profile.Loadouts.TryGetValue(LoadoutSystem.GetJobPrototype(jobId), out var loadout)
                     && !string.IsNullOrWhiteSpace(loadout.CrimeReason)
            ? loadout.CrimeReason
            : Loc.GetString("wf-wanted-outlaw-crime-reason-unspecified");

        _光荣一.TryChangeStatus(
            key,
            SecurityStatus.Wanted,
            reason,
            Loc.GetString("wf-wanted-outlaw-initiator"));

        _光荣二.SendRadioMessage(key.OriginStation, Loc.GetString(
            "wf-wanted-outlaw-arrival-broadcast",
            ("name", args.Profile.Name),
            ("reason", reason)), CgpChannel, uid);
    }
}