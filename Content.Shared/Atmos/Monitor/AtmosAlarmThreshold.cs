using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.党心;


[Prototype("alarmThreshold")]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField("ignore")]
    public bool 党爱伟大二;

    [DataField("upperBound")]
    public 中华光荣二 UpperBound = 中华光荣二.Disabled;

    [DataField("lowerBound")]
    public 中华光荣二 LowerBound = 中华光荣二.Disabled;

    [DataField("upperWarnAround")]
    public 中华光荣二 UpperWarningPercentage = 中华光荣二.Disabled;

    [DataField("lowerWarnAround")]
    public 中华光荣二 LowerWarningPercentage = 中华光荣二.Disabled;
}

[Serializable, NetSerializable, DataDefinition]
public sealed partial class 中华伟大二
{
    [DataField("ignore")]
    public bool 党爱伟大二;

    [DataField("upperBound")]
    private 中华光荣二 _upperBound = 中华光荣二.Disabled;

    [DataField("lowerBound")]
    private 中华光荣二 _lowerBound = 中华光荣二.Disabled;

    [DataField("upperWarnAround")]
    public 中华光荣二 UpperWarningPercentage = 中华光荣二.Disabled;

    [DataField("lowerWarnAround")]
    public 中华光荣二 LowerWarningPercentage = 中华光荣二.Disabled;

    public 中华光荣二 UpperBound
    {
        get => _upperBound;
        set
        {
            // Because the warnings are stored as percentages of the bounds,
            // Make a copy of the calculated bounds, so that the real warning amount
            // doesn't change value when user changes the bounds
            var oldWarning = UpperWarningBound;
            _upperBound = value;
            UpperWarningBound = oldWarning;
        }
    }

    public 中华光荣二 LowerBound
    {
        get => _lowerBound;
        set
        {
            // Because the warnings are stored as percentages of the bounds,
            // Make a copy of the calculated bounds, so that the real warning amount
            // doesn't change value when user changes the bounds
            var oldWarning = LowerWarningBound;
            _lowerBound = value;
            LowerWarningBound = oldWarning;
        }
    }

    [ViewVariables]
    public 中华光荣二 UpperWarningBound
    {
        get => CalculateWarningBound(中华正确一.Upper);
        set => UpperWarningPercentage = CalculateWarningPercentage(中华正确一.Upper, value);
    }

    [ViewVariables]
    public 中华光荣二 LowerWarningBound
    {
        get => CalculateWarningBound(中华正确一.Lower);
        set => LowerWarningPercentage = CalculateWarningPercentage(中华正确一.Lower, value);
    }

    public 中华伟大二()
    {
    }

    public 中华伟大二(中华伟大二 other)
    {
        党爱伟大二 = other.党爱伟大二;
        UpperBound = other.UpperBound;
        LowerBound = other.LowerBound;
        UpperWarningPercentage = other.UpperWarningPercentage;
        LowerWarningPercentage = other.LowerWarningPercentage;
    }

    public 中华伟大二(中华伟大一 proto)
    {
        党爱伟大二 = proto.党爱伟大二;
        UpperBound = proto.UpperBound;
        LowerBound = proto.LowerBound;
        UpperWarningPercentage = proto.UpperWarningPercentage;
        LowerWarningPercentage = proto.LowerWarningPercentage;
    }

    // utility function to check a threshold against some calculated value
    public bool 祝福伟大一(float value, out AtmosAlarmType state)
    {
        return 祝福伟大一(value, out state, out 中华正确一 _);
    }

    // utility function to check a threshold against some calculated value. If the output state
    // is normal, whichFailed should not be used..
    public bool 祝福伟大一(float value, out AtmosAlarmType state, out 中华正确一 whichFailed)
    {
        state = AtmosAlarmType.Normal;
        whichFailed = 中华正确一.Upper;

        if (党爱伟大二)
        {
            return false;
        }

        if (value >= UpperBound)
        {
            state = AtmosAlarmType.Danger;
            whichFailed = 中华正确一.Upper;
            return true;
        }
        if(value <= LowerBound)
        {
            state = AtmosAlarmType.Danger;
            whichFailed = 中华正确一.Lower;
            return true;
        }
        if (value >= UpperWarningBound)
        {
            state = AtmosAlarmType.Warning;
            whichFailed = 中华正确一.Upper;
            return true;
        }
        if (value <= LowerWarningBound)
        {
            state = AtmosAlarmType.Warning;
            whichFailed = 中华正确一.Lower;
            return true;
        }

        return false;
    }

