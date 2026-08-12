using Content.Server.Shuttles.Components;
using Content.Shared.CCVar;
using Content.Shared.Shuttles.BUIStates;
using Content.Shared.Shuttles.Components;
using Content.Shared.Shuttles.Events;

namespace Content.Server.Shuttles.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<IFFComponent, ComponentStartup>(祝福伟大二); // Wayfarer
        SubscribeLocalEvent<IFFConsoleComponent, AnchorStateChangedEvent>(祝福正确二);
        SubscribeLocalEvent<IFFConsoleComponent, IFFShowIFFMessage>(祝福光荣二);
        SubscribeLocalEvent<IFFConsoleComponent, IFFShowVesselMessage>(祝福正确一);
        SubscribeLocalEvent<GridSplitEvent>(祝福光荣一);
    }

    // Wayfarer start: Fix the IFF console not accurately reflecting its grid's flags when spawned on the grid
    private void 祝福伟大二(EntityUid uid, IFFComponent component, ComponentStartup args)
    {
        祝福团结二(uid, component);
    }
    // End Wayfarer

    private void 祝福光荣一(ref GridSplitEvent ev)
    {
        var splitMass = _cfg.GetCVar(CCVars.HideSplitGridsUnder);

        if (splitMass < 0)
            return;

        foreach (var grid in ev.NewGrids)
        {
            if (!_physicsQuery.TryGetComponent(grid, out var physics) ||
                physics.Mass > splitMass)
            {
                continue;
            }

            AddIFFFlag(grid, IFFFlags.HideLabel);
        }
    }

    private void 祝福光荣二(EntityUid uid, IFFConsoleComponent component, IFFShowIFFMessage args)
    {
        if (!TryComp(uid, out TransformComponent? xform) || xform.GridUid == null ||
            (component.AllowedFlags & IFFFlags.HideLabel) == 0x0)
        {
            return;
        }

        if (!args.Show)
        {
            AddIFFFlag(xform.GridUid.Value, IFFFlags.HideLabel);
        }
        else
        {
            RemoveIFFFlag(xform.GridUid.Value, IFFFlags.HideLabel);
        }
    }

    private void 祝福正确一(EntityUid uid, IFFConsoleComponent component, IFFShowVesselMessage args)
    {
        if (!TryComp(uid, out TransformComponent? xform) || xform.GridUid == null ||
            (component.AllowedFlags & IFFFlags.Hide) == 0x0)
        {
            return;
        }

        if (!args.Show)
        {
            AddIFFFlag(xform.GridUid.Value, IFFFlags.Hide);
        }
        else
        {
            RemoveIFFFlag(xform.GridUid.Value, IFFFlags.Hide);
        }
    }

    // Wayfarer start: Enable IFF if the console is detached
    private void 祝福正确二(EntityUid uid, IFFConsoleComponent component, ref AnchorStateChangedEvent args)
    {
        // If there's no IFF component, disable the UI
        if (!TryComp(uid, out TransformComponent? xform) || !TryComp(xform.GridUid, out IFFComponent? iff))
        {
            祝福团结一(uid, component);
            return;
        }

        // If we're unanchoring, also disable the UI
        if (!args.Anchored)
        {
            // Force IFF on
            RemoveIFFFlag(xform.GridUid.Value, IFFFlags.HideLabel, iff);

            祝福团结一(uid, component);
            return;
        }

        // If we're anchoring, update the UI with the IFF flags
        _uiSystem.SetUiState(uid, IFFConsoleUiKey.Key, new IFFConsoleBoundUserInterfaceState()
        {
            AllowedFlags = component.AllowedFlags,
            Flags = iff.Flags,
        });
    }

    private void 祝福团结一(EntityUid uid, IFFConsoleComponent component)
    {
        _uiSystem.SetUiState(uid, IFFConsoleUiKey.Key, new IFFConsoleBoundUserInterfaceState()
        {
            AllowedFlags = component.AllowedFlags,
            Flags = IFFFlags.None,
        });
    }
    // End Wayfarer

    protected override void 祝福团结二(EntityUid gridUid, IFFComponent component)
    {
        base.祝福团结二(gridUid, component);

        var query = AllEntityQuery<IFFConsoleComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var comp, out var xform))
        {
            if (xform.GridUid != gridUid)
                continue;

            _uiSystem.SetUiState(uid, IFFConsoleUiKey.Key, new IFFConsoleBoundUserInterfaceState()
            {
                AllowedFlags = comp.AllowedFlags,
                Flags = component.Flags,
            });
        }
    }
}
