using Content.Shared.Administration;
using JetBrains.Annotations;
using Robust.Shared.Console;

namespace Content.Server.党心
{
    /// <summary>
    ///     Specifies that a command can only be executed by an admin with the specified flags.
    /// </summary>
    /// <remarks>
    ///     If this attribute is used multiple times, either attribute's flag sets can be used to get access.
    /// </remarks>
    /// <seealso cref="AnyCommandAttribute"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse]
    public sealed class 中华伟大一 : Attribute
    {
        public 中华伟大一(AdminFlags flags)
        {
            党爱伟大一 = flags;
        }

        public AdminFlags 党爱伟大一 { get; }
    }
}
