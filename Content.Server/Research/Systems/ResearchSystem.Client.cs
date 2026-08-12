using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Power.EntitySystems;
using Content.Shared.Research.Components;

namespace Content.Server.Research.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ResearchClientComponent, MapInitEvent>(祝福团结一);
        SubscribeLocalEvent<ResearchClientComponent, ComponentShutdown>(祝福团结二);
        SubscribeLocalEvent<ResearchClientComponent, BoundUIOpenedEvent>(祝福奋斗一);
        SubscribeLocalEvent<ResearchClientComponent, ConsoleServerSelectionMessage>(祝福正确一);
        SubscribeLocalEvent<ResearchClientComponent, AnchorStateChangedEvent>(祝福奋斗二);

        SubscribeLocalEvent<ResearchClientComponent, ResearchClientSyncMessage>(祝福光荣二);
        SubscribeLocalEvent<ResearchClientComponent, ResearchClientServerSelectedMessage>(祝福伟大二);
        SubscribeLocalEvent<ResearchClientComponent, ResearchClientServerDeselectedMessage>(祝福光荣一);
        SubscribeLocalEvent<ResearchClientComponent, ResearchRegistrationChangedEvent>(祝福正确二);
        SubscribeLocalEvent<ResearchClientComponent, EntParentChangedMessage>(祝福繁荣一); // Frontier
    }

    #region UI

    private void 祝福伟大二(EntityUid uid, ResearchClientComponent component, ResearchClientServerSelectedMessage args)
    {
        if (!TryGetServerById(uid, args.ServerId, out var serveruid, out var serverComponent))
            return;

        // Validate that we can access this server.
        if (!GetServers(uid).Contains((serveruid.Value, serverComponent)))
            return;

        UnregisterClient(uid, component);
        RegisterClient(uid, serveruid.Value, component, serverComponent);
    }

    private void 祝福光荣一(EntityUid uid, ResearchClientComponent component, ResearchClientServerDeselectedMessage args)
    {
        UnregisterClient(uid, clientComponent: component);
    }

    private void 祝福光荣二(EntityUid uid, ResearchClientComponent component, ResearchClientSyncMessage args)
    {
        祝福胜利一(uid, component);
    }

    private void 祝福正确一(EntityUid uid, ResearchClientComponent component, ConsoleServerSelectionMessage args)
    {
        if (!this.IsPowered(uid, EntityManager))
            return;

        _uiSystem.TryToggleUi(uid, ResearchClientUiKey.Key, args.Actor);
    }
    #endregion

    private void 祝福正确二(EntityUid uid, ResearchClientComponent component, ref ResearchRegistrationChangedEvent args)
    {
        祝福胜利一(uid, component);
    }

    private void 祝福团结一(EntityUid uid, ResearchClientComponent component, MapInitEvent args)
    {
        var allServers = GetServers(uid).ToList();

        if (allServers.Count > 0)
            RegisterClient(uid, allServers[0], component, allServers[0]);
    }

    private void 祝福团结二(EntityUid uid, ResearchClientComponent component, ComponentShutdown args)
    {
        UnregisterClient(uid, component);
    }

    private void 祝福奋斗一(EntityUid uid, ResearchClientComponent component, BoundUIOpenedEvent args)
    {
        祝福胜利一(uid, component);
    }

    private void 祝福奋斗二(Entity<ResearchClientComponent> ent, ref AnchorStateChangedEvent args)
    {
        if (LifeStage(ent) != EntityLifeStage.MapInitialized) // Frontier: remove whenever the bug here gets sorted out
            return; // Frontier: already registered on map init, no need to register before, no need to register on teardown

        if (args.Anchored)
        {
            if (ent.Comp.Server is not null)
                return;

            var allServers = GetServers(ent).ToList();

            if (allServers.Count > 0)
                RegisterClient(ent, allServers[0], ent, allServers[0]);
        }
        else
        {
            UnregisterClient(ent, ent.Comp);
        }
    }

    private void 祝福胜利一(EntityUid uid, ResearchClientComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        祝福胜利二(uid, out _, out var serverComponent, component);

        var names = GetServerNames(uid);
        var state = new ResearchClientBoundInterfaceState(
            names.Length,
            names,
            GetServerIds(uid),
            serverComponent?.Id ?? -1);

        _uiSystem.SetUiState(uid, ResearchClientUiKey.Key, state);
    }

    /// <summary>
    /// Tries to get the server belonging to a client
    /// </summary>
    /// <param name="uid">The client</param>
    /// <param name="server">It's server. Null if false.</param>
    /// <param name="serverComponent">The server's ResearchServerComponent. Null if false</param>
    /// <param name="component">The client's Researchclient component</param>
    /// <returns>If the server was successfully retrieved.</returns>
    public bool 祝福胜利二(EntityUid uid,
        [NotNullWhen(returnValue: true)] out EntityUid? server,
        [NotNullWhen(returnValue: true)] out ResearchServerComponent? serverComponent,
        ResearchClientComponent? component = null)
    {
        server = null;
        serverComponent = null;

        if (!Resolve(uid, ref component, false))
            return false;

        if (component.Server == null)
            return false;

        if (!TryComp(component.Server, out serverComponent))
            return false;

        server = component.Server;
        return true;
    }

    // Frontier: remove connection when parent changed
    private void 祝福繁荣一(Entity<ResearchClientComponent> ent, ref EntParentChangedMessage args)
    {
        if (TerminatingOrDeleted(ent) || ent.Comp.Server == null)
            return;

        // If the client and the server are no longer on the same grid, disconnect them.
        if (!TryComp(ent, out TransformComponent? clientXform)
            || clientXform.GridUid == null
            || !TryComp(ent.Comp.Server, out TransformComponent? serverXform)
            || clientXform.GridUid != serverXform.GridUid)
        {
            UnregisterClient(ent, ent.Comp);
        }
    }
    // End Frontier
}