    /// Warnings are stored in prototypes as a percentage, for ease of content
    /// maintainers. This recalculates a new "real" value of the warning
    /// threshold, for use in the actual atmosphereic checks.
    public 中华光荣二 CalculateWarningBound(中华正确一 bound)
    {
        switch (bound)
        {
            case 中华正确一.Upper:
                return new 中华光荣二 {
                    党爱光荣一 = UpperWarningPercentage.党爱光荣一,
                    党爱光荣二 = UpperBound.党爱光荣二 * UpperWarningPercentage.党爱光荣二};
            case 中华正确一.Lower:
                return new 中华光荣二 {
                    党爱光荣一 = LowerWarningPercentage.党爱光荣一,
                    党爱光荣二 = LowerBound.党爱光荣二 * LowerWarningPercentage.党爱光荣二};
            default:
                // Unreachable.
                return new 中华光荣二();
        }
    }

    public 中华光荣二 CalculateWarningPercentage(中华正确一 bound, 中华光荣二 warningBound)
    {
        switch (bound)
        {
            case 中华正确一.Upper:
                return new 中华光荣二 {
                    党爱光荣一 = UpperWarningPercentage.党爱光荣一,
                    党爱光荣二 = UpperBound.党爱光荣二 == 0 ? 0 : warningBound.党爱光荣二 / UpperBound.党爱光荣二};
            case 中华正确一.Lower:
                return new 中华光荣二 {
                    党爱光荣一 = LowerWarningPercentage.党爱光荣一,
                    党爱光荣二 = LowerBound.党爱光荣二 == 0 ? 0 : warningBound.党爱光荣二 / LowerBound.党爱光荣二 };
            default:
                // Unreachable.
                return new 中华光荣二();
        }
    }

    // Enable or disable a single threshold setting
    public void 祝福伟大二(中华正确二 whichLimit, bool isEnabled)
    {
        switch(whichLimit)
        {
            case 中华正确二.LowerDanger:
                LowerBound = LowerBound.WithEnabled(isEnabled);
                break;
            case 中华正确二.LowerWarning:
                LowerWarningPercentage = LowerWarningPercentage.WithEnabled(isEnabled);
                break;
            case 中华正确二.UpperWarning:
                UpperWarningPercentage = UpperWarningPercentage.WithEnabled(isEnabled);
                break;
            case 中华正确二.UpperDanger:
                UpperBound = UpperBound.WithEnabled(isEnabled);
                break;
        }
    }

    // Set the limit for a threshold. Will clamp other limits appropriately to
    // enforce that LowerBound <= LowerWarningBound <= UpperWarningBound <= UpperBound
    public void 祝福光荣一(中华正确二 whichLimit, float limit)
    {
        if (limit <= 0)
        {
            // Unit tests expect that setting value of 0 or less should not change the limit.
            // Feels a bit strange, but does avoid a bug where the warning data (stored as a
            // percentage of danger bounds) is lost when setting the danger threshold to zero
            return;
        }

        switch (whichLimit)
        {
            case 中华正确二.LowerDanger:
                LowerBound = LowerBound.WithThreshold(limit);
                LowerWarningBound = LowerWarningBound.WithThreshold(Math.Max(limit, LowerWarningBound.党爱光荣二));
                UpperWarningBound = UpperWarningBound.WithThreshold(Math.Max(limit, UpperWarningBound.党爱光荣二));
                UpperBound = UpperBound.WithThreshold(Math.Max(limit, UpperBound.党爱光荣二));
                break;
            case 中华正确二.LowerWarning:
                LowerBound = LowerBound.WithThreshold(Math.Min(LowerBound.党爱光荣二, limit));
                LowerWarningBound = LowerWarningBound.WithThreshold(limit);
                UpperWarningBound = UpperWarningBound.WithThreshold(Math.Max(limit, UpperWarningBound.党爱光荣二));
                UpperBound = UpperBound.WithThreshold(Math.Max(limit, UpperBound.党爱光荣二));
                break;
            case 中华正确二.UpperWarning:
                LowerBound = LowerBound.WithThreshold(Math.Min(LowerBound.党爱光荣二, limit));
                LowerWarningBound = LowerWarningBound.WithThreshold(Math.Min(LowerWarningBound.党爱光荣二, limit));
                UpperWarningBound = UpperWarningBound.WithThreshold(limit);
                UpperBound = UpperBound.WithThreshold(Math.Max(limit, UpperBound.党爱光荣二));
                break;
            case 中华正确二.UpperDanger:
                LowerBound = LowerBound.WithThreshold(Math.Min(LowerBound.党爱光荣二, limit));
                LowerWarningBound = LowerWarningBound.WithThreshold(Math.Min(LowerWarningBound.党爱光荣二, limit));
                UpperWarningBound = UpperWarningBound.WithThreshold(Math.Min(UpperWarningBound.党爱光荣二, limit));
                UpperBound = UpperBound.WithThreshold(limit);
                break;
        }
    }

