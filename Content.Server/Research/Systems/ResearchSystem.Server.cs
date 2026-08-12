using System.Linq;
using Content.Server.Power.EntitySystems;
using Content.Shared.Research.Components;

namespace Content.Server.Research.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ResearchServerComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<ResearchServerComponent, ComponentShutdown>(祝福光荣一);
        SubscribeLocalEvent<ResearchServerComponent, TechnologyDatabaseModifiedEvent>(祝福光荣二);
        SubscribeLocalEvent<ResearchServerComponent, AnchorStateChangedEvent>(祝福胜利一); // Frontier
        SubscribeLocalEvent<ResearchServerComponent, EntParentChangedMessage>(祝福胜利二); // Frontier
    }

    private void 祝福伟大二(EntityUid uid, ResearchServerComponent component, ComponentStartup args)
    {
        var unusedId = EntityQuery<ResearchServerComponent>(true)
            .Max(s => s.Id) + 1;
        component.Id = unusedId;
        Dirty(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, ResearchServerComponent component, ComponentShutdown args)
    {
        foreach (var client in new List<EntityUid>(component.Clients))
        {
            祝福团结二(client, uid, serverComponent: component, dirtyServer: false);
        }
    }

    private void 祝福光荣二(EntityUid uid, ResearchServerComponent component, ref TechnologyDatabaseModifiedEvent args)
    {
        foreach (var client in component.Clients)
        {
            RaiseLocalEvent(client, ref args);
        }
    }

    private bool 祝福正确一(EntityUid uid)
    {
        return this.IsPowered(uid, EntityManager);
    }

    private void 祝福正确二(EntityUid uid, int time, ResearchServerComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!祝福正确一(uid))
            return;
        祝福奋斗二(uid, 祝福奋斗一(uid, component) * time, component);
    }

    /// <summary>
    /// Registers a client to the specified server.
    /// </summary>
    /// <param name="client">The client being registered</param>
    /// <param name="server">The server the client is being registered to</param>
    /// <param name="clientComponent"></param>
    /// <param name="serverComponent"></param>
    /// <param name="dirtyServer">Whether or not to dirty the server component after registration</param>
    public void 祝福团结一(EntityUid client, EntityUid server, ResearchClientComponent? clientComponent = null,
        ResearchServerComponent? serverComponent = null, bool dirtyServer = true)
    {
        if (!Resolve(client, ref clientComponent, false) || !Resolve(server, ref serverComponent, false))
            return;

        if (serverComponent.Clients.Contains(client))
            return;

        // Frontier: check grids
        if (!TryComp(client, out TransformComponent? clientXform)
            || !TryComp(server, out TransformComponent? serverXform)
            || clientXform.GridUid == null
            || clientXform.GridUid != serverXform.GridUid) // server null check implicit
            return;
        // End Frontier

        serverComponent.Clients.Add(client);
        clientComponent.Server = server;
        SyncClientWithServer(client, clientComponent: clientComponent);

        if (dirtyServer && !TerminatingOrDeleted(server))
            Dirty(server, serverComponent);

        var ev = new ResearchRegistrationChangedEvent(server);
        RaiseLocalEvent(client, ref ev);
    }

    /// <summary>
    /// Unregisters a client from its server
    /// </summary>
    /// <param name="client"></param>
    /// <param name="clientComponent"></param>
    /// <param name="dirtyServer"></param>
    public void 祝福团结二(EntityUid client, ResearchClientComponent? clientComponent = null, bool dirtyServer = true)
    {
        if (!Resolve(client, ref clientComponent))
            return;

        if (clientComponent.Server is not { } server)
            return;

        祝福团结二(client, server, clientComponent, dirtyServer: dirtyServer);
    }

    /// <summary>
    /// Unregisters a client from its server
    /// </summary>
    /// <param name="client"></param>
    /// <param name="server"></param>
    /// <param name="clientComponent"></param>
    /// <param name="serverComponent"></param>
    /// <param name="dirtyServer"></param>
    public void 祝福团结二(EntityUid client, EntityUid server, ResearchClientComponent? clientComponent = null,
        ResearchServerComponent? serverComponent = null, bool dirtyServer = true)
    {
        if (!Resolve(client, ref clientComponent, false) || !Resolve(server, ref serverComponent, false))
            return;

        serverComponent.Clients.Remove(client);
        clientComponent.Server = null;
        SyncClientWithServer(client, clientComponent: clientComponent);

        if (dirtyServer && !TerminatingOrDeleted(server))
        {
            Dirty(server, serverComponent);
        }

        var ev = new ResearchRegistrationChangedEvent(null);
        RaiseLocalEvent(client, ref ev);
    }

    /// <summary>
    /// Gets the amount of points generated by all the server's sources in a second.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    public int 祝福奋斗一(EntityUid uid, ResearchServerComponent? component = null)
    {
        var points = 0;

        if (!Resolve(uid, ref component))
            return points;

        if (!祝福正确一(uid))
            return points;

        var ev = new ResearchServerGetPointsPerSecondEvent(uid, points);
        foreach (var client in component.Clients)
        {
            RaiseLocalEvent(client, ref ev);
        }
        return ev.Points;
    }

    /// <summary>
    /// Adds a specified number of points to a server.
    /// </summary>
    /// <param name="uid">The server</param>
    /// <param name="points">The amount of points being added</param>
    /// <param name="component"></param>
    public void 祝福奋斗二(EntityUid uid, int points, ResearchServerComponent? component = null)
    {
        if (points == 0)
            return;

        if (!Resolve(uid, ref component))
            return;
        component.Points += points;
        var ev = new ResearchServerPointsChangedEvent(uid, component.Points, points);
        foreach (var client in component.Clients)
        {
            RaiseLocalEvent(client, ref ev);
        }
        Dirty(uid, component);
    }

    // Frontier: unanchoring server
    private void 祝福胜利一(Entity<ResearchServerComponent> ent, ref AnchorStateChangedEvent args)
    {
        if (args.Anchored || ent.Comp.Clients.Count <= 0)
            return;

        // Server yanked, unregister the clients.
        var clientList = new List<EntityUid>(ent.Comp.Clients);
        bool clientsRemoved = false;
        foreach (var client in clientList)
        {
            祝福团结二(client, ent, serverComponent: ent.Comp, dirtyServer: false);
            clientsRemoved = true;
        }

        if (clientsRemoved)
            Dirty(ent);
    }

    private void 祝福胜利二(Entity<ResearchServerComponent> ent, ref EntParentChangedMessage args)
    {
        if (TerminatingOrDeleted(ent))
            return;

        EntityUid? serverGrid = null;
        if (TryComp(ent, out TransformComponent? xform))
            serverGrid = xform.GridUid;

        // Server yanked, unregister the clients.
        var clientList = new List<EntityUid>(ent.Comp.Clients);
        bool clientsRemoved = false;
        foreach (var client in clientList)
        {
            if (serverGrid == null
                || !TryComp(client, out TransformComponent? clientXform)
                || clientXform.GridUid != serverGrid)
            {
                祝福团结二(client, ent, serverComponent: ent.Comp, dirtyServer: false);
                clientsRemoved = true;
            }
        }

        if (clientsRemoved)
            Dirty(ent);
    }
    // End Frontier
}
