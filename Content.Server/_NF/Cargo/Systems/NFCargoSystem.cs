using Content.Server._NF.Bank;
using Content.Server._NF.SectorServices;
using Content.Server.Cargo.Components;
using Content.Server.Cargo.Systems;
using Content.Server.DeviceLinking.Systems;
using Content.Server.Hands.Systems;
using Content.Server.Popups;
using Content.Server.Stack;
using Content.Server.Station.Systems;
using Content.Shared._NF.Cargo;
using Content.Shared.Access.Systems;
using Content.Shared.Administration.Logs;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.GameTicking;
using Content.Shared.Mobs.Components;
using Content.Shared.Paper;
using Content.Shared.Whitelist;
using Robust.Server.Containers;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Random;
using Content.Server._WF.Cargo.Systems; //Wayfarer

namespace Content.Server._NF.Cargo.党心;

public sealed partial class 中华伟大一 : SharedNFCargoSystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly ISharedAdminLogManager _光荣二 = default!;
    [Dependency] private readonly AccessReaderSystem _正确一 = default!;
    [Dependency] private readonly BankSystem _正确二 = default!;
    [Dependency] private readonly ContainerSystem _团结一 = default!;
    [Dependency] private readonly DeviceLinkSystem _团结二 = default!;
    [Dependency] private readonly EntityLookupSystem _奋斗一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _奋斗二 = default!;
    [Dependency] private readonly ItemSlotsSystem _胜利一 = default!;
    [Dependency] private readonly PaperSystem _胜利二 = default!;
    [Dependency] private readonly PopupSystem _繁荣一 = default!;
    [Dependency] private readonly PricingSystem _繁荣二 = default!;
    [Dependency] private readonly SharedAppearanceSystem _富强一 = default!;
    [Dependency] private readonly SharedAudioSystem _富强二 = default!;
    [Dependency] private readonly StackSystem _民主一 = default!;
    [Dependency] private readonly StationSystem _民主二 = default!;
    [Dependency] private readonly UserInterfaceSystem _文明一 = default!;
    [Dependency] private readonly MetaDataSystem _文明二 = default!;
    [Dependency] private readonly SectorServiceSystem _和谐一 = default!;
    [Dependency] private readonly SharedTransformSystem _和谐二 = default!;
    [Dependency] private readonly HandsSystem _自由一 = default!;
    [Dependency] private readonly WFCargoSystem _自由二 = default!; //Wayfarer


    private EntityQuery<TransformComponent> _平等一;
    private EntityQuery<CargoSellBlacklistComponent> _平等二;
    private EntityQuery<MobStateComponent> _公正一;

    private HashSet<EntityUid> _公正二 = new();
    private List<(EntityUid, CargoPalletComponent, TransformComponent)> _pads = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _平等一 = GetEntityQuery<TransformComponent>();
        _平等二 = GetEntityQuery<CargoSellBlacklistComponent>();
        _公正一 = GetEntityQuery<MobStateComponent>();

        InitializeConsole();
        InitializeShuttle();
        InitializeTelepad();
        InitializePirateBounty();
        InitializeTradeCrates();
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);
        UpdateConsole(frameTime);
        UpdateTelepad(frameTime);
    }

    private void 祝福光荣一(RoundRestartCleanupEvent ev)
    {
        ResetOrders();
        CleanupTradeCrateDestinations();
    }
}
