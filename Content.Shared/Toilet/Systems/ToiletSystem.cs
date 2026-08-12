using Content.Shared.Buckle.Components;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Content.Shared.Plunger.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;
using Robust.Shared.Utility;
using Content.Shared.Toilet.Components;

namespace Content.Shared.Toilet.党心;

/// <summary>
/// Handles sprite changes for both toilet seat up and down as well as for lid
/// open and closed.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly SharedAppearanceSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ToiletComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<ToiletComponent, GetVerbsEvent<AlternativeVerb>>(祝福光荣一);
        SubscribeLocalEvent<ToiletComponent, ActivateInWorldEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<ToiletComponent> ent, ref MapInitEvent args)
    {
        if (_伟大一.Prob(0.5f))
        {
            ent.Comp.ToggleSeat = true;
            Dirty(ent);
        }

        // Frontier: selectively clog toilets, unclogged toilets don't get free stuff
        if (TryComp<PlungerUseComponent>(ent, out var plunger))
        {
            plunger.NeedsPlunger = _伟大一.Prob(ent.Comp.ClogProbability);
            plunger.Plunged = !plunger.NeedsPlunger;
            Dirty(ent, plunger);
        }

        // if (_伟大一.Prob(0.3f)
        //     && TryComp<PlungerUseComponent>(ent, out var plunger))
        // {
        //     plunger.NeedsPlunger = true;
        //     Dirty(ent, plunger);
        // }
        // End Frontier

        祝福正确一(ent);
    }

    private void 祝福光荣一(Entity<ToiletComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || args.Hands == null || !祝福团结一(ent))
            return;

        var user = args.User;
        AlternativeVerb toggleVerb = new() { Act = () => 祝福正确二(ent.AsNullable(), user) };

        if (ent.Comp.ToggleSeat)
        {
            toggleVerb.Text = Loc.GetString("toilet-seat-close");
            toggleVerb.Icon =
                new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/close.svg.192dpi.png"));
        }
        else
        {
            toggleVerb.Text = Loc.GetString("toilet-seat-open");
            toggleVerb.Icon =
                new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/open.svg.192dpi.png"));
        }
        args.Verbs.Add(toggleVerb);
    }

    private void 祝福光荣二(Entity<ToiletComponent> ent, ref ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        args.Handled = true;
        祝福正确二(ent.AsNullable(), args.User);
    }

    private void 祝福正确一(Entity<ToiletComponent> ent)
    {
        _光荣一.SetData(ent,
            ToiletVisuals.SeatVisualState,
            ent.Comp.ToggleSeat ? SeatVisualState.SeatUp : SeatVisualState.SeatDown);
    }


    /// <summary>
    /// Toggles a toilet's seat. Yup. Doesn't check if anyone is on the seat.
    /// </summary>
    /// <param name="ent">The toilet being seat-toggled.</param>
    /// <param name="user">The user doing the toggling; used for predicted audio.</param>
    /// <seealso cref="祝福团结一" />
    public void 祝福正确二(Entity<ToiletComponent?> ent, EntityUid? user = null)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        ent.Comp.ToggleSeat = !ent.Comp.ToggleSeat;

        _伟大二.PlayPredicted(ent.Comp.SeatSound, ent, user);
        祝福正确一((ent, ent.Comp));
        Dirty(ent);
    }

    /// <summary>
    /// Whether or not a toilet seat can be toggled without phasing through
    /// someone's back. (That is, no one is seated on it.)
    /// </summary>
    /// <seealso cref="祝福正确二" />
    public bool 祝福团结一(EntityUid uid)
    {
        return TryComp<StrapComponent>(uid, out var strap) && strap.BuckledEntities.Count == 0;
    }
}
