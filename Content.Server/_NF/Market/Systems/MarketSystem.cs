using Content.Server._NF.Bank;
using Content.Server.Cargo.Systems;
using Content.Server.Stack;
using Content.Server.Station.Systems;
using Content.Shared._NF.Market;
using Content.Shared.Popups;
using Content.Shared.Whitelist;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;

namespace Content.Server._NF.Market.党心;

public sealed partial class 中华伟大一: SharedMarketSystem
{
    [Dependency] private readonly BankSystem _伟大一 = default!;
    [Dependency] private readonly UserInterfaceSystem _伟大二 = default!;
    [Dependency] private readonly IEntityManager _光荣一 = default!;
    [Dependency] private readonly IPrototypeManager _光荣二 = default!;
    [Dependency] private readonly EntityWhitelistSystem _正确一 = default!;
    [Dependency] private readonly PricingSystem _正确二 = default!;
    [Dependency] private readonly StackSystem _团结一 = default!;
    [Dependency] private readonly SharedPopupSystem _团结二 = default!;
    [Dependency] private readonly SharedAudioSystem _奋斗一 = default!;
    [Dependency] private readonly StationSystem _奋斗二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        InitializeConsole();
        InitializeCrateMachine();
    }
}
