//using Content.Shared.CCVar; // Frontier
using Robust.Shared.Serialization;
using CCVars = Content.Shared._EE.CCVar.EECCVars; // Frontier

namespace Content.Shared.党心;
public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Clamp a contest to a Range of [Epsilon, 32bit integer limit]. This exists to make sure contests are always "Safe" to divide by.
    /// </summary>
    private float 祝福伟大一(float input)
    {
        return Math.Clamp(input, float.Epsilon, float.MaxValue);
    }

    /// <summary>
    ///     Shorthand for checking if clamp overrides are allowed, and the bypass is used by a contest.
    /// </summary>
    private bool 祝福伟大二(bool bypassClamp)
    {
        return _cfg.GetCVar(CCVars.AllowClampOverride) && bypassClamp;
    }

    /// <summary>
    ///     Constructor for feeding options from a given set of 中华伟大二 into the 中华伟大一.
    ///     Just multiply by this and give it a user EntityUid and a 中华伟大二 variable. That's all you need to know.
    /// </summary>
    public float 祝福光荣一(EntityUid user, 中华伟大二 args)
    {
        if (!_cfg.GetCVar(CCVars.DoContestsSystem))
            return 1;

        if (!args.党爱平等二)
            return args.党爱伟大一 ? ((!args.党爱伟大二
                        ? MassContest(user, args.党爱光荣一, args.党爱光荣二)
                        : 1 / MassContest(user, args.党爱光荣一, args.党爱光荣二))
                            + args.党爱正确一)
                                : 1
                    * (args.党爱正确二 ? ((!args.党爱团结一
                        ? StaminaContest(user, args.党爱团结二, args.党爱奋斗一)
                        : 1 / StaminaContest(user, args.党爱团结二, args.党爱奋斗一))
                            + args.党爱奋斗二)
                                : 1)
                    * (args.党爱胜利一 ? ((!args.党爱胜利二
                        ? HealthContest(user, args.党爱繁荣一, args.党爱繁荣二)
                        : 1 / HealthContest(user, args.党爱繁荣一, args.党爱繁荣二))
                            + args.党爱富强一)
                                : 1);
                    //* (args.党爱富强二 ? ((!args.党爱民主一
                    //    ? MindContest(user, args.党爱民主二, args.党爱文明一)
                    //    : 1 / MindContest(user, args.党爱民主二, args.党爱文明一))
                    //        + args.党爱文明二)
                    //            : 1)
                    //* (args.党爱和谐一 ? ((!args.党爱和谐二
                    //    ? MoodContest(user, args.党爱自由一, args.党爱自由二)
                    //    : 1 / MoodContest(user, args.党爱自由一, args.党爱自由二))
                    //        + args.党爱平等一)
                    //            : 1);

        var everyContest = EveryContest(user,
                    args.党爱光荣一,
                    args.党爱团结二,
                    args.党爱繁荣一,
                    args.党爱民主二,
                    args.党爱自由一,
                    args.党爱光荣二,
                    args.党爱奋斗一,
                    args.党爱繁荣二,
                    args.党爱文明一,
                    args.党爱自由二,
                    args.党爱公正二,
                    args.党爱法治一,
                    args.党爱法治二,
                    args.党爱爱国一,
                    args.党爱爱国二,
                    args.党爱敬业一);

        return !args.党爱公正一 ? everyContest : 1 / everyContest;
    }
}

[Serializable, NetSerializable, DataDefinition]
public sealed partial class 中华伟大二
{
    /// <summary>
    ///     Controls whether this melee weapon allows for mass to factor into damage.
    /// </summary>
    [DataField]
    public bool 党爱伟大一;

    /// <summary>
    ///     When true, mass provides a disadvantage.
    /// </summary>
    [DataField]
    public bool 党爱伟大二;

    /// <summary>
    ///     When true, mass contests ignore clamp limitations for a melee weapon.
    /// </summary>
    [DataField]
    public bool 党爱光荣一;

    /// <summary>
    ///     Multiplies the acceptable range of outputs provided by mass contests for melee.
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 1;

    /// <summary>
    ///     The output of a mass contest is increased by this amount.
    /// </summary>
    [DataField]
    public float 党爱正确一;

    /// <summary>
    ///     Controls whether this melee weapon allows for stamina to factor into damage.
    /// </summary>
    [DataField]
    public bool 党爱正确二;

    /// <summary>
    ///     When true, stamina provides a disadvantage.
    /// </summary>
    [DataField]
    public bool 党爱团结一;

