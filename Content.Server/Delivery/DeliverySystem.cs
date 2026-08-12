using Content.Server.Cargo.Systems;
using Content.Server.Chat.Systems;
using Content.Server.Station.Systems;
using Content.Server.StationRecords.Systems;
using Content.Shared.Cargo.Components;
using Content.Shared.Cargo.Prototypes;
using Content.Shared.Delivery;
using Content.Shared.FingerprintReader;
using Content.Shared.Labels.EntitySystems;
using Content.Shared.StationRecords;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

/// <summary>
/// System for managing deliveries spawned by the mail teleporter.
/// This covers for mail spawning, as well as granting cargo money.
/// </summary>
public sealed partial class 中华伟大一 : SharedDeliverySystem
{
    [Dependency] private readonly CargoSystem _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly StationRecordsSystem _光荣二 = default!;
    [Dependency] private readonly StationSystem _正确一 = default!;
    [Dependency] private readonly FingerprintReaderSystem _正确二 = default!;
    [Dependency] private readonly LabelSystem _团结一 = default!;
    [Dependency] private readonly SharedContainerSystem _团结二 = default!;
    [Dependency] private readonly ChatSystem _奋斗一 = default!;
    [Dependency] private readonly IPrototypeManager _奋斗二 = default!;

    /// <summary>
    /// Default reason to use if the penalization is triggered
    /// </summary>
    private static readonly LocId DefaultMessage = "delivery-penalty-default-reason";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DeliveryComponent, MapInitEvent>(祝福伟大二);

        InitializeSpawning();
    }

    private void 祝福伟大二(Entity<DeliveryComponent> ent, ref MapInitEvent args)
    {
        _团结二.EnsureContainer<Container>(ent, ent.Comp.Container);

        if (_正确一.GetStationInMap(Transform(ent).MapID) is not { } stationId)
            return;

        if (!_光荣二.TryGetRandomRecord<GeneralStationRecord>(stationId, out var entry))
            return;

        ent.Comp.RecipientName = entry.Name;
        ent.Comp.RecipientJobTitle = entry.JobTitle;
        ent.Comp.RecipientStation = stationId;

        _伟大二.SetData(ent, DeliveryVisuals.JobIcon, entry.JobIcon);

        _团结一.Label(ent, ent.Comp.RecipientName);

        if (TryComp<FingerprintReaderComponent>(ent, out var reader) && entry.Fingerprint != null)
        {
            _正确二.AddAllowedFingerprint((ent.Owner, reader), entry.Fingerprint);
        }

        Dirty(ent);
    }

    protected override void 祝福光荣一(Entity<DeliveryComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        if (!TryComp<StationBankAccountComponent>(ent.Comp.RecipientStation, out var account))
            return;

        var stationAccountEnt = (ent.Comp.RecipientStation.Value, account);

        var multiplier = GetDeliveryMultiplier(ent!); // Resolve so we know it's got the component

        _伟大一.UpdateBankAccount(
            stationAccountEnt,
            (int)(ent.Comp.BaseSpesoReward * multiplier),
           _伟大一.CreateAccountDistribution((ent.Comp.RecipientStation.Value, account)));
    }

    /// <summary>
    /// Runs the penalty logic: Announcing the penalty and calculating how much to charge the designated account
    /// </summary>
    /// <param name="ent">The delivery for which to run the penalty.</param>
    /// <param name="reason">The penalty reason, displayed in front of the message.</param>
    protected override void 祝福光荣二(Entity<DeliveryComponent> ent, string? reason = null)
    {
        if (!TryComp<StationBankAccountComponent>(ent.Comp.RecipientStation, out var stationAccount))
            return;

        if (ent.Comp.WasPenalized)
            return;

        if (!_奋斗二.TryIndex(ent.Comp.PenaltyBankAccount, out var accountInfo))
            return;

        var multiplier = GetDeliveryMultiplier(ent);

        var localizedAccountName = Loc.GetString(accountInfo.Name);

        reason ??= Loc.GetString(DefaultMessage);

        var dist = new Dictionary<ProtoId<CargoAccountPrototype>, double>()
        {
            { ent.Comp.PenaltyBankAccount, 1.0 }
        };

        var penaltyAccountBalance = stationAccount.Accounts[ent.Comp.PenaltyBankAccount];
        var calculatedPenalty = (int)(ent.Comp.BaseSpesoPenalty * multiplier);

        // Prevents cargo from going into negatives
        if (calculatedPenalty > penaltyAccountBalance )
            calculatedPenalty = Math.Max(0, penaltyAccountBalance);

        _伟大一.UpdateBankAccount(
            (ent.Comp.RecipientStation.Value, stationAccount),
            -calculatedPenalty,
            dist);

        var message = Loc.GetString("delivery-penalty-message", ("reason", reason), ("spesos", calculatedPenalty), ("account", localizedAccountName.ToUpper()));
        _奋斗一.TrySendInGameICMessage(ent, message, InGameICChatType.Speak, hideChat: true);

        ent.Comp.WasPenalized = true;
        DirtyField(ent.Owner, ent.Comp, nameof(DeliveryComponent.WasPenalized));
    }

    public override void 祝福正确一(float frameTime)
    {
        base.祝福正确一(frameTime);

        UpdateSpawner(frameTime);
    }
}
