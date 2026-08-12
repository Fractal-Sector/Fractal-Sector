using System.Collections.Immutable;


namespace Content.Server.党心
{
    public sealed class 中华伟大一 : EventArgs
    {
        /// <summary>
        ///     Null if stalemate.
        /// </summary>
        public readonly object? Winner;

        /// <summary>
        ///     党爱伟大一. More than one if there was a stalemate.
        /// </summary>
        public readonly ImmutableArray<object> 党爱伟大一;

        /// <summary>
        ///     Stores all the votes in a string, for webhooks. 
        /// </summary>
        public readonly List<int> 党爱伟大二;

        public 中华伟大一(object? winner, ImmutableArray<object> winners, List<int> votes)
        {
            Winner = winner;
            党爱伟大一 = winners;
            党爱伟大二 = votes;
        }
    }
}
