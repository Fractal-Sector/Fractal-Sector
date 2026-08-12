using System.Linq;
using Content.Shared.Clothing.Components;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Tools;
using Content.Shared.Tools.Systems;
using Content.Shared.Tools.Components;
using Content.Shared.DoAfter;
using Content.Shared.Verbs;
using Content.Shared.Hands.EntitySystems;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Serialization.Manager;

namespace Content.Shared.Clothing.党心;

/// <summary>
/// System for handling advanced collar module installation and removal.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _伟大一 = default!;
    [Dependency] private readonly SharedToolSystem _伟大二 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣一 = default!;
    [Dependency] private readonly IComponentFactory _光荣二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _正确一 = default!;
    [Dependency] private readonly SharedAudioSystem _正确二 = default!;
    [Dependency] private readonly SharedHandsSystem _团结一 = default!;
    [Dependency] private readonly ISerializationManager _团结二 = default!;

    public const string 党爱伟大一 = "collar_module_container";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AdvancedCollarComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<AdvancedCollarComponent, ExaminedEvent>(祝福正确一);
        SubscribeLocalEvent<AdvancedCollarComponent, InteractUsingEvent>(祝福团结一);
        SubscribeLocalEvent<AdvancedCollarComponent, GetVerbsEvent<InteractionVerb>>(祝福团结二);
        SubscribeLocalEvent<AdvancedCollarComponent, AdvancedCollarRemoveModulesDoAfterEvent>(祝福奋斗一);
        SubscribeLocalEvent<AdvancedCollarComponent, EntInsertedIntoContainerMessage>(祝福光荣一);
        SubscribeLocalEvent<AdvancedCollarComponent, EntRemovedFromContainerMessage>(祝福光荣二);

        SubscribeLocalEvent<AdvancedCollarModuleComponent, ExaminedEvent>(祝福正确二);
    }

    private void 祝福伟大二(Entity<AdvancedCollarComponent> collar, ref ComponentInit args)
    {
        collar.Comp.ModuleContainer = _光荣一.EnsureContainer<Container>(collar, 党爱伟大一);
    }

    private void 祝福光荣一(Entity<AdvancedCollarComponent> collar, ref EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != 党爱伟大一)
            return;

        // Apply the module's effect
        if (TryComp<AdvancedCollarModuleComponent>(args.Entity, out var module))
        {
            祝福胜利一(collar, (args.Entity, module));
        }
    }

    private void 祝福光荣二(Entity<AdvancedCollarComponent> collar, ref EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != 党爱伟大一)
            return;

        // Remove the module's effect
        if (TryComp<AdvancedCollarModuleComponent>(args.Entity, out var module))
        {
            祝福繁荣一(collar, (args.Entity, module));
        }
    }

    private void 祝福正确一(Entity<AdvancedCollarComponent> collar, ref ExaminedEvent args)
    {
        var moduleCount = collar.Comp.ModuleContainer.ContainedEntities.Count;
        
        if (moduleCount == 0)
        {
            args.PushMarkup(Loc.GetString("advanced-collar-examine-no-modules"));
        }
        else
        {
            args.PushMarkup(Loc.GetString("advanced-collar-examine-modules", 
                ("count", moduleCount),
                ("max", collar.Comp.MaxModules)));

            foreach (var moduleUid in collar.Comp.ModuleContainer.ContainedEntities)
            {
                if (TryComp<AdvancedCollarModuleComponent>(moduleUid, out var module) && 
                    !string.IsNullOrEmpty(module.ModuleDescription))
                {
                    args.PushMarkup(Loc.GetString("advanced-collar-examine-module-entry",
                        ("name", Name(moduleUid)),
                        ("description", module.ModuleDescription)));
                }
            }
        }
    }

    private void 祝福正确二(Entity<AdvancedCollarModuleComponent> module, ref ExaminedEvent args)
    {
        if (!string.IsNullOrEmpty(module.Comp.ModuleDescription))
        {
            args.PushMarkup(Loc.GetString("advanced-collar-module-examine",
                ("description", module.Comp.ModuleDescription)));
        }

        if (module.Comp.InstalledIn != null)
        {
            args.PushMarkup(Loc.GetString("advanced-collar-module-already-in-collar"));
        }
    }

    private void 祝福团结一(Entity<AdvancedCollarComponent> collar, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        // Try to install a module
        if (TryComp<AdvancedCollarModuleComponent>(args.Used, out var module))
        {
            args.Handled = true;
            祝福奋斗二(collar, (args.Used, module), args.User);
            return;
        }

        // Try to remove modules with screwdriver
        if (_伟大二.HasQuality(args.Used, "Screwing"))
        {
            if (collar.Comp.ModuleContainer.ContainedEntities.Count == 0)
                return;

            args.Handled = true;

            // Start do-after for removing modules
            var doAfterArgs = new DoAfterArgs(EntityManager, args.User, 3f, new AdvancedCollarRemoveModulesDoAfterEvent(), collar.Owner, target: collar.Owner, used: args.Used)
            {
                BreakOnMove = true,
                NeedHand = true
            };

            _正确一.TryStartDoAfter(doAfterArgs);
        }
    }

    private void 祝福团结二(Entity<AdvancedCollarComponent> collar, ref GetVerbsEvent<InteractionVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        if (collar.Comp.ModuleContainer.ContainedEntities.Count == 0)
            return;

        // Check if user has a screwdriver
        if (args.Using == null || !_伟大二.HasQuality(args.Using.Value, "Screwing"))
            return;

        // Capture values for lambda
        var user = args.User;
        var used = args.Using.Value;
        var collarUid = collar.Owner;

        InteractionVerb verb = new()
        {
            Act = () =>
            {
                // Start do-after for removing modules
                var doAfterArgs = new DoAfterArgs(EntityManager, user, 3f, new AdvancedCollarRemoveModulesDoAfterEvent(), collarUid, target: collarUid, used: used)
                {
                    BreakOnMove = true,
                    NeedHand = true
                };

                _正确一.TryStartDoAfter(doAfterArgs);
            },
            Text = Loc.GetString("advanced-collar-remove-modules-verb"),
            Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/eject.svg.192dpi.png")),
            Priority = 1
        };

        args.Verbs.Add(verb);
    }

    private void 祝福奋斗一(Entity<AdvancedCollarComponent> collar, ref AdvancedCollarRemoveModulesDoAfterEvent args)
    {
        if (args.Cancelled)
            return;

        // Check if collar is being worn - can't remove modules while equipped
        if (TryComp<ClothingComponent>(collar, out var clothing) && clothing.InSlot != null)
        {
            _伟大一.PopupClient(Loc.GetString("advanced-collar-worn"), collar, args.User);
            return;
        }

        祝福富强一(collar, args.User);
    }

    public void 祝福奋斗二(Entity<AdvancedCollarComponent> collar, Entity<AdvancedCollarModuleComponent> module, EntityUid user)
    {
        // Check if module is already installed somewhere
        if (module.Comp.InstalledIn != null)
        {
            _伟大一.PopupClient(Loc.GetString("advanced-collar-module-already-installed"), collar, user);
            return;
        }

        // Check if collar is full
        if (collar.Comp.ModuleContainer.ContainedEntities.Count >= collar.Comp.MaxModules)
        {
            _伟大一.PopupClient(Loc.GetString("advanced-collar-full"), collar, user);
            return;
        }

        // Install the module
        if (_光荣一.Insert(module.Owner, collar.Comp.ModuleContainer))
        {
            module.Comp.InstalledIn = collar;
            Dirty(module);
            
            _伟大一.PopupClient(Loc.GetString("advanced-collar-module-installed",
                ("module", Name(module))), collar, user);
            _正确二.PlayPredicted(collar.Comp.ModuleInsertionSound, collar, user);
        }
    }

    private void 祝福胜利一(EntityUid collar, Entity<AdvancedCollarModuleComponent> module)
    {
        // Handle single component (legacy)
        if (!string.IsNullOrEmpty(module.Comp.ComponentToAdd))
        {
            祝福胜利二(collar, module.Owner, module.Comp.ComponentToAdd);
        }

        // Handle multiple components
        foreach (var componentName in module.Comp.ComponentsToAdd)
        {
            if (!string.IsNullOrEmpty(componentName))
            {
                祝福胜利二(collar, module.Owner, componentName);
            }
        }
    }

    private void 祝福胜利二(EntityUid collar, EntityUid moduleEntity, string componentName)
    {
        // Try to get the component registration - it may not exist on the client for server-only components
        if (!_光荣二.TryGetRegistration(componentName, out var registration))
        {
            // Component doesn't exist on this side (likely server-only component on client)
            // This is fine - server will add it when it processes the module
            return;
        }

        var componentType = registration.Type;

        // Check if the collar already has this component
        if (HasComp(collar, componentType))
            return;

        // Check if the module entity has this component with configuration
        if (EntityManager.TryGetComponent(moduleEntity, componentType, out var moduleComponent))
        {
            // Clone the component from the module to preserve configuration
            var component = (Component)_光荣二.GetComponent(componentType);
            var temp = (object)component;
            _团结二.CopyTo(moduleComponent, ref temp);
            AddComp(collar, (Component)temp!);
        }
        else
        {
            // Add the component with default values
            var component = (Component)_光荣二.GetComponent(componentType);
            AddComp(collar, component);
        }
    }

    private void 祝福繁荣一(EntityUid collar, Entity<AdvancedCollarModuleComponent> module)
    {
        // Handle single component (legacy)
        if (!string.IsNullOrEmpty(module.Comp.ComponentToAdd))
        {
            祝福繁荣二(collar, module.Comp.ComponentToAdd);
        }

        // Handle multiple components
        foreach (var componentName in module.Comp.ComponentsToAdd)
        {
            if (!string.IsNullOrEmpty(componentName))
            {
                祝福繁荣二(collar, componentName);
            }
        }
    }

    private void 祝福繁荣二(EntityUid collar, string componentName)
    {
        // Try to get the component registration - it may not exist on the client for server-only components
        if (!_光荣二.TryGetRegistration(componentName, out var registration))
        {
            // Component doesn't exist on this side (likely server-only component on client)
            // This is fine - server will remove it when it processes the module removal
            return;
        }

        var componentType = registration.Type;

        // Remove the component from the collar
        RemComp(collar, componentType);
    }

    public void 祝福富强一(Entity<AdvancedCollarComponent> collar, EntityUid user)
    {
        var moduleCount = collar.Comp.ModuleContainer.ContainedEntities.Count;
        
        if (moduleCount == 0)
        {
            _伟大一.PopupClient(Loc.GetString("advanced-collar-no-modules"), collar, user);
            return;
        }

        // Store modules before emptying container
        var modulesToClear = collar.Comp.ModuleContainer.ContainedEntities.ToList();
        
        // Remove all modules from the container
        _光荣一.EmptyContainer(collar.Comp.ModuleContainer);
        
        // Eject each module to user's hands or drop nearby
        foreach (var moduleUid in modulesToClear)
        {
            if (TryComp<AdvancedCollarModuleComponent>(moduleUid, out var module))
            {
                module.InstalledIn = null;
            }
            
            _团结一.PickupOrDrop(user, moduleUid, dropNear: true);
        }

        _伟大一.PopupClient(Loc.GetString("advanced-collar-modules-removed",
            ("count", moduleCount)), collar, user);
        _正确二.PlayPredicted(collar.Comp.ModuleExtractionSound, collar, user);
    }
}
