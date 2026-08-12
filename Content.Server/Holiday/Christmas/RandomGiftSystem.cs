using Content.Server.Administration.Logs;
using Content.Server.Hands.Systems;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Whitelist;
using Robust.Server.Audio;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Holiday.党心;

/// <summary>
/// This handles granting players their gift.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AudioSystem _伟大一 = default!;
    [Dependency] private readonly HandsSystem _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IRobustRandom _光荣二 = default!;
    [Dependency] private readonly IAdminLogManager _正确一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _正确二 = default!;
    [Dependency] private readonly SharedTransformSystem _团结一 = default!;

    private readonly List<string> _团结二 = new();
    private readonly List<string> _奋斗一 = new();

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福正确一);
        SubscribeLocalEvent<RandomGiftComponent, MapInitEvent>(祝福光荣二);
        SubscribeLocalEvent<RandomGiftComponent, UseInHandEvent>(祝福光荣一);
        SubscribeLocalEvent<RandomGiftComponent, ExaminedEvent>(祝福伟大二);
        祝福正确二();
    }

    private void 祝福伟大二(EntityUid uid, RandomGiftComponent component, ExaminedEvent args)
    {
        if (_正确二.IsWhitelistFail(component.ContentsViewers, args.Examiner) || component.SelectedEntity is null)
            return;

        var name = _光荣一.Index<EntityPrototype>(component.SelectedEntity).Name;
        args.PushText(Loc.GetString("gift-packin-contains", ("name", name)));
    }

    private void 祝福光荣一(EntityUid uid, RandomGiftComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        if (component.SelectedEntity is null)
            return;

        var coords = Transform(args.User).Coordinates;
        var handsEnt = Spawn(component.SelectedEntity, coords);
        _正确一.Add(LogType.EntitySpawn, LogImpact.Low, $"{ToPrettyString(args.User)} used {ToPrettyString(uid)} which spawned {ToPrettyString(handsEnt)}");
        if (component.Wrapper is not null)
            Spawn(component.Wrapper, coords);

        _伟大一.PlayPvs(component.Sound, args.User);

        // Don't delete the entity in the event bus, so we queue it for deletion.
        // We need the free hand for the new item, so we send it to nullspace.
        _团结一.DetachEntity(uid, Transform(uid));
        QueueDel(uid);

        _伟大二.PickupOrDrop(args.User, handsEnt);

        args.Handled = true;
    }

    private void 祝福光荣二(EntityUid uid, RandomGiftComponent component, MapInitEvent args)
    {
        if (component.InsaneMode)
            component.SelectedEntity = _光荣二.Pick(_奋斗一);
        else
            component.SelectedEntity = _光荣二.Pick(_团结二);
    }

    private void 祝福正确一(PrototypesReloadedEventArgs obj)
    {
        if (obj.WasModified<EntityPrototype>())
            祝福正确二();
    }

    private void 祝福正确二()
    {
        _团结二.Clear();
        _奋斗一.Clear();
        var itemCompName = Factory.GetComponentName<ItemComponent>();
        var mapGridCompName = Factory.GetComponentName<MapGridComponent>();
        var physicsCompName = Factory.GetComponentName<PhysicsComponent>();

        foreach (var proto in _光荣一.EnumeratePrototypes<EntityPrototype>())
        {
            if (proto.Abstract || proto.HideSpawnMenu || proto.Components.ContainsKey(mapGridCompName) || !proto.Components.ContainsKey(physicsCompName))
                continue;

            _奋斗一.Add(proto.ID);

            if (!proto.Components.ContainsKey(itemCompName))
                continue;

            _团结二.Add(proto.ID);
        }
    }
}
