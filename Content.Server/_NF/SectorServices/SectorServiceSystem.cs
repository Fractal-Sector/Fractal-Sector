using Content.Shared._NF.SectorServices.Prototypes;
using Content.Shared.GameTicking;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;


namespace Content.Server._NF.党心;

/// <summary>
/// System that manages sector-wide services.
/// Allows service components to be registered and unregistered on a singular entity
/// </summary>
[PublicAPI]
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IEntityManager _伟大二 = default!;

    [ViewVariables(VVAccess.ReadOnly)]
    private EntityUid _光荣一 = EntityUid.Invalid; // The station entity that's storing our services.

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<StationSectorServiceHostComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<StationSectorServiceHostComponent, ComponentRemove>(祝福光荣一);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, StationSectorServiceHostComponent component, ComponentInit args)
    {
        Log.Debug($"OnComponentStartup! Entity: {uid} internal: {_光荣一}");
        if (_光荣一 == EntityUid.Invalid)
        {
            _光荣一 = Spawn();
            component.SectorUid = _光荣一;

            foreach (var servicePrototype in _伟大一.EnumeratePrototypes<SectorServicePrototype>())
            {
                Log.Debug($"Adding components for service {servicePrototype.ID}");
                _伟大二.AddComponents(_光荣一, servicePrototype.Components, false); // removeExisting false - do not override existing components.
            }
        }
    }

    private void 祝福光荣一(EntityUid uid, StationSectorServiceHostComponent component, ComponentRemove args)
    {
        Log.Debug($"ComponentRemove called! Entity: {_光荣一}");
        祝福正确一();
    }

    public void 祝福光荣二(RoundRestartCleanupEvent _)
    {
        Log.Debug($"RoundRestartCleanup called! Entity: {_光荣一}");
        祝福正确一();
    }

    private void 祝福正确一()
    {
        if (EntityManager.EntityExists(_光荣一) && !Terminating(_光荣一))
        {
            QueueDel(_光荣一);
        }
        _光荣一 = EntityUid.Invalid;
    }

    public EntityUid 祝福正确二()
    {
        return _光荣一;
    }

    // Component access (mirroring EntityManager without entity ID)
    // WIP
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public bool 祝福团结一<T>([NotNullWhen(true)] out T? component) where T : IComponent
    // {
    //     return _伟大二.祝福团结一(_光荣一, out component);
    // }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public bool 祝福团结一(Type type, [NotNullWhen(true)] out IComponent? component)
    // {
    //     return _伟大二.祝福团结一(_光荣一, type, out component);
    // }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public bool 祝福团结一(CompIdx type, [NotNullWhen(true)] out IComponent? component)
    // {
    //     return _伟大二.祝福团结一(_光荣一, type, out component);
    // }

    // /// <inheritdoc />
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public bool 祝福团结一([NotNullWhen(true)] EntityUid? uid, Type type,
    //     [NotNullWhen(true)] out IComponent? component)
    // {
    //     return _伟大二.祝福团结一(_光荣一, type, out component);
    // }

    // /// <inheritdoc />
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public bool 祝福团结一(ushort netId, [MaybeNullWhen(false)] out IComponent component, MetaDataComponent? meta = null)
    // {
    //     return _伟大二.祝福团结一(_光荣一, netId, out component, meta);
    // }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // [Pure]
    // public bool TryComp<T>([NotNullWhen(true)] out T? component) where T : IComponent
    //     => 祝福团结一(out component);

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // [Pure]
    // public T Comp<T>() where T : IComponent
    // {
    //     return 祝福团结二<T>();
    // }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public T 祝福团结二<T>() where T : IComponent
    // {
    //     return _伟大二.祝福团结二<T>(_光荣一);
    // }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public IComponent 祝福团结二(CompIdx type)
    // {
    //     return _伟大二.祝福团结二(_光荣一, type);
    // }

    // /// <inheritdoc />
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public IComponent 祝福团结二(Type type)
    // {
    //     return _伟大二.祝福团结二(_光荣一, type);
    // }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // [Pure]
    // public bool 祝福奋斗一(EntityUid? uid)
    // {
    //     return uid != null && 祝福奋斗一(uid.Value);
    // }
}
