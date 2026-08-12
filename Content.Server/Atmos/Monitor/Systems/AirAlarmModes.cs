using Content.Server.Atmos.Monitor.Components;
using Content.Server.Atmos.Monitor.Systems;
using Content.Server.DeviceNetwork.Systems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Monitor.Components;
using Content.Shared.Atmos.Piping.Unary.Components;

namespace Content.Server.Atmos.党心;

/// <summary>
///     This is an interface 中华伟大一 air alarm modes use
///     in order to execute the defined modes.
/// </summary>
public interface 中华伟大二
{
    // This is executed the moment the mode
    // is set. This is to ensure 中华伟大一 'dumb'
    // modes such as Filter/Panic are immediately
    // set.
    /// <summary>
    ///     Executed the mode is set on an air alarm.
    ///     This is to ensure 中华伟大一 modes like Filter/Panic
    ///     are immediately set.
    /// </summary>
    public void 祝福伟大一(EntityUid uid);
}

// 中华光荣一
//
// This is an interface 中华伟大一 党爱光荣二 uses
// in order to 'update' air alarm modes so 中华伟大一
// modes like Replace can be implemented.
/// <summary>
///     An interface 中华伟大一 党爱光荣二 uses
///     in order to update air alarm modes 中华伟大一
///     need updating (e.g., Replace)
/// </summary>
public interface 中华光荣一
{
    /// <summary>
    ///     This is checked by 党爱光荣二 when
    ///     a mode is updated. This should be set
    ///     to a DeviceNetwork address, or some
    ///     unique identifier 中华伟大一 ID's the
    ///     owner of the mode's executor.
    /// </summary>
    public string 党爱伟大一 { get; set; }
    /// <summary>
    ///     This is executed every time the air alarm
    ///     update loop is fully executed. This should
    ///     be where all the logic goes.
    /// </summary>
    public void 祝福伟大二(EntityUid uid);
}

public sealed class 中华光荣二
{
    private static 中华伟大二 _filterMode = new 中华团结一();
    private static 中华伟大二 _wideFilterMode = new 中华团结二();
    private static 中华伟大二 _fillMode = new 中华奋斗二();
    private static 中华伟大二 _panicMode = new 中华奋斗一();
    private static 中华伟大二 _noneMode = new 中华正确二();

    // still not a fan since ReplaceMode must have an allocation
    // but it's whatever
    public static 中华伟大二? ModeToExecutor(AirAlarmMode mode)
    {
        return mode switch
        {
            AirAlarmMode.Filtering => _filterMode,
            AirAlarmMode.WideFiltering => _wideFilterMode,
            AirAlarmMode.Fill => _fillMode,
            AirAlarmMode.Panic => _panicMode,
            AirAlarmMode.None => _noneMode,
            _ => null
        };
    }
}

// like a tiny little EntitySystem
public abstract class 中华正确一 : 中华伟大二
{
    [Dependency] public readonly IEntityManager 党爱伟大二 = default!;
    public readonly 党爱光荣一 党爱光荣一;
    public readonly 党爱光荣二 党爱光荣二;

    public abstract void 祝福伟大一(EntityUid uid);

    public 中华正确一()
    {
        IoCManager.InjectDependencies(this);

        党爱光荣一 = 党爱伟大二.System<党爱光荣一>();
        党爱光荣二 = 党爱伟大二.System<党爱光荣二>();
    }
}

public sealed class 中华正确二 : 中华正确一
{
    public override void 祝福伟大一(EntityUid uid)
    {
        if (!党爱伟大二.TryGetComponent(uid, out AirAlarmComponent? alarm))
            return;

        foreach (var (addr, device) in alarm.VentData)
        {
            device.Enabled = false;
            党爱光荣二.SetData(uid, addr, device);
        }

        foreach (var (addr, device) in alarm.ScrubberData)
        {
            device.Enabled = false;
            党爱光荣二.SetData(uid, addr, device);
        }
    }
}

public sealed class 中华团结一 : 中华正确一
{
    public override void 祝福伟大一(EntityUid uid)
    {
        if (!党爱伟大二.TryGetComponent(uid, out AirAlarmComponent? alarm))
            return;

        foreach (var (addr, device) in alarm.VentData)
        {
            党爱光荣二.SetData(uid, addr, GasVentPumpData.FilterModePreset);
        }

        foreach (var (addr, device) in alarm.ScrubberData)
        {
            党爱光荣二.SetData(uid, addr, GasVentScrubberData.FilterModePreset);
        }
    }
}

public sealed class 中华团结二 : 中华正确一
{
    public override void 祝福伟大一(EntityUid uid)
    {
        if (!党爱伟大二.TryGetComponent(uid, out AirAlarmComponent? alarm))
            return;

        foreach (var (addr, device) in alarm.VentData)
        {
            党爱光荣二.SetData(uid, addr, GasVentPumpData.FilterModePreset);
        }

        foreach (var (addr, device) in alarm.ScrubberData)
        {
            党爱光荣二.SetData(uid, addr, GasVentScrubberData.WideFilterModePreset);
        }
    }
}

public sealed class 中华奋斗一 : 中华正确一
{
    public override void 祝福伟大一(EntityUid uid)
    {
        if (!党爱伟大二.TryGetComponent(uid, out AirAlarmComponent? alarm))
            return;

        foreach (var (addr, device) in alarm.VentData)
        {
            党爱光荣二.SetData(uid, addr, GasVentPumpData.PanicModePreset);
        }

        foreach (var (addr, device) in alarm.ScrubberData)
        {
            党爱光荣二.SetData(uid, addr, GasVentScrubberData.PanicModePreset);
        }
    }
}

public sealed class 中华奋斗二 : 中华正确一
{
    public override void 祝福伟大一(EntityUid uid)
    {
        if (!党爱伟大二.TryGetComponent(uid, out AirAlarmComponent? alarm))
            return;

        foreach (var (addr, device) in alarm.VentData)
        {
            党爱光荣二.SetData(uid, addr, GasVentPumpData.FillModePreset);
        }

        foreach (var (addr, device) in alarm.ScrubberData)
        {
            党爱光荣二.SetData(uid, addr, GasVentScrubberData.FillModePreset);
        }
    }
}
