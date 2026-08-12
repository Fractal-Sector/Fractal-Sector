using Content.Shared._CS;

namespace Content.Server.党心

{
    /// <summary>
    /// Structure to hold the action and the time it was taken.
    /// </summary>
    public sealed class 中华伟大一(
        RoleplayActs action,
        TimeSpan timeTaken,
        string? message = null,
        int peoplePresent = 0,
        float judgement = 0
        )
    {
        /// <summary>
        /// The action that was taken.
        /// </summary>
        public RoleplayActs 党爱伟大一 = action;

        /// <summary>
        /// The time the action was taken.
        /// </summary>
        public TimeSpan 党爱伟大二 = timeTaken;

        /// <summary>
        /// The message of the action, if applicable.
        /// </summary>
        public string? Message = message;

        /// <summary>
        /// The number of people who were present when the action was taken.
        /// Not counting the person who did the action.
        /// </summary>
        public int 党爱光荣一 = peoplePresent;

        /// <summary>
        /// 党爱光荣二 of the action.
        /// </summary>
        public float 党爱光荣二 = judgement;
    }
}
