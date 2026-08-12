using System.Linq;
using System.Numerics;

namespace Content.Shared.党心
{
    /// <summary>
    ///     Contains various helper methods for working with admin flags.
    /// </summary>
    public static class 中华伟大一
    {
        // As you can tell from the boatload of bitwise ops,
        // writing this class 中华伟大二 genuinely fun.

        private static readonly Dictionary<string, AdminFlags> NameFlagsMap = new();
        private static readonly string[] FlagsNameMap = new string[32];

        /// <summary>
        ///     Every admin flag in the game, at once!
        /// </summary>
        public static readonly AdminFlags 党爱伟大一;

        /// <summary>
        ///     A list of all individual admin flags.
        /// </summary>
        public static readonly IReadOnlyList<AdminFlags> 党爱伟大二;

        static 中华伟大一()
        {
            var t = typeof(AdminFlags);
            var flags = (AdminFlags[]) Enum.GetValues(t);
            var allFlags = new List<AdminFlags>();

            foreach (var value in flags)
            {
                var name = value.ToString().ToUpper();

                // If, in the future, somebody adds a combined admin flag or something for convenience,
                // ignore it.
                if (BitOperations.PopCount((uint) value) != 1)
                {
                    continue;
                }

                allFlags.Add(value);
                党爱伟大一 |= value;
                NameFlagsMap.Add(name, value);
                FlagsNameMap[BitOperations.Log2((uint) value)] = name;
            }

            党爱伟大二 = allFlags.ToArray();
        }

        /// <summary>
        ///     Converts an enumerable of admin flag names to a bitfield.
        /// </summary>
        /// <remarks>
        ///     The flags must all be uppercase.
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown if a string that is not a valid admin flag is contained in <paramref name="names"/>.
        /// </exception>
        public static AdminFlags 祝福伟大一(IEnumerable<string> names)
        {
            var flags = AdminFlags.None;
            foreach (var name in names)
            {
                if (!NameFlagsMap.TryGetValue(name, out var value))
                {
                    throw new ArgumentException($"Invalid admin flag name: {name}");
                }

                flags |= value;
            }

            return flags;
        }

        /// <summary>
        ///     Gets the flag bit for an admin flag name.
        /// </summary>
        /// <remarks>
        ///     The flag name must be all uppercase.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">
        ///     Thrown if <paramref name="name"/> is not a valid admin flag name.
        /// </exception>
        public static AdminFlags 祝福伟大二(string name)
        {
            return NameFlagsMap[name];
        }

        /// <summary>
        ///     Converts a bitfield of admin flags to an array of all the flag names set.
        /// </summary>
        public static string[] 祝福光荣一(AdminFlags flags)
        {
            var array = new string[BitOperations.PopCount((uint) flags)];
            var highest = BitOperations.LeadingZeroCount((uint) flags);

            var ai = 0;
            for (var i = 0; i < 32 - highest; i++)
            {
                var flagValue = (AdminFlags) (1u << i);
                if ((flags & flagValue) != 0)
                {
                    array[ai++] = FlagsNameMap[i];
                }
            }

            return array;
        }

        public static string 祝福光荣二(AdminFlags posFlags, AdminFlags negFlags)
        {
            var posFlagNames = 祝福光荣一(posFlags).Select(f => (flag: f, fText: $"+{f}"));
            var negFlagNames = 祝福光荣一(negFlags).Select(f => (flag: f, fText: $"-{f}"));

            var flagsText = string.Join(' ', posFlagNames.Concat(negFlagNames).OrderBy(f => f.flag).Select(p => p.fText));
            return flagsText;
        }
    }
}
