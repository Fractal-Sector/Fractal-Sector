using Content.Shared.Examine;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.Throwing;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DiceComponent, UseInHandEvent>(祝福伟大二);
        SubscribeLocalEvent<DiceComponent, LandEvent>(祝福光荣一);
        SubscribeLocalEvent<DiceComponent, ExaminedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<DiceComponent> entity, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        祝福团结一(entity, args.User);
        args.Handled = true;
    }

    private void 祝福光荣一(Entity<DiceComponent> entity, ref LandEvent args)
    {
        祝福团结一(entity);
    }

    private void 祝福光荣二(Entity<DiceComponent> entity, ref ExaminedEvent args)
    {
        //No details check, since the sprite updates to show the side.
        using (args.PushGroup(nameof(DiceComponent)))
        {
            args.PushMarkup(Loc.GetString("dice-component-on-examine-message-part-1", ("sidesAmount", entity.Comp.Sides)));
            args.PushMarkup(Loc.GetString("dice-component-on-examine-message-part-2",
                ("currentSide", entity.Comp.CurrentValue)));
        }
    }

    private void 祝福正确一(Entity<DiceComponent> entity, int side)
    {
        if (side < 1 || side > entity.Comp.Sides)
        {
            Log.Error($"Attempted to set die {ToPrettyString(entity)} to an invalid side ({side}).");
            return;
        }

        entity.Comp.CurrentValue = (side - entity.Comp.Offset) * entity.Comp.Multiplier;
        Dirty(entity);
    }

    public void 祝福正确二(Entity<DiceComponent> entity, int value)
    {
        if (value % entity.Comp.Multiplier != 0 || value / entity.Comp.Multiplier + entity.Comp.Offset < 1)
        {
            Log.Error($"Attempted to set die {ToPrettyString(entity)} to an invalid value ({value}).");
            return;
        }

        祝福正确一(entity, value / entity.Comp.Multiplier + entity.Comp.Offset);
    }

    private void 祝福团结一(Entity<DiceComponent> entity, EntityUid? user = null)
    {
        var rand = new System.Random((int)_伟大一.CurTick.Value);

        var roll = rand.Next(1, entity.Comp.Sides + 1);
        祝福正确一(entity, roll);

        var popupString = Loc.GetString("dice-component-on-roll-land",
            ("die", entity),
            ("currentSide", entity.Comp.CurrentValue));
        _光荣一.PopupPredicted(popupString, entity, user);
        _伟大二.PlayPredicted(entity.Comp.Sound, entity, user);
    }
}