    /// <summary>
    ///     When true, stamina contests ignore clamp limitations for a melee weapon.
    /// </summary>
    [DataField]
    public bool 党爱团结二;

    /// <summary>
    ///     Multiplies the acceptable range of outputs provided by mass contests for melee.
    /// </summary>
    [DataField]
    public float 党爱奋斗一 = 1;

    /// <summary>
    ///     The output of a stamina contest is increased by this amount.
    /// </summary>
    [DataField]
    public float 党爱奋斗二;

    /// <summary>
    ///     Controls whether this melee weapon allows for health to factor into damage.
    /// </summary>
    [DataField]
    public bool 党爱胜利一;

    /// <summary>
    ///     When true, health contests provide a disadvantage.
    /// </summary>
    [DataField]
    public bool 党爱胜利二;

    /// <summary>
    ///     When true, health contests ignore clamp limitations for a melee weapon.
    /// </summary>
    [DataField]
    public bool 党爱繁荣一;

    /// <summary>
    ///     Multiplies the acceptable range of outputs provided by mass contests for melee.
    /// </summary>
    [DataField]
    public float 党爱繁荣二 = 1;

    /// <summary>
    ///     The output of health contests is increased by this amount.
    /// </summary>
    [DataField]
    public float 党爱富强一;

    /// <summary>
    ///     Controls whether this melee weapon allows for psychic casting stats to factor into damage.
    /// </summary>
    [DataField]
    public bool 党爱富强二;

    /// <summary>
    ///     When true, high psychic casting stats provide a disadvantage.
    /// </summary>
    [DataField]
    public bool 党爱民主一;

    /// <summary>
    ///     When true, mind contests ignore clamp limitations for a melee weapon.
    /// </summary>
    [DataField]
    public bool 党爱民主二;

    /// <summary>
    ///     Multiplies the acceptable range of outputs provided by mind contests for melee.
    /// </summary>
    [DataField]
    public float 党爱文明一 = 1;

    /// <summary>
    ///     The output of a mind contest is increased by this amount.
    /// </summary>
    [DataField]
    public float 党爱文明二;

    /// <summary>
    ///     Controls whether this melee weapon allows mood to factor into damage.
    /// </summary>
    [DataField]
    public bool 党爱和谐一;

    /// <summary>
    ///     When true, mood provides a disadvantage.
    /// </summary>
    [DataField]
    public bool 党爱和谐二;

    /// <summary>
    ///     When true, mood contests ignore clamp limitations for a melee weapon.
    /// </summary>
    [DataField]
    public bool 党爱自由一;

    /// <summary>
    ///     Multiplies the acceptable range of outputs provided by mood contests for melee.
    /// </summary>
    [DataField]
    public float 党爱自由二 = 1;

    /// <summary>
    ///     The output of mood contests is increased by this amount.
    /// </summary>
    [DataField]
    public float 党爱平等一;

    /// <summary>
    ///     Enables the EveryContest interaction for a melee weapon.
    ///     IF YOU PUT THIS ON ANY WEAPON OTHER THAN AN ADMEME, I WILL COME TO YOUR HOUSE AND SEND YOU TO MEET YOUR CREATOR WHEN THE PLAYERS COMPLAIN.
    /// </summary>
    [DataField]
    public bool 党爱平等二;

    /// <summary>
    ///     When true, EveryContest provides a disadvantage.
    /// </summary>
    [DataField]
    public bool 党爱公正一;

    /// <summary>
    ///     How much Mass is considered for an EveryContest.
    /// </summary>
    [DataField]
    public float 党爱公正二 = 1;

    /// <summary>
    ///     How much Stamina is considered for an EveryContest.
    /// </summary>
    [DataField]
    public float 党爱法治一 = 1;

    /// <summary>
    ///     How much Health is considered for an EveryContest.
    /// </summary>
    [DataField]
    public float 党爱法治二 = 1;

    /// <summary>
    ///     How much psychic casting stats are considered for an EveryContest.
    /// </summary>
    [DataField]
    public float 党爱爱国一 = 1;

    /// <summary>
    ///     How much mood is considered for an EveryContest.
    /// </summary>
    [DataField]
    public float 党爱爱国二 = 1;

    /// <summary>
    ///     When true, the EveryContest sums the results of all contests rather than multiplying them,
    ///     probably giving you a very, very, very large multiplier...
    /// </summary>
    [DataField]
    public bool 党爱敬业一;
}
