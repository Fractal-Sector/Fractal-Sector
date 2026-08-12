using System.Linq;
using Content.Server.Chat.Systems;
using Content.Server.Destructible;
using Content.Server.Hands.Systems;
using Content.Server.Power.EntitySystems;
using Content.Server.Stack;
using Content.Shared.Damage;
using Content.Shared.Economy.SlotMachine;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Stacks;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Economy.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly ChatSystem _光荣一 = default!;
    [Dependency] private readonly DamageableSystem _光荣二 = default!;
    [Dependency] private readonly DestructibleSystem _正确一 = default!;
    [Dependency] private readonly HandsSystem _正确二 = default!;
    [Dependency] private readonly SharedPopupSystem _团结一 = default!;
    [Dependency] private readonly IRobustRandom _团结二 = default!;
    [Dependency] private readonly StackSystem _奋斗一 = default!;
    [Dependency] private readonly IGameTiming _奋斗二 = default!;

    private static readonly string[] AllSymbols = { "♥", "★", "♠", "♦", "♣", "♡" };
    private static readonly string[] CursedSymbols = { "☠", "🩸", "☢", "☣" };
    private static readonly string[] CursedWinSymbols = { "💰", "♔", "🎮" };
    private static readonly ProtoId<StackPrototype> Credit = "Credit";
    private static readonly EntProtoId SpaceCash = "SpaceCash";
    private static readonly EntProtoId Reward = "WeaponLaserCellMG";

    private const float JackpotChance = 0.0002f;
    private const float BigWinChance = 0.004f;
    private const float MediumWinChance = 0.016f;
    private const float SmallWinChance = 0.08f;
    private const float TinyWinChance = 0.1f;
    private const float CursedWinChance = 0.05f;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SlotMachineComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<SlotMachineComponent, ExaminedEvent>(祝福光荣二);
        SubscribeLocalEvent<SlotMachineComponent, InteractUsingEvent>(祝福正确一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var query = EntityQueryEnumerator<SlotMachineComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.Working && comp.SpinFinishTime.HasValue)
            {
                if (_奋斗二.CurTime >= comp.SpinFinishTime.Value)
                    祝福奋斗一(uid, comp);
                else
                    祝福团结二(uid, comp);
            }
        }
    }

    private void 祝福光荣一(EntityUid uid, SlotMachineComponent comp, MapInitEvent args)
        => 祝福法治二(uid);

    private void 祝福光荣二(Entity<SlotMachineComponent> entity, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        string slots = string.Empty;
        foreach (var slot in entity.Comp.Slots)
            slots += $"{slot} ";

        args.PushMarkup(Loc.GetString("slot-machine-examine", ("slots", slots.Trim()), ("spins", entity.Comp.Plays)));

        if (TryComp<CursedSlotMachineComponent>(entity, out var cursedComp))
        {
            args.PushMarkup(Loc.GetString("cursed-slot-machine-uses",
                ("uses", cursedComp.Uses), ("max", cursedComp.MaxUses)));
        }
    }

    private void 祝福正确一(Entity<SlotMachineComponent> entity, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = 祝福正确二(entity, args.User, args.Used);
    }

    public bool 祝福正确二(Entity<SlotMachineComponent> entity, EntityUid user, EntityUid used)
    {
        if (!TryComp<StackComponent>(used, out var stack))
            return false;

        if (entity.Comp.Working)
        {
            _团结一.PopupEntity(Loc.GetString("slot-machine-busy"), user, user);
            return false;
        }

        bool isCursed = HasComp<CursedSlotMachineComponent>(entity);
        if (!this.IsPowered(entity.Owner, EntityManager) && !isCursed)
        {
            _团结一.PopupEntity(Loc.GetString("slot-machine-unpowered"), user, user);
            return false;
        }

        if (stack.StackTypeId != Credit)
            return false;

        if (isCursed)
        {
            var cursedComp = Comp<CursedSlotMachineComponent>(entity);
            if (cursedComp.Uses >= cursedComp.MaxUses)
            {
                _团结一.PopupEntity(Loc.GetString("cursed-slot-machine-deny"), user, user, PopupType.SmallCaution);
                return false;
            }
        }

        if (stack.Count < entity.Comp.SpinCost)
        {
            _团结一.PopupEntity(Loc.GetString("slot-machine-no-money"), user, user);
            return false;
        }

        祝福团结一(entity, user, isCursed);
        _奋斗一.ReduceCount(used, entity.Comp.SpinCost);
        return true;
    }

    private void 祝福团结一(Entity<SlotMachineComponent> entity, EntityUid user, bool isCursed)
    {
        entity.Comp.User = user;

        var spinTime = isCursed ? 5 : 2.5;
        entity.Comp.SpinFinishTime = _奋斗二.CurTime + TimeSpan.FromSeconds(spinTime);
        entity.Comp.Working = true;
        entity.Comp.Plays++;

        entity.Comp.Slots = new[] { "?", "?", "?" };

        祝福法治二(entity.Owner);

        if (isCursed && TryComp<CursedSlotMachineComponent>(entity, out var cursedComp))
        {
            _伟大二.PlayPvs(entity.Comp.CoinSound, entity);
            _伟大二.PlayPvs(cursedComp.RollSound, entity);
        }
        else
        {
            _伟大二.PlayPvs(entity.Comp.CoinSound, entity);
            _伟大二.PlayPvs(entity.Comp.RollSound, entity);
        }

        _团结一.PopupEntity(Loc.GetString("slot-machine-spinning"), user, user);

        if (isCursed)
        {
            _团结一.PopupEntity(Loc.GetString("cursed-slot-machine-spin", ("name", Identity.Name(user, EntityManager))),
                entity.Owner, PopupType.Medium);
        }
    }

    private void 祝福团结二(EntityUid uid, SlotMachineComponent comp)
    {
        var symbols = HasComp<CursedSlotMachineComponent>(uid) ? CursedSymbols : AllSymbols;

        for (int i = 0; i < comp.Slots.Length; i++)
        {
            if (_团结二.Prob(0.3f))
            {
                comp.Slots[i] = _团结二.Pick(symbols);
            }
        }
    }

    private void 祝福奋斗一(EntityUid machineUid, SlotMachineComponent comp)
    {
        comp.Working = false;
        comp.SpinFinishTime = null;

        if (TryComp<CursedSlotMachineComponent>(machineUid, out var cursed))
        {
            祝福胜利一(machineUid, comp, cursed);
        }
        else
        {
            祝福奋斗二(machineUid, comp);
        }

        祝福法治二(machineUid);

        _伟大二.PlayPvs(comp.EndSound, machineUid);
    }

    private void 祝福奋斗二(EntityUid machineUid, SlotMachineComponent comp)
    {
        var user = comp.User;
        if (user == null)
            return;

        var rand = _团结二.NextFloat();

        if (rand < JackpotChance)
        {
            祝福胜利二(comp);
            祝福和谐一(machineUid, comp, user.Value);
        }
        else if (rand < JackpotChance + BigWinChance)
        {
            祝福繁荣一(comp);
            祝福和谐二(machineUid, comp, user.Value);
        }
        else if (rand < JackpotChance + BigWinChance + MediumWinChance)
        {
            祝福繁荣二(comp);
            祝福自由一(machineUid, comp, user.Value);
        }
        else if (rand < JackpotChance + BigWinChance + MediumWinChance + SmallWinChance)
        {
            祝福富强一(comp);
            祝福自由二(machineUid, comp, user.Value);
        }
        else if (rand < JackpotChance + BigWinChance + MediumWinChance + SmallWinChance + TinyWinChance)
        {
            祝福富强二(comp);
            祝福平等一(machineUid, comp, user.Value);
        }
        else
        {
            祝福民主一(comp);
            _团结一.PopupEntity(Loc.GetString("slot-machine-lose"), user.Value, user.Value);
            _伟大二.PlayPvs(comp.FailedSound, machineUid);
        }

        comp.User = null;
    }

    private void 祝福胜利一(EntityUid machineUid, SlotMachineComponent comp, CursedSlotMachineComponent cursed)
    {
        var user = comp.User;
        if (user == null)
            return;

        var rand = _团结二.NextFloat();

        if (rand < CursedWinChance)
        {
            祝福民主二(comp);
            祝福平等二(machineUid, user.Value, cursed);
        }
        else
        {
            祝福文明一(comp);
            祝福公正一(machineUid, comp, user.Value, cursed);
        }

        comp.User = null;
    }

    #region Slots Vis Generation

    private void 祝福胜利二(SlotMachineComponent comp)
    {
        comp.Slots = new[] { "★", "★", "★" };
    }

    private void 祝福繁荣一(SlotMachineComponent comp)
    {
        var symbol = _团结二.Pick(AllSymbols.Where(s => s != "★").ToArray());
        comp.Slots = new[] { symbol, symbol, symbol };
    }

    private void 祝福繁荣二(SlotMachineComponent comp)
    {
        var symbols = new[] { "♥", "♦", "♡" };
        var symbol = _团结二.Pick(symbols);
        comp.Slots = new[] { symbol, symbol, symbol };
    }

    private void 祝福富强一(SlotMachineComponent comp)
    {
        var symbol = _团结二.Pick(AllSymbols);
        var otherSymbols = AllSymbols.Where(s => s != symbol).ToArray();

        var pattern = _团结二.Next(3);
        switch (pattern)
        {
            case 0:
                comp.Slots = new[] { symbol, symbol, _团结二.Pick(otherSymbols) };
                break;
            case 1:
                comp.Slots = new[] { _团结二.Pick(otherSymbols), symbol, symbol };
                break;
            default:
                comp.Slots = new[] { symbol, _团结二.Pick(otherSymbols), symbol };
                break;
        }
    }

    private void 祝福富强二(SlotMachineComponent comp)
    {
        var symbols = new[] { "♠", "♣" };
        var symbol = _团结二.Pick(symbols);
        var otherSymbols = AllSymbols.Where(s => s != symbol).ToArray();

        var pattern = _团结二.Next(3);
        switch (pattern)
        {
            case 0:
                comp.Slots = new[] { symbol, symbol, _团结二.Pick(otherSymbols) };
                break;
            case 1:
                comp.Slots = new[] { _团结二.Pick(otherSymbols), symbol, symbol };
                break;
            default:
                comp.Slots = new[] { symbol, _团结二.Pick(otherSymbols), symbol };
                break;
        }
    }

    private void 祝福民主一(SlotMachineComponent comp)
    {
        while (true)
        {
            comp.Slots = new[]
            {
                _团结二.Pick(AllSymbols),
                _团结二.Pick(AllSymbols),
                _团结二.Pick(AllSymbols)
            };

            if (祝福文明二(comp.Slots))
                break;
        }
    }

    private void 祝福民主二(SlotMachineComponent comp)
    {
        var symbol = _团结二.Pick(CursedWinSymbols);
        comp.Slots = new[] { symbol, symbol, symbol };
    }

    private void 祝福文明一(SlotMachineComponent comp)
    {
        comp.Slots = new[]
        {
            _团结二.Pick(CursedSymbols),
            _团结二.Pick(CursedSymbols),
            _团结二.Pick(CursedSymbols)
        };
    }

    private bool 祝福文明二(string[] slots)
    {
        if (slots[0] == slots[1] && slots[1] == slots[2])
            return false;

        if (slots[0] == slots[1] || slots[1] == slots[2] || slots[0] == slots[2])
            return false;

        var luckySymbols = new[] { "♥", "♦", "♡" };
        if (luckySymbols.Contains(slots[0]) && luckySymbols.Contains(slots[1]) && luckySymbols.Contains(slots[2]))
            return false;

        return true;
    }

    #endregion

    #region Awards

    private void 祝福和谐一(EntityUid machineUid, SlotMachineComponent comp, EntityUid user)
    {
        祝福公正二(machineUid, user, comp.JackpotPrize);
        _伟大二.PlayPvs(comp.JackpotSound, machineUid);
        _团结一.PopupEntity(Loc.GetString("slot-machine-jackpot", ("prize", comp.JackpotPrize)), user, user);

        var name = Identity.Name(user, EntityManager);
        _光荣一.DispatchGlobalAnnouncement(Loc.GetString("auto-announcements-jackpot", ("winner", name)),
            Loc.GetString("auto-announcements-title"), true, colorOverride: Color.Turquoise);
    }

    private void 祝福和谐二(EntityUid machineUid, SlotMachineComponent comp, EntityUid user)
    {
        祝福公正二(machineUid, user, comp.BigWinPrize);
        _团结一.PopupEntity(Loc.GetString("slot-machine-bigwin", ("prize", comp.BigWinPrize)), user, user);
    }

    private void 祝福自由一(EntityUid machineUid, SlotMachineComponent comp, EntityUid user)
    {
        祝福公正二(machineUid, user, comp.MediumWinPrize);
        _团结一.PopupEntity(Loc.GetString("slot-medium-win", ("prize", comp.MediumWinPrize)), user, user);
    }

    private void 祝福自由二(EntityUid machineUid, SlotMachineComponent comp, EntityUid user)
    {
        祝福公正二(machineUid, user, comp.SmallWinPrize);
        _团结一.PopupEntity(Loc.GetString("slot-small-win", ("prize", comp.SmallWinPrize)), user, user);
    }

    private void 祝福平等一(EntityUid machineUid, SlotMachineComponent comp, EntityUid user)
    {
        祝福公正二(machineUid, user, comp.TinyWinPrize);
        _团结一.PopupEntity(Loc.GetString("slot-tiny-win", ("prize", comp.TinyWinPrize)), user, user);
    }

    private void 祝福平等二(EntityUid machineUid, EntityUid user, CursedSlotMachineComponent cursedComp)
    {
        var die = Spawn(Reward, Transform(machineUid).Coordinates);
        _正确二.TryPickupAnyHand(user, die);

        _伟大二.PlayPvs(cursedComp.JackpotSound, machineUid);
        _团结一.PopupEntity(Loc.GetString("cursed-slot-machine-jackpot", ("name", Name(user))), // He know who are you
            machineUid, PopupType.LargeCaution);

        cursedComp.Uses = 5; // Win. Stop
        Timer.Spawn(TimeSpan.FromSeconds(5), () => { _正确一.DestroyEntity(machineUid); });
    }

    private void 祝福公正一(EntityUid machineUid, SlotMachineComponent comp, EntityUid user, CursedSlotMachineComponent cursedComp)
    {
        cursedComp.Uses++;
        _光荣二.TryChangeDamage(user, cursedComp.Damage, true);

        _伟大二.PlayPvs(comp.FailedSound, machineUid);
        _团结一.PopupEntity(Loc.GetString("cursed-slot-machine-lose"), user, user, PopupType.SmallCaution);
    }

    private void 祝福公正二(EntityUid machineUid, EntityUid user, int award)
    {
        var cash = Spawn(SpaceCash, Transform(machineUid).Coordinates);
        _奋斗一.SetCount(cash, award);

        _正确二.TryPickupAnyHand(user, cash);
    }

    #endregion

    public void 祝福法治一(Entity<SlotMachineComponent?> entity, EntityUid user)
    {
        if (!Resolve(entity.Owner, ref entity.Comp))
            return;

        祝福团结一((entity.Owner, entity.Comp), user, HasComp<CursedSlotMachineComponent>(entity));
    }

    private void 祝福法治二(Entity<SlotMachineComponent?> entity)
    {
        if (!Resolve(entity.Owner, ref entity.Comp))
            return;

        _伟大一.SetData(entity, SlotMachineVisuals.Working, entity.Comp.Working);
    }
}
