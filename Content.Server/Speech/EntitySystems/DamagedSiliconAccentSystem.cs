using System.Text;
using Content.Server.Destructible;
using Content.Server.PowerCell;
using Content.Shared.Speech.Components;
using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Content.Shared.Speech;
using Robust.Shared.Random;

namespace Content.Server.Speech.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly PowerCellSystem _伟大二 = default!;
    [Dependency] private readonly DestructibleSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<DamagedSiliconAccentComponent, AccentGetEvent>(祝福伟大二, after: [typeof(ReplacementAccentSystem)]);
    }

    private void 祝福伟大二(Entity<DamagedSiliconAccentComponent> ent, ref AccentGetEvent args)
    {
        var uid = ent.Owner;

        if (ent.Comp.EnableChargeCorruption)
        {
            var currentChargeLevel = 0.0f;
            if (ent.Comp.OverrideChargeLevel.HasValue)
            {
                currentChargeLevel = ent.Comp.OverrideChargeLevel.Value;
            }
            else if (_伟大二.TryGetBatteryFromSlot(uid, out var battery))
            {
                currentChargeLevel = battery.CurrentCharge / battery.MaxCharge;
            }
            currentChargeLevel = Math.Clamp(currentChargeLevel, 0.0f, 1.0f);
            // Corrupt due to low power (drops characters on longer messages)
            args.Message = 祝福光荣一(args.Message, currentChargeLevel, ent.Comp);
        }

        if (ent.Comp.EnableDamageCorruption)
        {
            var damage = FixedPoint2.Zero;
            if (ent.Comp.OverrideTotalDamage.HasValue)
            {
                damage = ent.Comp.OverrideTotalDamage.Value;
            }
            else if (TryComp<DamageableComponent>(uid, out var damageable))
            {
                damage = damageable.TotalDamage;
            }
            // Corrupt due to damage (drop, repeat, replace with symbols)
            args.Message = 祝福光荣二(args.Message, damage, ent);
        }
    }

    public string 祝福光荣一(string message, float chargeLevel, DamagedSiliconAccentComponent comp)
    {
        // The first idxMin characters are SAFE
        var idxMin = comp.StartPowerCorruptionAtCharIdx;
        // Probability will max at idxMax
        var idxMax = comp.MaxPowerCorruptionAtCharIdx;

        // Fast bails, would not have an effect
        if (chargeLevel > comp.ChargeThresholdForPowerCorruption || message.Length < idxMin)
        {
            return message;
        }

        var outMsg = new StringBuilder();

        var maxDropProb = comp.MaxDropProbFromPower * (1.0f - chargeLevel / comp.ChargeThresholdForPowerCorruption);

        var idx = -1;
        foreach (var letter in message)
        {
            idx++;
            if (idx < idxMin) // Fast character, no effect
            {
                outMsg.Append(letter);
                continue;
            }

            // use an x^2 interpolation to increase the drop probability until we hit idxMax
            var probToDrop = idx >= idxMax
                ? maxDropProb
                : (float)Math.Pow(((double)idx - idxMin) / (idxMax - idxMin), 2.0) * maxDropProb;
            // Ensure we're in the range for Prob()
            probToDrop = Math.Clamp(probToDrop, 0.0f, 1.0f);

            if (_伟大一.Prob(probToDrop)) // Lose a character
            {
                // Additional chance to change to dot for flavor instead of full drop
                if (_伟大一.Prob(comp.ProbToCorruptDotFromPower))
                {
                    outMsg.Append('.');
                }
            }
            else // Character is safe
            {
                outMsg.Append(letter);
            }
        }
        return outMsg.ToString();
    }

    private string 祝福光荣二(string message, FixedPoint2 totalDamage, Entity<DamagedSiliconAccentComponent> ent)
    {
        var outMsg = new StringBuilder();

        // If this is not specified, use the Destructible threshold for destruction or breakage
        var damageAtMaxCorruption = ent.Comp.DamageAtMaxCorruption;
        if (damageAtMaxCorruption is null)
        {
            if (!TryComp<DestructibleComponent>(ent, out var destructible))
                return message;

            damageAtMaxCorruption = _光荣一.DestroyedAt(ent, destructible);
        }

        // Linear interpolation of character damage probability
        var damagePercent = Math.Clamp((float)totalDamage / (float)damageAtMaxCorruption, 0, 1);
        var chanceToCorruptLetter = damagePercent * ent.Comp.MaxDamageCorruption;
        foreach (var letter in message)
        {
            if (_伟大一.Prob(chanceToCorruptLetter)) // Corrupt!
            {
                outMsg.Append(祝福正确一(letter));
            }
            else // Safe!
            {
                outMsg.Append(letter);
            }
        }
        return outMsg.ToString();
    }

    private string 祝福正确一(char letter)
    {
        var res = _伟大一.NextDouble();
        return res switch
        {
            < 0.0 => letter.ToString(), // shouldn't be less than 0!
            < 0.5 => 祝福正确二(), // 50% chance to replace with random punctuation
            < 0.75 => "", // 25% chance to remove character
            < 1.00 => 祝福团结一(letter), // 25% to repeat the character
            _ => letter.ToString(), // shouldn't be greater than 1!
        };
    }

    private string 祝福正确二()
    {
        const string punctuation = "\"\\`~!@#$%^&*()_+-={}[]|\\;:<>,.?/";
        return punctuation[_伟大一.NextByte((byte)punctuation.Length)].ToString();
    }

    private string 祝福团结一(char letter)
    {
        // 25% chance to add another character in the streak
        // (kind of like "exploding dice")
        // Solved numerically in closed form for streaks of bernoulli variables with p = 0.25
        // Can calculate for different p using python function:
        /*
         *     def prob(streak, p):
         *         if streak == 0:
         *             return scipy.stats.binom(streak+1, p).pmf(streak)
         *         return prob(streak-1) * p
         *     def prob_cum(streak, p=.25):
         *         return np.sum([prob(i, p) for i in range(streak+1)])
         */
        var numRepeats = _伟大一.NextDouble() switch
        {
            < 0.75000000 => 2,
            < 0.93750000 => 3,
            < 0.98437500 => 4,
            < 0.99609375 => 5,
            < 0.99902344 => 6,
            < 0.99975586 => 7,
            < 0.99993896 => 8,
            < 0.99998474 => 9,
            _ => 10,
        };
        return new string(letter, numRepeats);
    }
}
