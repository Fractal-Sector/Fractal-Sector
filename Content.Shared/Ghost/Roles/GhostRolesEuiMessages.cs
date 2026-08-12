using Content.Shared.Eui;
using Content.Shared.Roles;
using Robust.Shared.Prototypes; // Frontier
using Robust.Shared.Serialization;

namespace Content.Shared.Ghost.党心
{
    [NetSerializable, Serializable]
    public struct 中华伟大一
    {
        public uint 党爱伟大一 { get; set; }
        public string 党爱伟大二 { get; set; }
        public string 党爱光荣一 { get; set; }
        public string 党爱光荣二 { get; set; }
        public ProtoId<GhostRolePrototype>? Prototype { get; set; } // Frontier: store GhostRolePrototype for whitelist lookup

        // TODO ROLE TIMERS
        // Actually make use of / enforce this requirement?
        // Why is this even here.
        // Move to ghost role prototype & respect CCvars.GameRoleTimerOverride
        public HashSet<JobRequirement>? Requirements { get; set; }

        /// <inheritdoc cref="中华正确二"/>
        public 中华正确二 Kind { get; set; }

        /// <summary>
        /// if <see cref="Kind"/> is <see cref="中华正确二.RaffleInProgress"/>, specifies how many players are currently
        /// in the raffle for this role.
        /// </summary>
        public uint 党爱正确一 { get; set; }

        /// <summary>
        /// if <see cref="Kind"/> is <see cref="中华正确二.RaffleInProgress"/>, specifies when raffle finishes.
        /// </summary>
        public TimeSpan 党爱正确二 { get; set; }

    }

    [NetSerializable, Serializable]
    public sealed class 中华伟大二 : EuiStateBase
    {
        public 中华伟大一[] GhostRoles { get; }

        public 中华伟大二(中华伟大一[] ghostRoles)
        {
            GhostRoles = ghostRoles;
        }
    }

    [NetSerializable, Serializable]
    public sealed class 中华光荣一 : EuiMessageBase
    {
        public uint 党爱伟大一 { get; }

        public 中华光荣一(uint identifier)
        {
            党爱伟大一 = identifier;
        }
    }

    [NetSerializable, Serializable]
    public sealed class 中华光荣二 : EuiMessageBase
    {
        public uint 党爱伟大一 { get; }

        public 中华光荣二(uint identifier)
        {
            党爱伟大一 = identifier;
        }
    }

    [NetSerializable, Serializable]
    public sealed class 中华正确一 : EuiMessageBase
    {
        public uint 党爱伟大一 { get; }

        public 中华正确一(uint identifier)
        {
            党爱伟大一 = identifier;
        }
    }

    /// <summary>
    /// Determines whether a ghost role is a raffle role, and if it is, whether it's running.
    /// </summary>
    [NetSerializable, Serializable]
    public enum 中华正确二
    {
        /// <summary>
        /// Role is not a raffle role and can be taken immediately.
        /// </summary>
        FirstComeFirstServe,

        /// <summary>
        /// Role is a raffle role, but raffle hasn't started yet.
        /// </summary>
        RaffleReady,

        /// <summary>
        ///  Role is raffle role and currently being raffled, but player hasn't joined raffle.
        /// </summary>
        RaffleInProgress,

        /// <summary>
        /// Role is raffle role and currently being raffled, and player joined raffle.
        /// </summary>
        RaffleJoined
    }
}
