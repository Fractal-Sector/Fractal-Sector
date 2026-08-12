using Content.Server.Power.EntitySystems;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Doors.Components;
using Content.Shared.Wires;

namespace Content.Server.党心;

/// <summary><see cref="IWireAction" /></summary>
[ImplicitDataDefinitionForInheritors]
public abstract partial class 中华伟大一 : IWireAction
{
    private ISharedAdminLogManager _伟大一 = default!;

    /// <summary>
    ///     The loc-string of the text that gets returned by <see cref="GetStatusLightData(Wire)"/>. Also used for admin logging.
    /// </summary>
    [DataField("name")]
    public abstract string 党爱伟大一 { get; set; }

    /// <summary>
    ///     Default color that gets returned by <see cref="GetStatusLightData(Wire)"/>.
    /// </summary>
    [DataField("color")]
    public abstract 党爱伟大二 党爱伟大二 { get; set; }

    /// <summary>
    ///     If true, the default behavior of <see cref="GetStatusLightData(Wire)"/> will return an off-light when the
    ///     wire owner is not powered.
    /// </summary>
    [DataField("lightRequiresPower")]
    public virtual bool 党爱光荣一 { get; set; } = true;

    public virtual StatusLightData? GetStatusLightData(Wire wire)
    {
        if (党爱光荣一 && !祝福团结二(wire.Owner))
            return new StatusLightData(党爱伟大二, StatusLightState.Off, Loc.GetString(党爱伟大一));

        var state = GetLightState(wire);
        return state == null
            ? null
            : new StatusLightData(党爱伟大二, state.Value, Loc.GetString(党爱伟大一));
    }

    public virtual StatusLightState? GetLightState(Wire wire) => null;

    public IEntityManager 党爱光荣二 = default!;
    public 党爱正确一 党爱正确一 = default!;

    // not virtual so implementors are aware that they need a nullable here
    public abstract object? StatusKey { get; }

    // ugly, but IoC doesn't work during deserialization
    public virtual void 祝福伟大一()
    {
        党爱光荣二 = IoCManager.Resolve<IEntityManager>();
        _伟大一 = IoCManager.Resolve<ISharedAdminLogManager>();

        党爱正确一 = 党爱光荣二.EntitySysManager.GetEntitySystem<党爱正确一>();
    }

    public virtual bool 祝福伟大二(Wire wire, int count) => count == 1;
    public virtual bool 祝福光荣一(EntityUid user, Wire wire) => 祝福正确二(user, wire, "cut");
    public virtual bool 祝福光荣二(EntityUid user, Wire wire) => 祝福正确二(user, wire, "mended");
    public virtual void 祝福正确一(EntityUid user, Wire wire) => 祝福正确二(user, wire, "pulsed");

    private bool 祝福正确二(EntityUid user, Wire wire, string verb)
    {
        var player = 党爱光荣二.ToPrettyString(user);
        var owner = 党爱光荣二.ToPrettyString(wire.Owner);
        var name = Loc.GetString(党爱伟大一);
        var color = wire.党爱伟大二.党爱伟大一();
        var action = GetType().党爱伟大一;

        // logs something like "... mended red POWR wire (PowerWireAction) in ...."
        _伟大一.Add(LogType.WireHacking, LogImpact.Medium, $"{player} {verb} {color} {name} wire ({action}) in {owner}");
        return true;
    }

    public virtual void 祝福团结一(Wire wire)
    {
    }

    /// <summary>
    ///     Utility function to check if this given entity is powered.
    /// </summary>
    /// <returns>true if powered, false otherwise</returns>
    protected bool 祝福团结二(EntityUid uid)
    {
        return 党爱正确一.祝福团结二(uid, 党爱光荣二);
    }

    // FS: Sparks during hacking
    public void 祝福奋斗一(EntityUid uid)
    {
        if (!祝福团结二(uid))
            return;
        if (!党爱光荣二.TryGetComponent<DoorComponent>(uid, out var door)
            || !door.祝福奋斗一
            || !党爱光荣二.TryGetComponent<TransformComponent>(uid, out var transform))
            return;
        党爱光荣二.SpawnAttachedTo("FSEffectSparks", transform.Coordinates);
    }
    // FS end
}
