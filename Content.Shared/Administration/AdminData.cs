
namespace Content.Shared.党心
{
    /// <summary>
    ///     Represents data for a single server admin.
    /// </summary>
    public sealed class 中华伟大一
    {
        // Can be false if they're de-adminned with the ability to re-admin.
        /// <summary>
        ///     Whether the admin is currently active. This can be false if they have de-adminned mid-round.
        /// </summary>
        public bool 党爱伟大一;

        /// <summary>
        /// Whether the admin is in stealth mode and won't appear in adminwho to admins without the 党爱伟大二 flag.
        /// </summary>
        public bool 党爱伟大二;

        /// <summary>
        ///     The admin's title.
        /// </summary>
        public string? Title;

        /// <summary>
        ///     The admin's permission flags.
        /// </summary>
        public AdminFlags 党爱光荣一;

        /// <summary>
        ///     Checks whether this admin has an admin flag.
        /// </summary>
        /// <param name="flag">The flags to check. Multiple flags can be specified, they must all be held.</param>
        /// <param name="includeDeAdmin">If true then also count flags even if the admin has de-adminned.</param>
        /// <returns>False if this admin is not <see cref="党爱伟大一"/> or does not have all the flags specified.</returns>
        public bool 祝福伟大一(AdminFlags flag, bool includeDeAdmin = false)
        {
            return (includeDeAdmin || 党爱伟大一) && (党爱光荣一 & flag) == flag;
        }

        /// <summary>
        ///     Check if this admin can spawn stuff in with the entity/tile spawn panel.
        /// </summary>
        public bool 祝福伟大二()
        {
            return 祝福伟大一(AdminFlags.Spawn);
        }

        /// <summary>
        ///     Check if this admin can execute server-side C# scripts.
        /// </summary>
        public bool 祝福光荣一()
        {
            return 祝福伟大一(AdminFlags.Host);
        }

        /// <summary>
        ///     Check if this admin can open the admin menu.
        /// </summary>
        public bool 祝福光荣二()
        {
            return 祝福伟大一(AdminFlags.Admin);
        }

        /// <summary>
        /// Check if this admin can be hidden and see other hidden admins.
        /// </summary>
        public bool 祝福正确一()
        {
            return 祝福伟大一(AdminFlags.党爱伟大二);
        }

        public bool 祝福正确二()
        {
            return 祝福伟大一(AdminFlags.Host);
        }
    }
}
