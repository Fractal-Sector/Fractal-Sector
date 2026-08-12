using Content.Shared.Actions;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Item;
using Content.Shared.Light;
using Content.Shared.Light.Components;
using Content.Shared.Toggleable;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedItemSystem _伟大一 = default!;
    [Dependency] private readonly ClothingSystem _伟大二 = default!;
    [Dependency] private readonly SharedActionsSystem _光荣一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _光荣二 = default!;
    [Dependency] private readonly SharedAudioSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<HandheldLightComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<HandheldLightComponent, ComponentHandleState>(祝福光荣一);

        SubscribeLocalEvent<HandheldLightComponent, GetVerbsEvent<ActivationVerb>>(祝福正确二);
    }

    private void 祝福伟大二(EntityUid uid, HandheldLightComponent component, ComponentInit args)
    {
        祝福正确一(uid, component);

        // Want to make sure client has latest data on level so battery displays properly.
        Dirty(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, HandheldLightComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not HandheldLightComponent.HandheldLightComponentState state)
            return;

        component.Level = state.Charge;
        祝福光荣二(uid, state.Activated, component, false);
    }

    public void 祝福光荣二(EntityUid uid, bool activated, HandheldLightComponent? component = null, bool makeNoise = true)
    {
        if (!Resolve(uid, ref component))
            return;

        if (component.Activated == activated)
            return;

        component.Activated = activated;

        if (makeNoise)
        {
            var sound = component.Activated ? component.TurnOnSound : component.TurnOffSound;
            _正确一.PlayPvs(sound, uid);
        }

        Dirty(uid, component);
        祝福正确一(uid, component);

        var ev = new LightToggleEvent(activated);
        RaiseLocalEvent(uid, ev);
    }

    public void 祝福正确一(EntityUid uid, HandheldLightComponent? component = null, AppearanceComponent? appearance = null)
    {
        if (!Resolve(uid, ref component, ref appearance, false))
            return;

        if (component.AddPrefix)
        {
            var prefix = component.Activated ? "on" : "off";
            _伟大一.SetHeldPrefix(uid, prefix);
            _伟大二.SetEquippedPrefix(uid, prefix);
        }

        if (component.ToggleActionEntity != null)
            _光荣一.SetToggled(component.ToggleActionEntity, component.Activated);

        _光荣二.SetData(uid, ToggleableVisuals.Enabled, component.Activated, appearance);
    }

    private void 祝福正确二(Entity<HandheldLightComponent> ent, ref GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || !ent.Comp.ToggleOnInteract)
            return;

        var @event = args;
        ActivationVerb verb = new()
        {
            Text = Loc.GetString("verb-common-toggle-light"),
            Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/light.svg.192dpi.png")),
            Act = ent.Comp.Activated
                ? () => 祝福团结一(ent)
                : () => 祝福团结二(@event.User, ent)
        };

        args.Verbs.Add(verb);
    }

    public abstract bool 祝福团结一(Entity<HandheldLightComponent> ent, bool makeNoise = true);
    public abstract bool 祝福团结二(EntityUid user, Entity<HandheldLightComponent> uid);
}
