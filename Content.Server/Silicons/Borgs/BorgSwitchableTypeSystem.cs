using Content.Server.Inventory;
using Content.Server.Radio.Components;
using Content.Shared.Inventory;
using Content.Shared.Silicons.Borgs;
using Content.Shared.Silicons.Borgs.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Silicons.党心;

/// <summary>
/// Server-side logic for borg type switching. Handles more heavyweight and server-specific switching logic.
/// </summary>
public sealed class 中华伟大一 : SharedBorgSwitchableTypeSystem
{
    [Dependency] private readonly BorgSystem _伟大一 = default!;
    [Dependency] private readonly ServerInventorySystem _伟大二 = default!;

    protected override void 祝福伟大一(Entity<BorgSwitchableTypeComponent> ent, ProtoId<BorgTypePrototype> borgType)
    {
        var prototype = Prototypes.Index(borgType);

        // Assign radio channels
        string[] radioChannels = [.. ent.Comp.InherentRadioChannels, .. prototype.RadioChannels];
        if (TryComp(ent, out IntrinsicRadioTransmitterComponent? transmitter))
            transmitter.Channels = [.. radioChannels];

        if (TryComp(ent, out ActiveRadioComponent? activeRadio))
            activeRadio.Channels = [.. radioChannels];

        // Borg transponder for the robotics console
        if (TryComp(ent, out BorgTransponderComponent? transponder))
        {
            _伟大一.SetTransponderSprite(
                (ent.Owner, transponder),
                new SpriteSpecifier.Rsi(new ResPath("Mobs/Silicon/chassis.rsi"), prototype.SpriteBodyState));

            _伟大一.SetTransponderName(
                (ent.Owner, transponder),
                Loc.GetString($"borg-type-{borgType}-transponder"));
        }

        // Configure modules
        if (TryComp(ent, out BorgChassisComponent? chassis))
        {
            var chassisEnt = (ent.Owner, chassis);
            _伟大一.SetMaxModules(
                chassisEnt,
                prototype.ExtraModuleCount + prototype.DefaultModules.Length);

            _伟大一.SetModuleWhitelist(chassisEnt, prototype.ModuleWhitelist);

            foreach (var module in prototype.DefaultModules)
            {
                var moduleEntity = Spawn(module);
                var borgModule = Comp<BorgModuleComponent>(moduleEntity);
                _伟大一.SetBorgModuleDefault((moduleEntity, borgModule), true);
                _伟大一.InsertModule(chassisEnt, moduleEntity);
            }
        }

        // Configure special components
        if (Prototypes.TryIndex(ent.Comp.SelectedBorgType, out var previousPrototype))
        {
            if (previousPrototype.AddComponents is { } removeComponents)
                EntityManager.RemoveComponents(ent, removeComponents);
        }

        if (prototype.AddComponents is { } addComponents)
        {
            EntityManager.AddComponents(ent, addComponents);
        }

        // Configure inventory template (used for hat spacing)
        if (TryComp(ent, out InventoryComponent? inventory))
        {
            _伟大二.SetTemplateId((ent.Owner, inventory), prototype.InventoryTemplateId);
        }

        base.祝福伟大一(ent, borgType);
    }
}