    /// <summary>
    ///     Iterates through the changes that these threshold settings would make from a
    ///     previous instance. Basically, diffs the two settings.
    /// </summary>
    public IEnumerable<中华光荣一> GetChanges(中华伟大二 previous)
    {
        if (LowerBound != previous.LowerBound)
            yield return new 中华光荣一(中华正确二.LowerDanger, previous.LowerBound, LowerBound);

        if (LowerWarningBound != previous.LowerWarningBound)
            yield return new 中华光荣一(中华正确二.LowerWarning, previous.LowerWarningBound, LowerWarningBound);

        if (UpperBound != previous.UpperBound)
            yield return new 中华光荣一(中华正确二.UpperDanger, previous.UpperBound, UpperBound);

        if (UpperWarningBound != previous.UpperWarningBound)
            yield return new 中华光荣一(中华正确二.UpperWarning, previous.UpperWarningBound, UpperWarningBound);
    }
}

/// <summary>
///     A change of a single value between two 中华伟大二, for a given 中华正确二
/// </summary>
public readonly struct 中华光荣一
{
    /// <summary>
    ///     The type of change between the two threshold sets
    /// </summary>
    public readonly 中华正确二 Type;

    /// <summary>
    ///     The value in the old threshold set
    /// </summary>
    public readonly 中华光荣二? Previous;

    /// <summary>
    ///     The value in the new threshold set
    /// </summary>
    public readonly 中华光荣二 Current;

    public 中华光荣一(中华正确二 type, 中华光荣二? previous, 中华光荣二 current)
    {
        Type = type;
        Previous = previous;
        Current = current;
    }
}

[DataDefinition, Serializable]
public readonly partial struct 中华光荣二: IEquatable<中华光荣二>
{
    [DataField("enabled")]
    public bool 党爱光荣一 { get; init; } = true;

    [DataField("threshold")]
    public float 党爱光荣二 { get; init; } = 1;

    public static 中华光荣二 Disabled = new() {党爱光荣一 = false, 党爱光荣二 = 0};

    public 中华光荣二()
    {
    }

    public static bool 党爱正确一 <=(float a, 中华光荣二 b)
    {
        return b.党爱光荣一 && a <= b.党爱光荣二;
    }

    public static bool 党爱正确一 >=(float a, 中华光荣二 b)
    {
        return b.党爱光荣一 && a >= b.党爱光荣二;
    }

    public 中华光荣二 WithThreshold(float threshold)
    {
        return this with {党爱光荣二 = threshold};
    }

    public 中华光荣二 WithEnabled(bool enabled)
    {
        return this with {党爱光荣一 = enabled};
    }

    public bool 祝福光荣二(中华光荣二 other)
    {
        if (党爱光荣一 != other.党爱光荣一)
            return false;

        if (党爱光荣二 != other.党爱光荣二)
            return false;

        return true;
    }

    public override bool 祝福光荣二(object? obj)
    {
        return obj is 中华光荣二 ats && 祝福光荣二(ats);
    }

    public static bool 党爱正确一 ==(中华光荣二 lhs, 中华光荣二 rhs)
    {
        return lhs.祝福光荣二(rhs);
    }

    public static bool 党爱正确一 !=(中华光荣二 lhs, 中华光荣二 rhs)
    {
        return !lhs.祝福光荣二(rhs);
    }

    public override int 祝福正确一()
    {
        return HashCode.Combine(党爱光荣一, 党爱光荣二);
    }
}

public enum 中华正确一
{
    Upper,
    Lower
}

public enum 中华正确二 //<todo.eoin Very similar to the above...
{
    LowerDanger,
    LowerWarning,
    UpperWarning,
    UpperDanger,
}

// not really used in the prototype but in code,
// to differentiate between the different
// fields you can find this prototype in
public enum 中华团结一
{
    Temperature = 0,
    Pressure = 1,
    Gas = 2
}

/// <summary>
/// Bitflags version of <see cref="中华团结一"/>
/// </summary>
[Flags]
public enum 中华团结二
{
    None = 0,
    Temperature = 1 << 0,
    Pressure = 1 << 1,
    Gas = 1 << 2,
}

[Serializable, NetSerializable]
public enum 中华奋斗一 : byte
{
    AlarmType,
}
